namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.FontResources;
    
    /// <summary>
    /// Recurs que representa un font.
    /// </summary>
    public sealed class FontResource: Resource {

        private readonly Font font;

        /// <summary>
        /// Constructor del recurs.
        /// </summary>
        /// <param name="resourceId">Identificador del recurs.</param>
        /// <param name="languaje">Llenguatge del recurs.</param>
        /// <param name="font">El font que conte aquest recurs.</param>
        public FontResource(string resourceId, string languaje, Font font) :
            base(resourceId, languaje) {

            if (font == null)
                throw new ArgumentNullException("font");

            this.font = font;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el font contingut en el recurs.
        /// </summary>
        public Font Font {
            get {
                return font;
            }
        }
    }
}
