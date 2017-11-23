namespace EosBitmapGenerator {

    using System;
    using System.Drawing;

    class Program {

        static void Main(string[] args) {

            string fileName = null;
            string outPath = null;

            foreach (string arg in args) {
                if (arg.StartsWith("/P:"))
                    outPath = arg.Substring(3);
            }

            Image image = Image.FromFile(fileName);
            Bitmap bitmap = new Bitmap(image);
            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++) {
                    Color c = bitmap.GetPixel(x, y);
                    
                }
        }
    }
}
