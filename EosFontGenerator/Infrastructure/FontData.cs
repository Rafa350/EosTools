namespace EosTools.v1.FontGeneratorApp.Infrastructure {

    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Clase que representa informacio global d'un font.
    /// </summary>
    /// 
    public sealed class FontData {

        private readonly string name;
        private readonly string fontName;
        private readonly string familyName;
        private readonly int ascent;
        private readonly int descent;
        private readonly int height;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="font">El font.</param>
        /// 
        public FontData(Font font) {

            using (Bitmap bmp = new Bitmap(1, 1)) {
                using (Graphics graphics = Graphics.FromImage(bmp)) {

                    IntPtr hdc = graphics.GetHdc();
                    try {
                        IntPtr hOldFont = FontAPI.SelectObject(hdc, font.ToHfont());
                        try {
                            FontAPI.TEXTMETRICS tm;

                            if (!FontAPI.GetTextMetrics(hdc, out tm)) {
                                int error = Marshal.GetLastWin32Error();
                                throw new InvalidOperationException(
                                    String.Format("GetTextMetrics: ERROR '{0}", error));
                            }

                            familyName = font.FontFamily.Name;
                            fontName = font.Name;

                            StringBuilder sb = new StringBuilder();
                            sb.Append(font.Name);
                            if (font.Bold)
                                sb.Append(", Bold");
                            if (font.Italic)
                                sb.Append(", Italic");
                            sb.AppendFormat(", {0}pt", font.SizeInPoints);
                            name = sb.ToString();

                            ascent = tm.tmAscent;
                            descent = tm.tmDescent;
                            height = tm.tmHeight;
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

        /// <summary>
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name {
            get { return name; }
        }

        public string FamilyName {
            get { return familyName; }
        }

        public string FontName {
            get { return fontName; }
        }

        /// <summary>
        /// Obte l'ascendent.
        /// </summary>
        /// 
        public int Ascent {
            get { return ascent; }
        }

        /// <summary>
        /// Obte el descendent.
        /// </summary>
        /// 
        public int Descent {
            get { return descent; }
        }

        /// <summary>
        /// Obte l'alçada.
        /// </summary>
        /// 
        public int Height {
            get { return height; }
        }
    }
}
