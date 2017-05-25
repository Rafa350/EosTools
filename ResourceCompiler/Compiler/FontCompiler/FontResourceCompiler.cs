namespace EosTools.v1.ResourceCompiler.Compiler.FontCompiler {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.FontResources;

    public class FontResourceCompiler {

        private bool useProxyVariable = false;
        private string outputExtension = "c";
        private string includeFiles;

        public void Compile(FontResource fontResource, string outputPath, CompilerParameters parameters) {

            if ((parameters != null) && parameters.Exists("use-proxy-variable"))
                useProxyVariable = true;

            if ((parameters != null) && parameters.Exists("output-extension"))
                outputExtension = parameters["output-extension"];

            if ((parameters != null) && parameters.Exists("include-files"))
                includeFiles = parameters["include-files"];

            string fileName = String.Format("{0}.{1}", fontResource.ResourceId, outputExtension);
            string path = Path.Combine(outputPath, fileName);
            TextWriter writer = new StreamWriter(
                new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None));
            try {
                GenerateOutput(fontResource.Font, writer);
            }
            finally {
                writer.Close();
            }
        }

        private void GenerateOutput(Font font, TextWriter writer) {

            // Calcula el primer i l'ultim caracter que conte el font
            //
            int firstChar = Int32.MaxValue;
            int lastChar = Int32.MinValue;
            foreach (FontChar fontChar in font.Chars) {
                if (firstChar > fontChar.Code)
                    firstChar = fontChar.Code;
                if (lastChar < fontChar.Code)
                    lastChar = fontChar.Code;
            }

            List<int> charList = new List<int>();
            foreach (FontChar fontChar in font.Chars)
                charList.Add(fontChar.Code);

            // Calcula els offsets dels bitmaps de cada caracter
            //
            Dictionary<int, int> bitmapOffsets = new Dictionary<int, int>();
            int bitmapOffset = 0;
            foreach (FontChar fontChar in font.Chars) {
                if (fontChar.Bitmap != null && fontChar.Bitmap.Length > 0) {
                    bitmapOffsets.Add(fontChar.Code, bitmapOffset);
                    bitmapOffset += fontChar.Bitmap.Length;
                }
            }

            string fontName = font.Name.Replace(" ", "");

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Genera la presentacio inicial
            //
            writer.WriteLine("/*************************************************************************");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Archivo generado desde un archivo de recursos.");
            writer.WriteLine(" *       No modificar!");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Fuente : {0}", font.Name);
            writer.WriteLine(" *");
            writer.WriteLine(" *       Fecha de generacion  : {0}", DateTime.Now);
            writer.WriteLine(" *       Nombre del generador : {0}", "EosResourceCompiler");
            writer.WriteLine(" *       Version del generador: {0}", version);
            writer.WriteLine(" *");
            writer.WriteLine(" ************************************************************************/");
            writer.WriteLine();
            writer.WriteLine();

            // Inclueix fitxers si cal
            //
            if (!String.IsNullOrEmpty(includeFiles)) {
                string[] files = includeFiles.Split(';');
                foreach (string file in files)
                    writer.WriteLine("#include \"{0}\"", file);
                writer.WriteLine();
                writer.WriteLine();
            }

            writer.WriteLine("#ifdef FONT_USE_{0}", fontName);
            writer.WriteLine();
            writer.WriteLine();

            int addr = 0;

            // Calcula els offsets al inici de cada seccio
            //
            const int fontInfoBytes = 8;
            const int charMapBytes = 2;
            const int charInfoBytes = 7;
            int charInfoOffset = (charMapBytes * (lastChar - firstChar + 1)) + fontInfoBytes;
            int charBitsOffset = charInfoOffset + (charList.Count * charInfoBytes);

            // Genera la capcelera del font
            //
            byte flags = 0;
            if (useProxyVariable)
                writer.WriteLine("static const unsigned char font[] = {");
            else
                writer.WriteLine("const unsigned char font{0}[] = {{", fontName);
            writer.WriteLine();
            writer.WriteLine("              // FONTINFO");
            writer.WriteLine("/* {8:X4} */    0x{0:X2}, 0x{1:X2}, 0x{2:X2}, 0x{3:X2}, 0x{4:X2}, 0x{5:X2}, 0x{6:X2}, 0x{7:X2},",
                flags,
                font.Height,
                font.Ascent,
                font.Descent,
                firstChar,
                lastChar,
                (byte) (fontInfoBytes % 256),
                (byte) (fontInfoBytes / 256),
                addr);
            addr += fontInfoBytes;
            writer.WriteLine();

            // Genera el mapa de caracters
            //
            writer.WriteLine("              // CHARMAP");
            for (int code = firstChar; code <= lastChar; code++) {
                if (charList.Contains(code)) {
                    writer.WriteLine("/* {2:X4} */    0x{0:X2}, 0x{1:X2},",
                        (byte) (charInfoOffset % 256),
                        (byte) (charInfoOffset / 256),
                        addr);
                    addr += charMapBytes;
                    charInfoOffset += charInfoBytes;
                }
                else
                    writer.WriteLine("    0xFF, 0xFF,");
            }
            writer.WriteLine();

            // Genera la taula de caracters
            //
            writer.WriteLine("              // CHARINFO");
            foreach (FontChar fontChar in font.Chars) {
                int offset;
                if (bitmapOffsets.TryGetValue(fontChar.Code, out offset))
                    offset += charBitsOffset;
                else
                    offset = 0xFFFF;

                writer.WriteLine("/* {7:X4} */    0x{0:X2}, 0x{1:X2}, 0x{2:X2}, 0x{3:X2}, 0x{4:X2}, 0x{5:X2}, 0x{6:X2},",
                    fontChar.Width,
                    fontChar.Height,
                    (byte) fontChar.Left,
                    (byte) fontChar.Top,
                    (byte) fontChar.Advance,
                    (offset % 256),
                    (offset / 256),
                    addr);
                addr += charInfoBytes;
            }
            writer.WriteLine();

            // Genera la taula de bitmaps
            //
            writer.WriteLine("              // CHARBITS");
            foreach (FontChar fontChar in font.Chars) {
                if (fontChar.Bitmap != null && fontChar.Bitmap.Length > 0) {
                    int columnCount = 0;
                    int maxCount = fontChar.Bitmap.Length;
                    for (int byteCount = 0; byteCount < maxCount; byteCount++) {
                        byte b = fontChar.Bitmap[byteCount];
                        if (byteCount == 0) 
                            writer.Write("/* {0:X4} */    ", addr);
                        writer.Write("0x{0:X2}, ", b);
                        columnCount++;
                        if (columnCount == 8) {
                            columnCount = 0;
                            writer.WriteLine();
                            writer.Write("              ");
                        }
                    }
                    if (columnCount > 0)
                        writer.WriteLine();
                    addr += fontChar.Bitmap.Length;
                }
            }
            writer.WriteLine("};");
            writer.WriteLine();

            if (useProxyVariable) {
                writer.WriteLine("const unsigned char *font{0} = font;", fontName);
                writer.WriteLine();
            }

            writer.WriteLine();
            writer.WriteLine("#endif");
        }
    }
}
