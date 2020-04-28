namespace EosTools.v1.FontGeneratorApp.Model {

    using System;
    using System.Drawing;

    public class CharacterDescriptor {

        private readonly Font font;
        private readonly char character;

        public CharacterDescriptor(Font font, char character) {

            if (font == null)
                throw new ArgumentNullException(nameof(font));

            this.font = font;
            this.character = character;
        }

        public Font Font {
            get { return font; }
        }

        public char Character {
            get { return character; }
        }
    }
}