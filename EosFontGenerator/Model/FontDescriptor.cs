namespace Media.PicFontGenerator.Model {

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Media.PicFontGenerator.Infrastructure;

    public class FontDescriptor {

        private readonly Font font;
        private readonly string name;
        private readonly FontInfo fontInfo;
        private readonly Dictionary<char, CharacterDescriptor> characterDescriptors = new Dictionary<char, CharacterDescriptor>();

        public FontDescriptor(Font font, string name, char firstChar, char lastChar) :
            this(font, name, MakeCharacterString(firstChar, lastChar)) {
        }

        public FontDescriptor(Font font, string name, string characters) {

            if (font == null)
                throw new ArgumentNullException("font");

            if (String.IsNullOrEmpty(characters))
                throw new ArgumentNullException("characters");

            this.font = font;
            this.name = name;

            fontInfo = FontAPI.GetFontInfo(font);

            foreach (char character in characters)
                characterDescriptors.Add(character, new CharacterDescriptor(this, character));
        }

        private static string MakeCharacterString(char firstChar, char lastChar) {

            StringBuilder sb = new StringBuilder();
            for (char ch = firstChar; ch <= lastChar; ch++)
                sb.Append(ch);
            return sb.ToString();
        }

        public CharacterDescriptor GetDescriptor(char ch) {

            CharacterDescriptor characterDescriptor;

            if (characterDescriptors.TryGetValue(ch, out characterDescriptor))
                return characterDescriptor;
            else
                return null;
        }

        public IEnumerable<CharacterDescriptor> CharacterDescriptors {
            get {
                return characterDescriptors.Values;
            }
        }

        public string FontIdString {
            get {
                string sStyle = String.Empty;
                if (font.Bold)
                    sStyle += "Bold";
                if (font.Italic)
                    sStyle += "Italic";
                return String.Format("{0}{1}{2}pt",
                    String.IsNullOrEmpty(name) ? font.Name.Replace(" ", null) : name,
                    sStyle,
                    font.SizeInPoints);
            }
        }

        public Font Font {
            get {
                return font;
            }
        }

        public string Name {
            get {
                return name;
            }
        }

        public int NumChars {
            get {
                return characterDescriptors.Count;
            }
        }

        public int Height {
            get {
                return fontInfo.Height;
            }
        }

        public int Ascent {
            get {
                return fontInfo.Ascent;
            }
        }
        
        public int Descent {
            get {
                return fontInfo.Descent;
            }
        }
    }
}
