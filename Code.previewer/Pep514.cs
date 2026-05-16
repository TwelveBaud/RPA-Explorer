using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Code.previewer
{
    public class PythonInstance
    {
        public Version Version { get; set; }
        public string Tag { get; set; }
        public string Arch { get; set; }
        public string Company { get; set; }
        public string DisplayName { get; set; }
        public string Path { get; set; }

        private static IEnumerable<PythonInstance> GetPep514Instances()
        {
            using var HKCU = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Python");
            using var HKLMEmuHive = Environment.Is64BitOperatingSystem
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitProcess
                    ? RegistryView.Registry32 : RegistryView.Registry64)
                : null;
            using var HKLMEmu = HKLMEmuHive?.OpenSubKey("SOFTWARE\\Python");
            using var HKLMNative = Registry.LocalMachine.OpenSubKey("Software\\Python");

            static IEnumerable<PythonInstance> ProcessHive(RegistryKey hive, string arch)
            {
                if (hive == null) yield break;
                foreach (var company in hive.GetSubKeyNames())
                {
                    var companyKey = hive.OpenSubKey(company);
                    var companyName = (string)companyKey.GetValue(null) ?? company;
                    foreach (var tag in companyKey.GetSubKeyNames())
                    {
                        var instanceKey = companyKey.OpenSubKey(tag);
                        var stringVersion = instanceKey.GetValue("SysVersion") as string ?? instanceKey.GetValue("Version") as string ?? tag;
                        var instance = new PythonInstance
                        {
                            Tag = tag,
                            Company = companyName,
                            DisplayName = instanceKey.GetValue("DisplayName") as string ?? "Python " + tag,
                            Arch = instanceKey.GetValue("SysArchitecture") as string ?? arch ?? (Environment.Is64BitOperatingSystem ? "64bit" : "32bit"),
                        };
                        if (Version.TryParse(stringVersion, out var version)) instance.Version = version;
                        var installPathKey = instanceKey.OpenSubKey("InstallPath");
                        if (installPathKey != null)
                        {
                            var basePath = (installPathKey.GetValue(null) as string);
                            if (basePath != null) basePath += "\\python.exe";
                            instance.Path = installPathKey.GetValue("ExecutablePath") as string ?? basePath;
                        }
                        yield return instance;
                        instanceKey.Close();
                        installPathKey?.Close();
                    }
                    companyKey.Close();
                }
            }

            var cu = ProcessHive(HKCU, null);
            var lme = ProcessHive(HKLMEmu, Environment.Is64BitProcess ? "32bit" : "64bit");
            var lmn = ProcessHive(HKLMNative, (Environment.Is64BitProcess && Environment.Is64BitOperatingSystem) ? "64bit" : "32bit");
            return cu.Union(lme).Union(lmn).ToList();
        }

        // https://github.com/python/pymanager/blob/52242bf3af2363e2da899ca5230b30124bf42240/src/manage/pep514utils.py#L383
        private static IEnumerable<PythonInstance> GetStoreInstances()
        {
            var PFNs = new string[] { "_qbz5n2kfra8p0", "_3847v3x7pw1km", "_hd69rhyc2wevp" };
            var PerUserApps = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Microsoft", "WindowsApps"));
            if (!PerUserApps.Exists) yield break;
            var Pythons = PerUserApps.GetDirectories("PythonSoftwareFoundation.Python.3.*").Where(dir => PFNs.Any(dir.Name.ToLowerInvariant().EndsWith));
            // Note: A system-wide Store Python might exist, _and_ it might even be one we can/should use, but unless it has a PEP514 registration we'd need Administrator privs to know about it.
            foreach (var instance in Pythons)
            {
                var tag = "3." + instance.Name.Substring(instance.Name.LastIndexOf(".") + 1);
                tag = tag.Substring(tag.IndexOf("_"));
                yield return new PythonInstance
                {
                    DisplayName = $"Python {tag} (Store)",
                    Company = "Python Software Foundation",
                    Arch = "64bit",
                    Version = Version.Parse(tag),
                    Tag = tag,
                    Path = System.IO.Path.Combine(instance.FullName, "python.exe")
                };
            }
        }

        public static IEnumerable<PythonInstance> GetLocalInstances()
        {
            return GetPep514Instances().Union(GetStoreInstances())
                .Where(instance => File.Exists(instance.Path))
                .GroupBy(instance => instance.Path)
                .Select(group => group.First())
                .OrderByDescending(instance => instance.Version)
                .ThenByDescending(instance => instance.Arch);
        }
    }
}
