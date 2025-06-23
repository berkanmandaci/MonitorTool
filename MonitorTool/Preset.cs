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
    }

    public class Preset
    {
        public List<MonitorSetting> monitors { get; set; }
    }
} 