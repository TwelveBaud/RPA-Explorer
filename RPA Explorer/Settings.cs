using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RPA_Explorer
{
    public static class Settings
    {
        private readonly static Dictionary<string, Dictionary<string, Dictionary<string, string>>> _settings = new();
        private readonly static string settingsPath;

        static Settings()
        {
            var appName = System.Reflection.Assembly.GetExecutingAssembly()?.GetName().Name;
            var settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
            Directory.CreateDirectory(settingsDir);
            settingsPath = Path.Combine(settingsDir, "settings.xml");
            if (File.Exists(settingsPath))
            {
                try
                {
                    ReadSettings();
                }
                catch { }
            }
            else
            {
                if (File.Exists(Path.Combine(settingsDir, "settings.ini")))
                {
                    foreach (var line in File.ReadAllLines(Path.Combine(settingsDir, "settings.ini")))
                    {
                        var setting = line.Split('=');
                        if (setting.Length < 2) continue;
                        var name = setting[0];
                        var value = string.Join("=", setting.Skip(1));
                        switch (name)
                        {
                            case "language":
                                if (value == "English") value = "en-US";
                                SetSetting("Language", value, typeof(Program));
                                break;
                            case "python":
                                SetSetting("PythonPath", value, "Code.previewer.dll", "RpycPreviewer");
                                break;
                            case "unpyrc":
                                SetSetting("ScriptPath", value, "Code.previewer.dll", "RpycPreviewer");
                                break;
                            case "archive":
                                SetSetting("LastOpenedFile", value, typeof(MainWindow));
                                break;
                        }
                    }
                }
            }
        }

        public static string GetSetting(string key, Type onBehalfOf = null)
        {
            onBehalfOf ??= GetCaller();
            if (_settings.TryGetValue(onBehalfOf.Assembly.GetName().Name.Replace(" ", ""), out var assem) && assem.TryGetValue(onBehalfOf.FullName, out var type) && type.TryGetValue(key, out var value))
                return value;
            return null;
        }

        public static void SetSetting(string key, string value, Type onBehalfOf = null)
        {
            onBehalfOf ??= GetCaller();
            var assem = onBehalfOf.Assembly.GetName().Name.Replace(" ", "");
            var type = onBehalfOf.FullName;
            SetSetting(key, value, assem, type);
        }

        private static void SetSetting(string key, string value, string assem, string type)
        {
            if (!_settings.ContainsKey(assem)) _settings[assem] = new();
            if (!_settings[assem].ContainsKey(type)) _settings[assem][type] = new();
            _settings[assem][type][key] = value;
            WriteSettings();
        }

        private static void ReadSettings()
        {
            var doc = XDocument.Load(settingsPath);
            var root = doc.Root;
            foreach (var assemXml in root.Elements())
            {
                _settings[assemXml.Name.LocalName] = new();
                foreach (var typeXml in assemXml.Elements())
                {
                    _settings[assemXml.Name.LocalName][typeXml.Name.LocalName] = new();
                    foreach (var settingXml in typeXml.Elements())
                    {
                        _settings[assemXml.Name.LocalName][typeXml.Name.LocalName][settingXml.Name.LocalName] = settingXml.Value;
                    }
                }
            }
        }

        private static void WriteSettings()
        {
            var doc = new XDocument();
            var root = new XElement("Settings");
            foreach (var assem in _settings)
            {
                var assemXml = new XElement(assem.Key);
                foreach (var type in assem.Value)
                {
                    var typeXml = new XElement(type.Key);
                    foreach (var key in type.Value)
                    {
                        var keyXml = new XElement(key.Key);
                        keyXml.Add(key.Value);
                        typeXml.Add(keyXml);
                    }
                    assemXml.Add(typeXml);
                }
                root.Add(assemXml);
            }
            doc.Add(root);
            doc.Save(settingsPath);
        }

        private static Type GetCaller()
        {
            var trace = new System.Diagnostics.StackTrace();
            return trace.GetFrame(2).GetMethod().DeclaringType;
        }
    }
}
