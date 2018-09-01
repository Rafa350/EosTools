namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.BitmapResources;

    /// <summary>
    /// Recurs que representa un bitmap.
    /// </summary>
    /// 
    public sealed class BitmapResource: Resource {

        private readonly Bitmap bitmap;

        /// <summary>
        /// Constructor del recurs.
        /// </summary>
        /// <param name="resourceId">Identificador del recurs.</param>
        /// <param name="languaje">Llenguatge del recurs.</param>
        /// <param name="bitmap">El bitmap que conte aquest recurs.</param>
        /// 
        public BitmapResource(string resourceId, string languaje, Bitmap bitmap) :
            base(resourceId, languaje) {

            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            this.bitmap = bitmap;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el bitmap contingut en el recurs.
        /// </summary>
        /// 
        public Bitmap Bitmap {
            get {
                return bitmap;
            }
        }
    }
}
