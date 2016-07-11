using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace migratorGui
{
    public partial class Form1 : Form
    {
        int one;
        int two;
        int three;
        int four;

        public Form1()
        {
            InitializeComponent();

            exportFolder.Text = @"N:\dev\starHtmlMigrator\LocalOnly";

            one = fileList.Left;
            two = findFilesButton.Left - fileList.Right;
            three = processedFiles.Left - findFilesButton.Right;
            four = Width - processedFiles.Right;
        }

        private int _messageCounter;
        private List<string> FilePaths { get; set; }

        private string FolderPath { get; set; }

        void AddMessage(string message)
        {
            messages.Text = $"{++_messageCounter}. {message}{Environment.NewLine}" + messages.Text;
        }

        void StartSession()
        {
            AddMessage($"Starting session at {GetNow()}");
        }

        private string GetAncestor(string originalFolder, int generations)
        {
            var path = originalFolder;
            for (var i = 0; i < generations; i++)
            {
                path = Directory.GetParent(path).ToString();
            }

            return path;
        }

        private string GetBootstrapCode()
        {
            var exeFolder = Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty;
            var assetsFolderPath = GetAncestor(exeFolder, 3);
            var assetsFolderUri = new Uri(assetsFolderPath).AbsoluteUri;
            var suffix = ToJSON.Checked ? "JSON" : ToRIS.Checked ? "RIS" : null;

            var bootstrapCode = $"<link href='{assetsFolderUri}/migrator.css' rel='stylesheet' type='text/css'>" +
                                "<script src = 'http://code.jquery.com/jquery-3.0.0.min.js' " +
                                "integrity='sha256-JmvOoLtYsmqlsWxa7mDSLMwa6dZ9rrIdtrrVYRnDRH0=' " +
                                "crossorigin='anonymous'" +
                                "></script>" +
                                $"<script src = '{assetsFolderUri}/migrator_{suffix}.js'></script>";
            return bootstrapCode;
        }

        private void findFilesButton_Click(object sender, EventArgs e)
        {
            fileList.Items.Clear();
            StartSession();

            if (!Directory.Exists(FolderPath))
            {
                AddMessage("Folder doesn't exist. Aborting.");
                return;
            }
            FilePaths = Directory.GetFiles(FolderPath)
                .Where(f => f.ToLower().EndsWith(".html"))
                .ToList();
            if (!FilePaths.Any())
            {
                AddMessage($"No html files found at \"{FolderPath}\". Aborting.");
                return;
            }
            processFilesButton.Enabled = true;

            var files = FilePaths
                .Select(Path.GetFileName)
                .ToList();
            
            files.ForEach(f => fileList.Items.Add(f));

            AddMessage($"Files found. Please press the 'Add Bootstrap' button.");
        }

        string GetNow()
        {
            return DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
        }


        static Regex reRemoveCssAndScript = new Regex(
            "(?ims)(?<script>\\<script.+?\\<\\/script\\>)|(?<style>\\<link.+?type=\\\"text/css\"[^>]*\\>)"
        );
        private void processFiles_Click(object sender, EventArgs e)
        {
            var selectedFiles = fileList.SelectedIndices.Cast<int>()
                .Select(i => FilePaths[i])
                .ToList();

            selectedFiles = FilePaths;

            var outputFolderPath = Path.Combine(
                FolderPath, 
                GetNow()
            );
            var prefix = $"{FolderPath}\\";
            processedFiles.Items.Clear();
            Directory.CreateDirectory(outputFolderPath);
            selectedFiles.ForEach(f =>
            {
                var outputFilePath = Path.Combine(
                    outputFolderPath,
                    Path.GetFileName(f)
                );
                var input = File.ReadAllText(f);
                var output =
                    reRemoveCssAndScript.Replace(input, string.Empty)
                    .Replace("<head>", "<head>" + GetBootstrapCode());

                File.WriteAllText(outputFilePath, output);

                processedFiles.Items.Add(outputFilePath.Replace(prefix, string.Empty));
            });
            var relPath = outputFolderPath.Replace(prefix, string.Empty);
            AddMessage($"Modified files written to newly-created sub-folder: \"{relPath}\". Please open them in your browser.");

            Process.Start(outputFolderPath);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            var availWidth = Width 
                - findFilesButton.Width 
                - one - two - three - four;

            var a = availWidth/2;
            var b = availWidth - a;

            var deltaA = a - fileList.Width;
            var deltaB = b - processedFiles.Width;

            new Control[]
            {
                findFilesButton,
                processFilesButton,
                processedFiles
            }.ForEach(c => c.Left += deltaA);

            fileList.Width += deltaA;
            processedFiles.Width += deltaB;
        }

        private void exportFolder_TextChanged(object sender, EventArgs e)
        {
            processFilesButton.Enabled = false;
            FolderPath = exportFolder.Text.Trim();
        }
    }
}
