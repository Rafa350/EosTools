namespace EosTools.v1.FileSystemGeneratorApp {

    using System.Collections.Generic;
    using System.IO;

    public sealed class ROMFsGenerator {

        /// <summary>
        /// Constructor del objecte.
        /// </summary>
        /// 
        public ROMFsGenerator() {

        }

        /// <summary>
        /// Genera el fitxers del sistema de fitxers.
        /// </summary>
        /// <param name="srcFolder">Ruta de la carpeta origen.</param>
        /// <param name="dstFileName">Fitxer de resultat.</param>
        /// 
        public void Generate(string srcFolder, string dstFileName) {

            using (TextWriter writer = new StreamWriter(
                new FileStream(dstFileName, FileMode.Create, FileAccess.Write, FileShare.None))) {

                writer.WriteLine("const char romFileSystem[] = {");

                foreach (var srcFileName in EnumerateFiles(srcFolder)) {

                    using (Stream input = new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {

                        string fileName = srcFileName.Replace(srcFolder, null).Replace(Path.DirectorySeparatorChar, '/');
                        writer.WriteLine();
                        writer.WriteLine("  // {0}", fileName);
                        writer.WriteLine("  0x{0:X2},", fileName.Length);
                        writer.Write("  ");
                        foreach (var ch in fileName)
                            writer.Write("'{0}', ", ch);
                        writer.WriteLine();

                        int length = (int)input.Length;
                        writer.WriteLine("  0x{0:X2}, 0x{1:X2}, ", length & 0xFF, length >> 8);

                        byte[] buffer = new byte[16];
                        int count = input.Read(buffer, 0, buffer.Length);
                        while (count > 0) {
                            bool first = true;
                            for (int i = 0; i < count; i++) {
                                if (first) {
                                    first = false;
                                    writer.Write("  ");
                                }
                                else
                                    writer.Write(", ");
                                writer.Write("0x{0:X2}", buffer[i]);
                            }
                            writer.WriteLine();
                            count = input.Read(buffer, 0, buffer.Length);
                        }
                    }
                }

                writer.WriteLine("};");
            }
        }

        /// <summary>
        /// Enumera els noms dels fitxers a procesar.
        /// </summary>
        /// <param name="srcFolder">La carpeta a analitzar.</param>
        /// <returns>Els noms dels fitxers a procesar.</returns>
        /// 
        private static IEnumerable<string> EnumerateFiles(string srcFolder) {

            if (!srcFolder.EndsWith(Path.DirectorySeparatorChar))
                srcFolder += Path.DirectorySeparatorChar;

            return Directory.EnumerateFiles(srcFolder, "*.*", SearchOption.AllDirectories);
        }
    }
}
