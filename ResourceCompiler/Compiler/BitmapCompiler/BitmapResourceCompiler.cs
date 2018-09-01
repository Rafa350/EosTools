namespace EosTools.v1.ResourceCompiler.Compiler.BitmapCompiler {

    using EosTools.v1.ResourceModel.Model;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Bitmap = EosTools.v1.ResourceModel.Model.BitmapResources.Bitmap;
    using Image = System.Drawing.Bitmap;
    using Color = System.Drawing.Color;

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
            writer.WriteLine("    0x{0:X2}, 0x{1:X2},", image.Width >> 16, image.Width & 0xFF);
            writer.WriteLine("    0x{0:X2}, 0x{1:X2},", image.Height >> 16, image.Height & 0xFF);
            writer.WriteLine();

            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {

                    if ((x % columns) == 0)
                        writer.Write("    ");

                    Color color = image.GetPixel(x, y);
                    writer.Write(String.Format("0x{0:X2}, 0x{1:X2}, 0x{2:X2}, ", color.R, color.G, color.B));

                    if ((x % columns) == (columns - 1))
                        writer.WriteLine();

                    else if (x == image.Width - 1)
                        writer.WriteLine();
                }
            }

            writer.WriteLine("};");

        }
    }
}
