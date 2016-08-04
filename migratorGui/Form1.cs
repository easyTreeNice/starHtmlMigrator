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
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace migratorGui
{
    public partial class Form1 : Form
    {
        int one;
        int two;
        int three;
        int four;

        private string FileNameNow { get; set; }

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
            fileList.Items.Clear();
            processedFiles.Items.Clear();
            messages.Text = string.Empty;

            FileNameNow = GetNow();
            AddMessage($"Starting session at {FileNameNow}");
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

        private string GetBootstrapCode(string filePath, bool newMethod)
        {
            var exeFolder = Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty;
            var assetsFolderPath = GetAncestor(exeFolder, Properties.Settings.Default.AssetFolderAncestorGenerations);
            var assetsFolderUri = new Uri(assetsFolderPath).AbsoluteUri;
            var flavour = GetFileFlavour(filePath);
            var suffix = newMethod 
                            ? (flavour == Flavour.Json ? "JSON" : flavour == Flavour.Ris ? "RIS" : null)
                            : (ToJSON.Checked ? "JSON" : ToRIS.Checked ? "RIS" : null);

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

        private static Flavour GetFileFlavour(string filePath)
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
                )
            .Where(f => !f.Contains(_processedFolderName))
            .ToList();

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

            InputFilePaths.ForEach(f =>
            {
                var flavour = GetFileFlavour(f).ToString();
                var suffix = f.Replace(prefix, string.Empty);
                fileList.Items.Add($"[{flavour}] {suffix}");
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

        //private IWebDriver _driver;
        private string _processedFolderName = "ProcessedFiles";
        private DriverService _service;

        private void processFiles_Click(object sender, EventArgs e)
        {
        }

        private void DoAddBootstrap(bool newMethod)
        {
            Properties.Settings.Default.ToRIS = ToRIS.Checked;
            Properties.Settings.Default.Save();

            var selectedFiles = fileList.SelectedIndices.Cast<int>()
                .Select(i => InputFilePaths[i])
                .ToList();

            selectedFiles = InputFilePaths;

            var prefix = $"{FolderPath}\\";
            processedFiles.Items.Clear();
            OutputFilePaths = new List<string>();
            selectedFiles.ForEach(f =>
            {
                var fileFolderPath = Path.GetDirectoryName(f);
                var outputFolderPath = Path.Combine(
                    fileFolderPath,
                    _processedFolderName,
                    FileNameNow
                );
                Directory.CreateDirectory(outputFolderPath);
                var outputFilePath = Path.Combine(
                    outputFolderPath,
                    Path.GetFileName(f)
                    );
                var input = File.ReadAllText(f);
                var output =
                    reRemoveCssAndScript.Replace(input, string.Empty)
                        .Replace("<head>", "<head>" + GetBootstrapCode(f, newMethod));

                File.WriteAllText(outputFilePath, output);

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
                //findFilesButton,
                //processFilesButton,
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

        private string GetLogFilePath()
        {
            //var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var logFolderPath = Path.Combine(FolderPath, "Logs");
            Directory.CreateDirectory(logFolderPath);
            var logFilePath = Path.Combine(logFolderPath, "LogFile.txt");
            return logFilePath;
        }

        private void runAutomatedButton_Click(object sender, EventArgs e)
        {
            _service = ChromeDriverService.CreateDefaultService();
            //_service.LogPath = GetLogFilePath();

            //AddMessage($"LogFilePath set to {_service.LogPath}");
            //_service.EnableVerboseLogging = true;
            IWebDriver driver = null;

            try
            {
                //_service.Start();
                driver = CreateDriver();

                OutputFilePaths.ForEach(f => AutoProcessFile(driver, f));
            }                  
            catch (Exception ex)
            {
                AddMessage($"Exception: {ex.Message}");
            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
                //_service.Dispose();
            }
        }

        private void DoAutoProcessFile(IWebDriver driver, string filePath)
        {
            switch (GetFileFlavour(filePath))
            {
                case Flavour.Json:
                    AddMessage($"[Start]  [JSON] Processing file '{filePath}'");
                    GetJsonAndStoreInOutputFile(driver, filePath);
                    AddMessage($"[Finish] [JSON] Processing file '{filePath}'");
                    break;
                case Flavour.Ris:
                    AddMessage($"[Start]  [RIS]  Processing file '{filePath}'");
                    GetRisAndStoreInOutputFile(driver, filePath);
                    AddMessage($"[Finish] [RIS]  Processing file '{filePath}'");
                    break;
                case Flavour.None:
                    AddMessage($"File neither JSON nor RIS: '{filePath}'.");
                    break;
            }
        }

        private IWebDriver CreateDriver()
        {
            var driverOptions = new ChromeOptions();
            driverOptions.SetLoggingPreference(LogType.Driver, 0);
            driverOptions.SetLoggingPreference(LogType.Browser, 0);
            //driverOptions.SetLoggingPreference(LogType.Client, 0);
            //driverOptions.SetLoggingPreference(LogType.Profiler, 0);
            //driverOptions.SetLoggingPreference(LogType.Server, 0);
            driverOptions.AddArgument("--log-level=0");
            driverOptions.AddArgument("--verbose");
            var logPath = GetLogFilePath();
            driverOptions.AddArgument($"--log-path={logPath}");
            driverOptions.AddArgument("--disable-extensions");
            AddMessage($"LogFilePath set to {logPath}");
            var driver = new ChromeDriver(_service as ChromeDriverService, driverOptions, Timeout.InfiniteTimeSpan);

            //var driver = new RemoteWebDriver(new Uri("http://localhost:9515"), DesiredCapabilities.Chrome());    
            //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.MaxValue);
            //driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.MaxValue);
            //            driver.Manage().Logs.GetLog(LogType.Browser);

            //return driver;
            //return null;
            return driver;
        }

        private void AutoProcessFile(IWebDriver driver, string filePath)
        {
            try
            {
                DoAutoProcessFile (driver, filePath);
            }
            catch (Exception ex)
            {
                AddMessage($"Exception: {ex.Message} : {ex.StackTrace}");
            }
        }

        private void GetRisAndStoreInOutputFile(IWebDriver driver, string filePath)
        {
            GetUriAndWriteTextAreaContentToFile(driver, filePath, "risOutput");
        }

        private void GetJsonAndStoreInOutputFile(IWebDriver driver, string filePath)
        {
            GetUriAndWriteTextAreaContentToFile(driver, filePath, "output");
        }

        private void GetUriAndWriteTextAreaContentToFile(IWebDriver driver, string inputFilePath, string textAreaId)
        {
            //System.Diagnostics.Debugger.Launch();
            var uri = new Uri(inputFilePath).AbsoluteUri;
            driver.Navigate().GoToUrl(uri);
            var output = WaitForElement(driver, textAreaId);

            //System.Diagnostics.Debugger.Launch();
            var json = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerHTML", output).ToString();
            var outputFilePath = MakeOutputFilePath(inputFilePath);
            EnsureFolderPath(outputFilePath);

            File.WriteAllText(outputFilePath, json);
        }

        private IWebElement WaitForElement(IWebDriver driver, string elementId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromDays(1));
            return wait.Until(d => d.FindElement(By.Id(elementId)));
        }

        private static string MakeOutputFilePath(string inputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }
            var folder = Path.Combine(Path.GetDirectoryName(inputFilePath), "OutputFiles");
            var fileName = Path.GetFileName(inputFilePath);
            var flavour = GetFileFlavour(inputFilePath);
            var flavourSuffix = flavour == Flavour.Json
                ? "json"
                : flavour == Flavour.Ris
                    ? "ris"
                    : "error";

            var outputFileName = $"{fileName}.{flavourSuffix}.txt";
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
            DoAddBootstrap(newMethod: true);
        }

        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void testButton_Click(object sender, EventArgs e)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                //Notice navigation is slightly different than the Java version
                //This is because 'get' is a keyword in C#
                driver.Navigate().GoToUrl("http://www.google.com/");

                // Find the text input element by its name
                IWebElement query = driver.FindElement(By.Name("q"));

                // Enter something to search for
                query.SendKeys("Cheese");

                // Now submit the form. WebDriver will find the form for us from the element
                query.Submit();

                // Google's search is rendered dynamically with JavaScript.
                // Wait for the page to load, timeout after 10 seconds
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

                // Should see: "Cheese - Google Search" (for an English locale)
                Console.WriteLine("Page title is: " + driver.Title);
            }
        }
    }
}
