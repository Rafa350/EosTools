namespace EosTools.v1.ResourceCompiler.Compiler.BitmapCompiler {

    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.BitmapResources;
    using System;
    using System.IO;
    using System.Reflection;

    using Bitmap = EosTools.v1.ResourceModel.Model.BitmapResources.Bitmap;
    using Color = System.Drawing.Color;
    using Image = System.Drawing.Bitmap;

    public class BitmapResourceCompiler {

        private const string defOutputExtension = "c";
        private const string defOutputHeaderExtension = "h";

        private readonly Version version;

        public BitmapResourceCompiler() {

            version = Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Compila el recurs.
        /// </summary>
        /// <param name="resource">El recurs.</param>
        /// <param name="outputPath">Carpeta des fitxert de sortida.</param>
        /// <param name="parameters">Parametres de compilacio.</param>
        /// 
        public void Compile(BitmapResource resource, string outputPath, CompilerParameters parameters) {

            TextWriter writer;
            string fileName, path;

            if (resource == null)
                throw new ArgumentNullException("resource");

            string outputBaseFileName = resource.ResourceId;
            string outputExtension = defOutputExtension;
            string outputHeaderExtension = defOutputHeaderExtension;

            if (parameters != null) {

                if (parameters.Exists("output-extension"))
                    outputExtension = parameters["output-extension"];

                if (parameters.Exists("output-header-extension"))
                    outputHeaderExtension = parameters["output-header-extension"];
            }

            fileName = String.Format("{0}.{1}", outputBaseFileName, outputExtension);
            path = Path.Combine(outputPath, fileName);

            writer = new StreamWriter(
                new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None));
            try {
                GenerateSource(resource, writer);
            }
            finally {
                writer.Close();
            }

            fileName = String.Format("{0}.{1}", outputBaseFileName, outputHeaderExtension);
            path = Path.Combine(outputPath, fileName);

            writer = new StreamWriter(
                new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None));
            try {
                GenerateHeader(resource, writer);
            }
            finally {
                writer.Close();
            }
        }

        private void GenerateHeader(BitmapResource resource, TextWriter writer) {

        }


        private void GenerateSource(BitmapResource resource, TextWriter writer) {

            const int columns = 5;
            Bitmap bitmap = resource.Bitmap;

            string fileName = Path.Combine(@"c:\temp\", bitmap.Source);
            Image image = BitmapUtils.LoadImage(fileName);

            writer.WriteLine("const unsigned char bitmap{0}[] = {{", resource.ResourceId);
            writer.WriteLine();
            writer.WriteLine("    0x{0:X2}, 0x{1:X2},    // width : {2}", image.Width >> 16, image.Width & 0xFF, image.Width);
            writer.WriteLine("    0x{0:X2}, 0x{1:X2},    // height: {2}", image.Height >> 16, image.Height & 0xFF, image.Height);
            switch (bitmap.Format) {
                case BitmapFormat.RGB565:
                    writer.WriteLine("    0x00, 0x{0:X2},    // flags : {1}", Convert.ToInt32(bitmap.Format), bitmap.Format.ToString());
                    break;
            }
            writer.WriteLine();

            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {

                    if ((x % columns) == 0)
                        writer.Write("    ");

                    Color color = image.GetPixel(x, y);
                    switch (bitmap.Format) {
                        case ResourceModel.Model.BitmapResources.BitmapFormat.RGB565: {
                            int r = (color.R >> 3) & 0x1F;
                            int g = (color.G >> 2) & 0x3F;
                            int b = (color.B >> 3) & 0x1F;
                            int c = (r << 11) | (g << 5) | b;
                            writer.Write(String.Format("0x{0:X2}, 0x{1:X2}, ", c & 0xFF, c >> 8));
                            break;
                        }
                    }

                    if ((x % columns) == (columns - 1))
                        writer.WriteLine();

                    else if (x == image.Width - 1)
                        writer.WriteLine();
                }
                if (y != image.Height - 1)
                    writer.WriteLine();
            }

            writer.WriteLine("};");

        }
    }
}
