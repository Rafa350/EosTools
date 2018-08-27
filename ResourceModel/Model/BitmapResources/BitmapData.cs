namespace EosTools.v1.ResourceModel.Model.BitmapResources {

    using System;
    using System.Windows;

    public sealed class BitmapData: IVisitable {

        private readonly string source;
        private readonly string format;

        public BitmapData(string source, string format) {

            this.source = source;
            this.format = format;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Source {
            get {
                return source;
            }
        }

        public string Format {
            get {
                return format;
            }
        }
    }
}
