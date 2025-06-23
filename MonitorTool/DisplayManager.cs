using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MonitorTool
{
    public class DisplayManager
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public ushort dmSpecVersion;
            public ushort dmDriverVersion;
            public ushort dmSize;
            public ushort dmDriverExtra;
            public uint dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public uint dmDisplayOrientation;
            public uint dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public ushort dmLogPixels;
            public uint dmBitsPerPel;
            public uint dmPelsWidth;
            public uint dmPelsHeight;
            public uint dmDisplayFlags;
            public uint dmDisplayFrequency;
            public uint dmICMMethod;
            public uint dmICMIntent;
            public uint dmMediaType;
            public uint dmDitherType;
            public uint dmReserved1;
            public uint dmReserved2;
            public uint dmPanningWidth;
            public uint dmPanningHeight;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, int dwflags, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd, int dwflags, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        // EnumDisplayDevices ve diğer yardımcı fonksiyonlar buraya eklenecek
    }

    public static class DisplayManagerHelper
    {
        public static Preset GetCurrentPreset()
        {
            var monitors = new List<MonitorSetting>();
            int devNum = 0;
            while (true)
            {
                var d = new DisplayManager.DISPLAY_DEVICE
                {
                    cb = Marshal.SizeOf(typeof(DisplayManager.DISPLAY_DEVICE)),
                    DeviceName = string.Empty,
                    DeviceString = string.Empty,
                    DeviceID = string.Empty,
                    DeviceKey = string.Empty
                };
                if (!DisplayManager.EnumDisplayDevices(null, (uint)devNum, ref d, 0))
                    break;
                if ((d.StateFlags & 0x1) != 0) // DISPLAY_DEVICE_ACTIVE
                {
                    var mode = new DisplayManager.DEVMODE
                    {
                        dmDeviceName = string.Empty,
                        dmFormName = string.Empty,
                        dmSize = (ushort)Marshal.SizeOf(typeof(DisplayManager.DEVMODE))
                    };
                    if (DisplayManager.EnumDisplaySettings(d.DeviceName, -1, ref mode))
                    {
                        monitors.Add(new MonitorSetting
                        {
                            deviceName = d.DeviceName,
                            width = (int)mode.dmPelsWidth,
                            height = (int)mode.dmPelsHeight,
                            hz = (int)mode.dmDisplayFrequency,
                            posX = mode.dmPositionX,
                            posY = mode.dmPositionY,
                            bitsPerPel = (int)mode.dmBitsPerPel,
                            orientation = (int)mode.dmDisplayOrientation
                        });
                    }
                }
                devNum++;
            }
            return new Preset { monitors = monitors };
        }

        public static void ApplyPreset(Preset preset)
        {
            const int DM_PELSWIDTH = 0x80000;
            const int DM_PELSHEIGHT = 0x100000;
            const int DM_DISPLAYFREQUENCY = 0x400000;
            const int DM_POSITION = 0x20;
            const int DM_BITSPERPEL = 0x40000;
            const int DM_DISPLAYORIENTATION = 0x80;
            const int CDS_UPDATEREGISTRY = 0x1;
            foreach (var m in preset.monitors)
            {
                var mode = new DisplayManager.DEVMODE
                {
                    dmDeviceName = m.deviceName,
                    dmSize = (ushort)Marshal.SizeOf(typeof(DisplayManager.DEVMODE)),
                    dmPelsWidth = (uint)m.width,
                    dmPelsHeight = (uint)m.height,
                    dmDisplayFrequency = (uint)m.hz,
                    dmPositionX = m.posX,
                    dmPositionY = m.posY,
                    dmBitsPerPel = (uint)m.bitsPerPel,
                    dmDisplayOrientation = (uint)m.orientation,
                    dmFields = (uint)(DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_POSITION | DM_BITSPERPEL | DM_DISPLAYORIENTATION)
                };
                int result = DisplayManager.ChangeDisplaySettingsEx(m.deviceName, ref mode, IntPtr.Zero, CDS_UPDATEREGISTRY, IntPtr.Zero);
                if (result == 0)
                    Console.WriteLine($"Applied: {m.deviceName} {m.width}x{m.height}@{m.hz}Hz {m.bitsPerPel}bpp ({m.posX},{m.posY}) orientation:{m.orientation}");
                else
                    Console.WriteLine($"Failed to apply: {m.deviceName} (Error code: {result})");
            }
        }
    }
} 