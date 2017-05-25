namespace EosTools.v1.ResourceModel.Model.FontResources {

    public sealed class FontChar: IVisitable {

        private readonly int code;
        private readonly int left;
        private readonly int top;
        private readonly int width;
        private readonly int height;
        private readonly int advance;
        private readonly byte[] bitmap;

        public FontChar(int code, int left, int top, int width, int height, int advance, byte[] bitmap) {

            this.code = code;
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.advance = advance;
            this.bitmap = bitmap;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public int Code {
            get { return code; }
        }

        public int Left {
            get { return left; }
        }

        public int Top {
            get { return top; }
        }

        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        public int Advance {
            get { return advance; }
        }

        public byte[] Bitmap {
            get { return bitmap; }
        }
    }
}
