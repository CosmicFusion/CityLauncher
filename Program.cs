using NLog;
using NLog.Config;
using NLog.Targets;
using System.Globalization;

namespace CityLauncher
{
    internal static class Program
    {

        private static readonly Logger Nlog = LogManager.GetCurrentClassLogger();

        public static readonly string CurrentTime = DateTime.Now.ToString("dd-MM-yy__hh-mm-ss");

        public static CityLauncher MainWindow;

        public static IniHandler IniHandler;

        public static FileHandler FileHandler;

        public static InputHandler InputHandler;

        /// <summary>
        ///     Replacement Application for the original Batman: Arkham City BmLauncher
        ///     Offers more configuration options, enables compatibility with High-Res Texture Packs
        ///     and automatically takes care of the ReadOnly properties of each file, removing
        ///     any requirement to manually edit .ini files. Guarantees a much more comfortable user experience.
        ///     @author Neato (https://steamcommunity.com/id/frofoo)
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupLogger();
            InitializeProgram();
            Application.Run(MainWindow);
        }

        private static void InitializeProgram()
        {
            Nlog.Info("InitializeProgram - Starting logs at {0} on {1}.", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.ToString("D", new CultureInfo("en-GB")));
            ApplicationConfiguration.Initialize();
            MainWindow = new CityLauncher();
            FileHandler = new FileHandler();
            IniHandler = new IniHandler();
            InputHandler = new InputHandler();
            new IniReader().InitDisplay();
            new InputReader().InitControls();

        }

        private static void SetupLogger()
        {
            LoggingConfiguration config = new();
            ConsoleTarget logconsole = new("logconsole");
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            FileTarget logfile = new FileTarget("logfile")
            {
                FileName = Directory.GetCurrentDirectory() + "\\logs\\citylauncher_report__" + CurrentTime + ".log"
            };
            DirectoryInfo LogDirectory = new(Directory.GetCurrentDirectory() + "\\logs");
            DateTime OldestAllowedArchive = DateTime.Now - new TimeSpan(3, 0, 0, 0);
            foreach (FileInfo file in LogDirectory.GetFiles())
            {
                if (file.CreationTime < OldestAllowedArchive)
                {
                    file.Delete();
                }
            }

            config.AddRule(LogLevel.Debug, LogLevel.Warn, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Warn, logfile);
            LogManager.Configuration = config;
        }
    }
}