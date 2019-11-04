namespace EosTools.v1.FontGeneratorApp.Infrastructure {

    using System;
    using System.Runtime.InteropServices;

    public static class FontAPI {

        public enum GGOFormat: uint {
            GGO_METRICS = 0,
            GGO_BITMAP = 1,
            GGO_GRAY2_BITMAP = 4,
            GGO_GRAY4_BITMAP = 5,
            GGO_GRAY8_BITMAP = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TEXTMETRICS {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public ushort tmFirstChar;
            public ushort tmLastChar;
            public ushort tmDefaultChar;
            public ushort tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GLYPHMETRICS {
            public uint gmBlackBoxX;
            public uint gmBlackBoxY;
            public POINT gmptGlyphOrigin;
            public short gmCellIncX;
            public short gmCellIncY;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FIXED {
            public short fract;
            public short value;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MAT2 {
            [MarshalAs(UnmanagedType.Struct)]
            public FIXED eM11;
            [MarshalAs(UnmanagedType.Struct)]
            public FIXED eM12;
            [MarshalAs(UnmanagedType.Struct)]
            public FIXED eM21;
            [MarshalAs(UnmanagedType.Struct)]
            public FIXED eM22;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetGlyphOutline(
            IntPtr hdc, 
            uint ch,
            GGOFormat fmt, 
            out GLYPHMETRICS gm, 
            uint bufferSize, 
            IntPtr buffer, 
            ref MAT2 matrix);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetTextMetrics(
            IntPtr hdc, 
            out TEXTMETRICS tm);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SelectObject(
            IntPtr hdc, 
            IntPtr obj);
    }
}
