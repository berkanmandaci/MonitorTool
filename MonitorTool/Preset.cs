using System.Collections.Generic;

namespace MonitorTool
{
    public class MonitorSetting
    {
        public string deviceName { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int hz { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public int bitsPerPel { get; set; }
        public int orientation { get; set; } // 0: Landscape, 1: Portrait, 2: Landscape (flipped), 3: Portrait (flipped)
    }

    public class Preset
    {
        public List<MonitorSetting> monitors { get; set; }
    }
} 