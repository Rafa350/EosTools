namespace Media.PicFontGenerator.Infrastructure {

    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class FontInfo {

        public int Ascent;
        public int Descent;
        public int Height;
    }

    public sealed class GlyphInfo {

        public Bitmap Bitmap;
        public int Left;
        public int Top;
        public int Width { get { return Bitmap == null ? 0 : Bitmap.Width; } }
        public int Height { get { return Bitmap == null ? 0 : Bitmap.Height; } }
        public int Advance;
    }
    
    public static class FontAPI {

        private enum GGOFormat: uint {
            GGO_METRICS = 0,
            GGO_BITMAP = 1,
            GGO_GRAY2_BITMAP = 4,
            GGO_GRAY4_BITMAP = 5,
            GGO_GRAY8_BITMAP = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TEXTMETRICS {
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
        private struct POINT {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GLYPHMETRICS {
            public uint gmBlackBoxX;
            public uint gmBlackBoxY;
            public POINT gmptGlyphOrigin;
            public short gmCellIncX;
            public short gmCellIncY;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FIXED {
            public short fract;
            public short value;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct MAT2 {
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
        private static extern uint GetGlyphOutline(
            IntPtr hdc, 
            uint ch,
            uint format, 
            out GLYPHMETRICS gm, 
            uint bufferSize, 
            IntPtr buffer, 
            ref MAT2 matrix);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetTextMetrics(
            IntPtr hdc, 
            out TEXTMETRICS tm);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SelectObject(
            IntPtr hdc, 
            IntPtr obj);

        public static FontInfo GetFontInfo(Font font) {

            using (Bitmap bitmap = new Bitmap(1, 1)) {
                using (Graphics graphics = Graphics.FromImage(bitmap)) {

                    IntPtr hdc = graphics.GetHdc();
                    try {
                        IntPtr hOldFont = SelectObject(hdc, font.ToHfont());
                        try {
                            TEXTMETRICS tm;
                            if (!GetTextMetrics(hdc, out tm)) {
                                int error = Marshal.GetLastWin32Error();
                                throw new InvalidOperationException(
                                    String.Format("GetTextMetrics: ERROR '{0}", error));
                            }
                            return CreateFontInfo(tm);
                        }
                        finally {
                            SelectObject(hdc, hOldFont);
                        }
                    }
                    finally {
                        graphics.ReleaseHdc(hdc);
                    }
                }
            }
        }

        public static GlyphInfo GetGlyphInfo(Font font, char ch) {

            using (Bitmap bitmap = new Bitmap(1, 1)) {
                using (Graphics graphics = Graphics.FromImage(bitmap)) {

                    IntPtr hdc = graphics.GetHdc();
                    try {
                        IntPtr hOldFont = SelectObject(hdc, font.ToHfont());
                        try {

                            GLYPHMETRICS gm;
                            MAT2 matrix = new MAT2();
                            matrix.eM11.value = 1;
                            matrix.eM12.value = 0;
                            matrix.eM21.value = 0;
                            matrix.eM22.value = 1;

                            GGOFormat format = GGOFormat.GGO_BITMAP;

                            byte[] pixels = null;
                            int bufferSize = (int) GetGlyphOutline(hdc, ch, (uint) format,
                                out gm, 0, IntPtr.Zero, ref matrix);
                            if (bufferSize > 0) {
                                IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
                                try {
                                    if (GetGlyphOutline(hdc, ch, (uint) format, out gm, (uint) bufferSize, buffer, ref matrix) == 0) {
                                        int error = Marshal.GetLastWin32Error();
                                        throw new InvalidOperationException(
                                            String.Format("GetGlyphOutline: ERROR '{0}", error));
                                    }

                                    pixels = new byte[bufferSize];
                                    Marshal.Copy(buffer, pixels, 0, bufferSize);
                                }
                                finally {
                                    Marshal.FreeHGlobal(buffer);
                                }
                            }

                            return CreateGlyphInfo(gm, pixels, format);

                        }
                        finally {
                            SelectObject(hdc, hOldFont);
                        }
                    }
                    finally {
                        graphics.ReleaseHdc(hdc);
                    }
                }
            }
        }

        private static FontInfo CreateFontInfo(TEXTMETRICS tm) {

            FontInfo fi = new FontInfo();
            fi.Ascent = tm.tmAscent;
            fi.Descent = tm.tmDescent;
            fi.Height = tm.tmHeight;

            return fi;
        }

        private static GlyphInfo CreateGlyphInfo(GLYPHMETRICS gm, byte[] pixels, GGOFormat format) {

            GlyphInfo gi = new GlyphInfo();
            gi.Advance = gm.gmCellIncX;
            gi.Left = gm.gmptGlyphOrigin.x;
            gi.Top = gm.gmptGlyphOrigin.y;

            if (pixels != null) {

                // Calcula el tamany real del bitmap
                //
                int maxX = Int32.MinValue;
                int maxY = Int32.MinValue;
                for (int y = 0; y < (int) gm.gmBlackBoxY; y++) {
                    for (int x = 0; x < (int) gm.gmBlackBoxX; x++) {
                        if (GetPixel(pixels, format, x, y)) {
                            if (x > maxX)
                                maxX = x;
                            if (y > maxY)
                                maxY = y;
                        }
                    }
                }
                int minX = Int32.MaxValue;
                int minY = Int32.MaxValue;
                for (int y = (int) gm.gmBlackBoxY - 1; y >= 0; y--) {
                    for (int x = (int) gm.gmBlackBoxX - 1; x >= 0; x--) {
                        if (GetPixel(pixels, format, x, y)) {
                            if (x < minX)
                                minX = x;
                            if (y < minY)
                                minY = y;
                        }
                    }
                }

                // Corrigeix la posicio del bitmap
                //
                gi.Left += minX;
                gi.Top += minY;

                // Crea el bitmap
                //
                Bitmap bitmap = new Bitmap(maxX - minX + 1, maxY - minY + 1);
                Color black = Color.FromKnownColor(KnownColor.Black);
                Color transparent = Color.FromKnownColor(KnownColor.Transparent);
                for (int y = minY; y <= maxY; y++) 
                    for (int x = minX; x <= maxX; x++) 
                        bitmap.SetPixel(x - minX, y - minY, GetPixel(pixels, format, x, y) ? black : transparent);
                gi.Bitmap = bitmap;
            }

            return gi;
        }

        private static bool GetPixel(byte[] pixels, GGOFormat format, int x, int y) {

            switch (format) {
                default:
                case GGOFormat.GGO_BITMAP:
                    return (pixels[(y * 4) + (x >> 3)] & (0x80 >> (x & 0x07))) != 0;

                case GGOFormat.GGO_GRAY2_BITMAP:
                    return (pixels[(y * 4) + (x >> 3)] & (0x80 >> (x & 0x07))) != 0;

                case GGOFormat.GGO_GRAY4_BITMAP:
                    return (pixels[(y * 4) + (x >> 3)] & (0x80 >> (x & 0x07))) != 0;

                case GGOFormat.GGO_GRAY8_BITMAP:
                    return (pixels[(y * 4) + (x >> 3)] & (0x80 >> (x & 0x07))) != 0;
            }
        }
    }
}
