namespace EosTools.v1.FontGeneratorApp.Infrastructure {

    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class GlyphData {

        private readonly GlyphBitmap glyph;
        private readonly int advance;

        /// <summary>
        /// Constructor del objecte.
        /// </summary>
        /// <param name="font">El font.</param>
        /// <param name="ch">El caracter.</param>
        /// <param name="format">Format del bitmap.</param>
        /// 
        public GlyphData(Font font, char ch, GlyphFormat format) {

            using (Bitmap bmp = new Bitmap(1, 1)) {
                using (Graphics graphics = Graphics.FromImage(bmp)) {

                    IntPtr hdc = graphics.GetHdc();
                    try {
                        IntPtr hOldFont = FontAPI.SelectObject(hdc, font.ToHfont());
                        try {

                            FontAPI.GLYPHMETRICS gm;

                            FontAPI.MAT2 matrix = new FontAPI.MAT2();
                            matrix.eM11.value = 1;
                            matrix.eM12.value = 0;
                            matrix.eM21.value = 0;
                            matrix.eM22.value = 1;

                            FontAPI.GGOFormat fmt;
                            switch (format) {
                                default:
                                case GlyphFormat.L1:
                                    fmt = FontAPI.GGOFormat.GGO_BITMAP;
                                    break;

                                case GlyphFormat.L2:
                                    fmt = FontAPI.GGOFormat.GGO_GRAY2_BITMAP;
                                    break;

                                case GlyphFormat.L4:
                                    fmt = FontAPI.GGOFormat.GGO_GRAY4_BITMAP;
                                    break;

                                case GlyphFormat.L8:
                                    fmt = FontAPI.GGOFormat.GGO_GRAY8_BITMAP;
                                    break;
                            }

                            int bufferSize = (int)FontAPI.GetGlyphOutline(hdc, ch, fmt, out gm, 0, IntPtr.Zero, ref matrix);
                            if (bufferSize > 0) {
                                byte[] pixels = null;
                                IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
                                try {
                                    if (FontAPI.GetGlyphOutline(hdc, ch, fmt, out gm, (uint)bufferSize, buffer, ref matrix) == 0) {
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

                                glyph = new GlyphBitmap(gm, pixels, fmt);
                                advance = gm.gmCellIncX;
                            }
                        }
                        finally {
                            FontAPI.SelectObject(hdc, hOldFont);
                        }
                    }
                    finally {
                        graphics.ReleaseHdc(hdc);
                    }
                }
            }
        }

        public int Advance {
            get { return advance; }
        }

        public GlyphBitmap Glyph {
            get { return glyph; }
        }
    }
}
