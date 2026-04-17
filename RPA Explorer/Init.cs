using RPA_Explorer.Previewers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace RPA_Explorer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitializeLanguages();
            Lang.Culture = new CultureInfo(Settings.GetSetting("Language") ?? "en-US");

            Previewers = new List<IPreviewer>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(IPreviewer).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
                    Previewers.Add((IPreviewer)Activator.CreateInstance(type));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        static void InitializeLanguages()
        {
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var appDir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

            // Each language is stored in a `.resources.dll` file in a directory named after the language ID.
            var availableCultures = appDir.GetDirectories()
                .Select(dir => new { Directory = dir, Culture = allCultures.FirstOrDefault(culture => dir.Name == culture.Name) })
                .Where(entry => entry.Culture != null)
                .Where(entry => File.Exists(entry.Directory.FullName + "/" + Assembly.GetExecutingAssembly().GetName().Name + ".resources.dll"))
                .Select(entry => entry.Culture)
                .OrderBy(culture => culture.NativeName)
                .ToList();

            // Since it's compiled into the app, `en-US` is always available; add it to the top if it isn't there already.
            var neutralCulture = CultureInfo.GetCultureInfo("en-US");
            if (!availableCultures.Contains(neutralCulture)) availableCultures.Insert(0, neutralCulture);

            // Show the user's Windows language at the very top if it's one we support.
            var systemCulture = Thread.CurrentThread.CurrentUICulture;
            if (availableCultures.Contains(systemCulture))
            {
                availableCultures.Remove(systemCulture);
                availableCultures.Insert(0, systemCulture);
            }

            Languages = availableCultures;
        }

        public static IList<CultureInfo> Languages;
        public static IList<IPreviewer> Previewers;
        public static StatusBarBroker StatusBar;
    }
}