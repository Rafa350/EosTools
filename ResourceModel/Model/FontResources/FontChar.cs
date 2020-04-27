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

        public int Code => code;

        public int Left => left;

        public int Top => top;

        public int Width => width;

        public int Height => height;

        public int Advance => advance;

        public byte[] Bitmap => bitmap;
    }
}
