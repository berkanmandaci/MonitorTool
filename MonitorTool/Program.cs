using System;
using System.IO;
using MonitorTool.utils;

namespace MonitorTool
{
    static class Program
    {
        static string PresetsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "presets");

        static void CreateShortcut(string presetName)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutLocation = Path.Combine(desktop, $"MonitorPreset_{presetName}.lnk");
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            Type t = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(t);
            dynamic shortcut = shell.CreateShortcut(shortcutLocation);
            shortcut.Description = $"Apply monitor preset: {presetName}";
            shortcut.TargetPath = exePath;
            shortcut.Arguments = $"use {presetName}";
            shortcut.Save();
        }

        static void Main(string[] args)
        {
            if (!Directory.Exists(PresetsDir))
                Directory.CreateDirectory(PresetsDir);

            // Kısayoldan (use presetName ile) çalıştırılırsa preset uygula
            if (args.Length == 2 && args[0].ToLower() == "use")
            {
                string presetName = args[1];
                string presetPath = Path.Combine(PresetsDir, presetName + ".json");
                if (!File.Exists(presetPath))
                {
                    Console.WriteLine($"Preset not found: {presetName}");
                    return;
                }
                var toApply = JsonHelper.LoadFromFile<Preset>(presetPath);
                DisplayManagerHelper.ApplyPreset(toApply);
                Console.WriteLine($"Preset applied: {presetName}");
                return;
            }

            // Doğrudan çalıştırılırsa save olarak çalışsın
            Console.WriteLine("Enter a preset name to save current monitor settings (e.g. gaming, movie, work):");
            string presetNameInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(presetNameInput))
            {
                Console.WriteLine("Invalid preset name. Exiting...");
                return;
            }
            string presetPathSave = Path.Combine(PresetsDir, presetNameInput + ".json");
            var preset = DisplayManagerHelper.GetCurrentPreset();
            JsonHelper.SaveToFile(presetPathSave, preset);
            Console.WriteLine($"Preset saved: {presetNameInput}");
            CreateShortcut(presetNameInput);
            Console.WriteLine($"Shortcut created on desktop: MonitorPreset_{presetNameInput}.lnk");
            Console.WriteLine("You can apply the preset by clicking the shortcut.");
        }
    }
}
