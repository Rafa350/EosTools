﻿namespace EosTools.v1.ResourceCompiler.Compiler.BitmapCompiler {

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
        private bool useProxyVariable = false;

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
                throw new ArgumentNullException(nameof(resource));

            string outputBaseFileName = resource.Id;
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

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Genera la presentacio inicial
            //
            writer.WriteLine("/*************************************************************************");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Archivo generado desde un archivo de recursos.");
            writer.WriteLine(" *       No modificar!");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Bitmap : {0}", fileName);
            writer.WriteLine(" *");
            writer.WriteLine(" *       Fecha de generacion  : {0}", DateTime.Now);
            writer.WriteLine(" *       Nombre del generador : {0}", "EosResourceCompiler");
            writer.WriteLine(" *       Version del generador: {0}", version);
            writer.WriteLine(" *");
            writer.WriteLine(" ************************************************************************/");
            writer.WriteLine();
            writer.WriteLine();

            if (useProxyVariable)
                writer.WriteLine("static const unsigned char bitmap[] = {");
            else
                writer.WriteLine("const unsigned char bitmap{0}[] = {{", resource.Id);
            writer.WriteLine();

            int offset = 0;

            writer.WriteLine("                   // BITMAPINFO");
            writer.WriteLine("    /* {0:X4} */     0x{1:X2}, 0x{2:X2},    // width : {3}", offset, image.Width & 0xFF, image.Width >> 8, image.Width);
            offset += 2;

            writer.WriteLine("    /* {0:X4} */     0x{1:X2}, 0x{2:X2},    // height: {3}", offset, image.Height & 0xFF, image.Height >> 8, image.Height);
            offset += 2;

            switch (bitmap.Format) {
                case BitmapFormat.RGB565:
                    writer.WriteLine("    /* {0:X4} */     0x00, 0x{1:X2},    // flags : {1}", offset, Convert.ToInt32(bitmap.Format), bitmap.Format.ToString());
                    offset += 2;
                    break;
            }
            writer.WriteLine();

            writer.WriteLine("                   // PIXELS");
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {

                    if ((x % columns) == 0)
                        writer.Write("    /* {0:X4} */     ", offset);

                    Color color = image.GetPixel(x, y);
                    switch (bitmap.Format) {
                        case ResourceModel.Model.BitmapResources.BitmapFormat.RGB565: {
                            int r = (color.R >> 3) & 0x1F;
                            int g = (color.G >> 2) & 0x3F;
                            int b = (color.B >> 3) & 0x1F;
                            int c = (r << 11) | (g << 5) | b;
                            writer.Write(String.Format("0x{0:X2}, 0x{1:X2}, ", c & 0xFF, c >> 8));
                            offset++;
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
            writer.WriteLine();

            if (useProxyVariable) {
                writer.WriteLine("const unsigned char *bitmap{0} = bitmap;", resource.Id);
                writer.WriteLine();
            }
        }
    }
}
