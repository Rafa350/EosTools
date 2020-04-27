namespace EosTools.v1.ResourceModel.Model.FontResources {

    using System;
    using System.Collections.Generic;

    public sealed class Font: IVisitable {

        private readonly string name;
        private readonly int height;
        private readonly int ascent;
        private readonly int descent;
        private readonly IEnumerable<FontChar> chars;

        public Font(string name, int height, int ascent, int descent, IEnumerable<FontChar> chars) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.height = height;
            this.ascent = ascent;
            this.descent = descent;
            this.chars = chars;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;

        public int Height => height;

        public int Ascent => ascent;

        public int Descent => descent;

        public IEnumerable<FontChar> Chars => chars;
    }
}
