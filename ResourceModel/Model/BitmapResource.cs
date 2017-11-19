namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.BitmapResources;
    
    /// <summary>
    /// Recurs que representa un font.
    /// </summary>
    public sealed class BitmapResource: Resource {

        /// <summary>
        /// Constructor del recurs.
        /// </summary>
        /// <param name="resourceId">Identificador del recurs.</param>
        /// <param name="languaje">Llenguatge del recurs.</param>
        /// <param name="font">El font que conte aquest recurs.</param>
        public BitmapResource(string resourceId, string languaje) :
            base(resourceId, languaje) {

        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
