namespace Media.PicFontGenerator.Model {

    using System;
    using System.Drawing;
    using Media.PicFontGenerator.Infrastructure;

    public class CharacterDescriptor {

        private readonly FontDescriptor fontDescriptor;
        private readonly char character;
        private readonly GlyphInfo glyphInfo;

        public CharacterDescriptor(FontDescriptor fontDescriptor, char character) {

            if (fontDescriptor == null)
                throw new ArgumentNullException("fontDescriptor");

            this.fontDescriptor = fontDescriptor;
            this.character = character;

            glyphInfo = FontAPI.GetGlyphInfo(fontDescriptor.Font, character);
        }

        public FontDescriptor FontDescriptor {
            get {
                return fontDescriptor;
            }
        }

        public char Character {
            get {
                return character;
            }
        }

        public Bitmap Bitmap {
            get {
                return glyphInfo.Bitmap;
            }
        }

        public int Advance {
            get {
                return glyphInfo.Advance;
            }
        }

        public int Width {
            get {
                return glyphInfo.Width;
            }
        }

        public int Height {
            get {
                return glyphInfo.Height;
            }
        }

        public int Left {
            get {
                return glyphInfo.Left;
            }
        }

        public int Top {
            get {
                return glyphInfo.Top;
            }
        }
    }
}