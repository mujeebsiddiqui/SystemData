using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;
using System.Threading.Tasks;


namespace Test
{
    internal class Program
    {
        public enum ProgramVersion
        {
            x86,
            x64
        }

        private static IEnumerable<string> GetRegisterSubkeys(RegistryKey registryKey)
        {
            return registryKey.GetSubKeyNames()
                    .Select(registryKey.OpenSubKey)
                    .Select(subkey => subkey.GetValue("DisplayName") as string);
        }

        private static bool CheckNode(RegistryKey registryKey, string applicationName, ProgramVersion? programVersion)
        {
            return GetRegisterSubkeys(registryKey).Any(displayName => displayName != null
                                                                      && displayName.Contains(applicationName)
                                                                      && displayName.Contains(programVersion.ToString()));
        }

        private static bool CheckApplication(string registryKey, string applicationName, ProgramVersion? programVersion)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);

            if (key != null)
            {
                if (CheckNode(key, applicationName, programVersion))
                    return true;

                key.Close();
            }

            return false;
        }

        public static bool IsSoftwareInstalled(string applicationName, ProgramVersion? programVersion)
        {
            string[] registryKey = new[] {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

            return registryKey.Any(key => CheckApplication(key, applicationName, programVersion));
        }

        private static void Main()
        {
            // Examples
            
            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            Console.WriteLine("UserName: {0}", Environment.UserName);
            Console.WriteLine("OsName: {0}", Environment.OSVersion);
            Console.WriteLine("Operating System Information");
            Console.WriteLine("----------------------------");


            var versionID = new ComputerInfo().OSVersion;//6.1.7601.65536
            var versionName = new ComputerInfo().OSFullName;//Microsoft Windows 7 Ultimate
            var verionPlatform = new ComputerInfo().OSPlatform;//WinNT

            Console.WriteLine(versionID);
            Console.WriteLine(versionName);
            Console.WriteLine(verionPlatform);
            Console.WriteLine("----------------------------");
            Console.WriteLine("Notepad++: " + IsSoftwareInstalled("Notepad++", null));
            Console.WriteLine("Notepad++(x86): " + IsSoftwareInstalled("Notepad++", ProgramVersion.x86));
            Console.WriteLine("Notepad++(x64): " + IsSoftwareInstalled("Notepad++", ProgramVersion.x64));
            Console.WriteLine("Microsoft Visual C++ 2015: " + IsSoftwareInstalled("Microsoft Visual C++ 2015", null));
            Console.WriteLine("Microsoft Visual C-- 2015: " + IsSoftwareInstalled("Microsoft Visual C-- 2015", null));
            Console.WriteLine("Microsoft Visual C++ 2015: " + IsSoftwareInstalled("Microsoft Visual C++ 2015", null));
            Console.WriteLine("Microsoft Visual C++ 2015 Redistributable (x86): " + IsSoftwareInstalled("Microsoft Visual C++ 2015", ProgramVersion.x86));
            Console.WriteLine("Microsoft Visual C++ 2015 Redistributable (x64): " + IsSoftwareInstalled("Microsoft Visual C++ 2015", ProgramVersion.x64));
            Console.ReadKey();

        }


    }
}
