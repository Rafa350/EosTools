namespace EosTools.v1.FontGeneratorApp.Infrastructure {

    using System;
    using System.Drawing;

    public enum GlyphFormat {
        Unknown,
        L1,
        L2,
        L4,
        L8
    }

    /// <summary>
    /// Clase que representa el bitmap del caracter.
    /// </summary>
    public sealed class GlyphBitmap {

        private readonly Bitmap bitmap;
        private readonly GlyphFormat format = GlyphFormat.Unknown;
        private readonly int offsetX;
        private readonly int offsetY;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="gm">Metriques del caracter.</param>
        /// <param name="pixels">Els pixels del bitmap.</param>
        /// <param name="fmt">El format de pixels.</param>
        /// 
        public GlyphBitmap(FontAPI.GLYPHMETRICS gm, byte[] pixels, FontAPI.GGOFormat fmt) {

            bool GetPixel(int x, int y) {

                switch (fmt) {
                    default:
                    case FontAPI.GGOFormat.GGO_BITMAP: {
                        int width = (int)(((gm.gmBlackBoxX + 31) >> 3) & ~3);
                        return (pixels[(y * width) + (x >> 3)] & (0x80 >> (x & 0x07))) != 0;
                    }
                }
            }

            // Calcula el tamany real del bitmap
            //
            int maxX = Int32.MinValue;
            int maxY = Int32.MinValue;
            for (int y = 0; y < (int)gm.gmBlackBoxY; y++) {
                for (int x = 0; x < (int)gm.gmBlackBoxX; x++) {
                    if (GetPixel(x, y)) {
                        if (x > maxX)
                            maxX = x;
                        if (y > maxY)
                            maxY = y;
                    }
                }
            }
            int minX = Int32.MaxValue;
            int minY = Int32.MaxValue;
            for (int y = (int)gm.gmBlackBoxY - 1; y >= 0; y--) {
                for (int x = (int)gm.gmBlackBoxX - 1; x >= 0; x--) {
                    if (GetPixel(x, y)) {
                        if (x < minX)
                            minX = x;
                        if (y < minY)
                            minY = y;
                    }
                }
            }

            // Crea el bitmap
            //
            bitmap = new Bitmap(maxX - minX + 1, maxY - minY + 1);
            Color black = Color.FromKnownColor(KnownColor.Black);
            Color transparent = Color.FromKnownColor(KnownColor.Transparent);
            for (int y = minY; y <= maxY; y++)
                for (int x = minX; x <= maxX; x++)
                    bitmap.SetPixel(x - minX, y - minY, GetPixel(x, y) ? black : transparent);

            offsetX = gm.gmptGlyphOrigin.x + minX;
            offsetY = gm.gmptGlyphOrigin.y + minY;

            switch (fmt) {
                default:
                case FontAPI.GGOFormat.GGO_BITMAP:
                    format = GlyphFormat.L1;
                    break;

                case FontAPI.GGOFormat.GGO_GRAY2_BITMAP:
                    format = GlyphFormat.L2;
                    break;
            }
        }

        /// <summary>
        /// Obte el desplaçament horitzontal del bitmap d'ins de la cel·la del caracter.
        /// </summary>
        /// 
        public int OffsetX {
            get { return offsetX; }
        }

        /// <summary>
        /// Obte el desplaçament vertical del bitmap d'ins de la cel·la del caracter.
        /// </summary>
        /// 
        public int OffsetY {
            get { return offsetY; }
        }

        public int Width {
            get { return bitmap.Width; }
        }

        public int Height {
            get { return bitmap.Height; }
        }

        /// <summary>
        /// Obte el bitmap del caracter.
        /// </summary>
        /// 
        public Bitmap Bitmap {
            get { return bitmap; }
        }

        public GlyphFormat Format {
            get { return format; }
        }
    }
}
