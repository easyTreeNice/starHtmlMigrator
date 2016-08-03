using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

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

            //exportFolder.Text = @"N:\dev\starHtmlMigrator\LocalOnly";
            exportFolder.Text = Properties.Settings.Default.RootFolder;
            ToJSON.Checked = !Properties.Settings.Default.ToRIS;
            ToRIS.Checked = Properties.Settings.Default.ToRIS;
            JsonIdentifierPattern.Text = Properties.Settings.Default.JsonIdentifierPattern;
            RisIdentifierPattern.Text = Properties.Settings.Default.RisIdentifierPattern;

            one = fileList.Left;
            two = findFilesButton.Left - fileList.Right;
            three = processedFiles.Left - findFilesButton.Right;
            four = Width - processedFiles.Right;
        }

        private int _messageCounter;
        private List<string> InputFilePaths { get; set; }
        private List<string> OutputFilePaths { get; set; }

        private string FolderPath { get; set; }

        void AddMessage(string message)
        {
            messages.Text = $"{++_messageCounter}. {message}{Environment.NewLine}" + messages.Text;
            Thread.Sleep(10);
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
            var assetsFolderPath = GetAncestor(exeFolder, Properties.Settings.Default.AssetFolderAncestorGenerations);
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

        private enum Flavour
        {
            None = 0,
            Json = 1,
            Ris = 2
        }

        private Flavour GetFileFlavour(string filePath)
        {
            var flavour = Flavour.None;

            if (filePath.Contains(Properties.Settings.Default.JsonIdentifierPattern))
            {
                flavour = Flavour.Json;
            }
            else if (filePath.Contains(Properties.Settings.Default.RisIdentifierPattern))
            {
                flavour = Flavour.Ris;
            }

            return flavour;
        }

        private void findFilesButton_Click(object sender, EventArgs e)
        {
            DoFindFiles(recursive: false);
        }

        private void DoFindFiles(bool recursive, bool usePatterns = false)
        {
            Properties.Settings.Default.RootFolder = exportFolder.Text.Trim();
            Properties.Settings.Default.Save();

            fileList.Items.Clear();
            StartSession();

            if (!Directory.Exists(FolderPath))
            {
                AddMessage("Folder doesn't exist. Aborting.");
                return;
            }

            InputFilePaths = Directory.GetFiles(
                FolderPath,
                "*.html",
                recursive
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly
                ).ToList();

            if (!InputFilePaths.Any())
            {
                AddMessage($"No html files found at \"{FolderPath}\". Aborting.");
                return;
            }
            processFilesButton.Enabled = true;

            var prefix = $"{FolderPath}\\";

            var files = InputFilePaths
                .Select(f => f.Replace(prefix, string.Empty))
                .ToList();

            files.ForEach(f =>
            {
                var flavour = GetFileFlavour(f).ToString();
                fileList.Items.Add($"[{flavour}] {f}");
            });

            var nextButtonName = recursive
                ? runAutomatedButton.Text
                : processFilesButton.Text;

            AddMessage($"Files found. Please press the '{nextButtonName}' button.");
        }

        string GetNow()
        {
            return DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
        }


        static Regex reRemoveCssAndScript = new Regex(
            "(?ims)(?<script>\\<script.+?\\<\\/script\\>)|(?<style>\\<link.+?type=\\\"text/css\"[^>]*\\>)"
        );

        private ChromeDriver _driver;

        private void processFiles_Click(object sender, EventArgs e)
        {
            DoAddBootstrap();
        }

        private void DoAddBootstrap()
        {
            Properties.Settings.Default.ToRIS = ToRIS.Checked;
            Properties.Settings.Default.Save();

            var selectedFiles = fileList.SelectedIndices.Cast<int>()
                .Select(i => InputFilePaths[i])
                .ToList();

            selectedFiles = InputFilePaths;

            var prefix = $"{FolderPath}\\";
            processedFiles.Items.Clear();
            selectedFiles.ForEach(f =>
            {
                var fileFolderPath = Path.GetDirectoryName(f);
                var outputFolderPath = Path.Combine(
                    fileFolderPath,
                    "Processed",
                    GetNow()
                );
                Directory.CreateDirectory(outputFolderPath);
                var outputFilePath = Path.Combine(
                    outputFolderPath,
                    Path.GetFileName(f)
                    );
                var input = File.ReadAllText(f);
                var output =
                    reRemoveCssAndScript.Replace(input, string.Empty)
                        .Replace("<head>", "<head>" + GetBootstrapCode());

                File.WriteAllText(outputFilePath, output);

                OutputFilePaths = new List<string>();
                OutputFilePaths.Add(outputFilePath);

                var flavour = GetFileFlavour(f).ToString();
                var relative = outputFilePath.Replace(prefix, string.Empty);
                var listEntry = $"[{flavour}] {relative}";

                processedFiles.Items.Add(listEntry);
            });
            //var relPath = outputFolderPath.Replace(prefix, string.Empty);
            //AddMessage($"Modified files written to newly-created sub-folder: \"{relPath}\". Please open them in your browser.");

            //Process.Start(outputFolderPath);
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
                processedFiles,
                groupBox1,
                groupBox2
            }.ForEach(c => c.Left += deltaA);

            fileList.Width += deltaA;
            exportFolder.Width += deltaA;
            processedFiles.Width += deltaB;
        }

        private void exportFolder_TextChanged(object sender, EventArgs e)
        {
            processFilesButton.Enabled = false;
            FolderPath = exportFolder.Text.Trim();
        }

        private void runAutomatedButton_Click(object sender, EventArgs e)
        {
            var options = new ChromeOptions();
            var service = ChromeDriverService.CreateDefaultService();
            try
            {
                service.Start();

                _driver = new ChromeDriver(service, options);

                OutputFilePaths.ForEach(AutoProcessFile);
            }
            finally
            {
                _driver.Quit();
                service.Dispose();
            }
        }

        private void AutoProcessFile(string filePath)
        {
            switch (GetFileFlavour(filePath))
            {
                case Flavour.Json:
                    //var filePath = @"N:\dev\starHtmlMigrator\LocalOnly\For JSON extraction\Eds_Test_files\2016.07.26-15.30.27\1000065.html";
                    AddMessage($"[Start]  [JSON] Processing file '{filePath}'");
                    GetJsonAndStoreInOutputFile(filePath);
                    AddMessage($"[Finish] [JSON] Processing file '{filePath}'");
                    break;
                case Flavour.Ris:
                    // var filePath = @"N:\dev\starHtmlMigrator\LocalOnly\For RIS extraction\2016.08.02-12.23.15\simpyNamed.html";
                    AddMessage($"[Start]  [RIS]  Processing file '{filePath}'");
                    GetRisAndStoreInOutputFile(filePath);
                    AddMessage($"[Finish] [RIS]  Processing file '{filePath}'");
                    break;
                case Flavour.None:
                    AddMessage($"File neither JSON nor RIS: '{filePath}'.");
                    //throw new Exception(
                    //    $"All files should be detected as either JSON or RIS. This file has been detected as neither: '{filePath}' "
                    //);
                    break;
            }
        }

        private void GetRisAndStoreInOutputFile(string filePath)
        {
            GetUriAndWriteTextAreaContentToFile(filePath, "risOutput");
        }

        private void GetJsonAndStoreInOutputFile(string filePath)
        {
            GetUriAndWriteTextAreaContentToFile(filePath, "output");
        }

        private void GetUriAndWriteTextAreaContentToFile(string inputFilePath, string textAreaId)
        {
            System.Diagnostics.Debugger.Launch();
            var uri = new Uri(inputFilePath).AbsoluteUri;
            _driver.Url = uri;
            _driver.Navigate();
            var output = WaitForElement(textAreaId);

            System.Diagnostics.Debugger.Launch();
            var json = ((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].innerHTML", output).ToString();
            var outputFilePath = MakeOutputFilePath(inputFilePath);
            EnsureFolderPath(outputFilePath);

            File.WriteAllText(outputFilePath, json);
        }

        private IWebElement WaitForElement(string elementId)
        {
            return _driver.WaitForElement(By.Id(elementId), uint.MaxValue, false);
        }

        private static string MakeOutputFilePath(string inputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }
            var folder = Path.Combine(Path.GetDirectoryName(inputFilePath), "OutputFiles", "level2");
            var fileName = Path.GetFileName(inputFilePath);
            var outputFileName = $"{fileName}.json.txt";
            var outputFilePath = Path.Combine(folder, outputFileName);

            return outputFilePath;
        }

        private static void EnsureFolderPath(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(folder);
        }

        private void findFilesInFolderTree_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.JsonIdentifierPattern = JsonIdentifierPattern.Text.Trim();
            Properties.Settings.Default.RisIdentifierPattern = RisIdentifierPattern.Text.Trim();
            Properties.Settings.Default.Save();

            DoFindFiles(recursive: true, usePatterns: true);
            DoAddBootstrap();
        }
    }
}
