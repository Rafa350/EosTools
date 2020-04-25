namespace EosTools.v1.ResourceModel.Model.BitmapResources {

    using System;

    public enum BitmapFormat {
        RGB565 = 0,
        RGB888 = 1,
        ARGB8888 = 2,
    }

    public class Bitmap: IVisitable {

        private readonly string source;
        private BitmapFormat format;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="source">Origen del bitmap.</param>
        /// <param name="format">Format.</param>
        /// 
        public Bitmap(string source, BitmapFormat format) {

            if (String.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));

            this.source = source;
            this.format = format;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte l'origen del bitmap.
        /// </summary>
        /// 
        public string Source {
            get {
                return source;
            }
        }

        /// <summary>
        /// Obte el format.
        /// </summary>
        /// 
        public BitmapFormat Format {
            get {
                return format;
            }
        }
    }
}
