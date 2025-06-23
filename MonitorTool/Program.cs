using System;
using System.IO;
using MonitorTool.utils;

namespace MonitorTool
{
    static class Program
    {
        static string PresetsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "presets");
        static string AutoPresetPath = Path.Combine(PresetsDir, "auto.json");

        static void Main(string[] args)
        {
            if (!Directory.Exists(PresetsDir))
                Directory.CreateDirectory(PresetsDir);

            var preset = DisplayManagerHelper.GetCurrentPreset();
            JsonHelper.SaveToFile(AutoPresetPath, preset);
            Console.WriteLine($"Tüm monitör ayarları '{AutoPresetPath}' dosyasına kaydedildi.\n");
            foreach (var m in preset.monitors)
            {
                Console.WriteLine($"{m.deviceName}: {m.width}x{m.height}@{m.hz}Hz ({m.posX},{m.posY})");
            }
        }
    }
}
