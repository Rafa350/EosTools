namespace EosTools.v1.ResourceModel.Model.FontResources {

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    public sealed class Font: IVisitable {

        private readonly string name;
        private readonly int height;
        private readonly int ascent;
        private readonly int descent;
        private readonly List<FontChar> chars = new List<FontChar>();

        public Font(string name, int height, int ascent, int descent, IEnumerable<FontChar> chars) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
            this.height = height;
            this.ascent = ascent;
            this.descent = descent;
            this.chars.AddRange(chars);
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name {
            get {
                return name;
            }
        }

        public int Height {
            get {
                return height;
            }
        }

        public int Ascent {
            get {
                return ascent;
            }
        }
        
        public int Descent {
            get {
                return descent;
            }
        }

        public ReadOnlyCollection<FontChar> Chars {
            get {
                return chars.AsReadOnly();
            }
        }
    }
}
