namespace EosTools.v1.FontGeneratorApp {

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using EosTools.v1.FontGeneratorApp.Generator;
    using EosTools.v1.FontGeneratorApp.Generator.XML;
    using EosTools.v1.FontGeneratorApp.Model;

    class Program {

        static void Main(string[] args) {

            string outPath = null;
            string faceName = null;
            float emSize;
            char firstChar, lastChar;

            foreach (string arg in args) {

                if (arg.StartsWith("/F:", StringComparison.OrdinalIgnoreCase))
                    faceName = arg.Substring(3);

                else if (arg.StartsWith("/P:", StringComparison.OrdinalIgnoreCase))
                    outPath = arg.Substring(3);

                else if (arg.StartsWith("/S:", StringComparison.OrdinalIgnoreCase))
                    emSize = Convert.ToSingle(arg.Substring(3));

                else if (arg.StartsWith("/FC:", StringComparison.OrdinalIgnoreCase))
                    firstChar = Convert.ToChar(arg.Substring(4));

                else if (arg.StartsWith("/LC:", StringComparison.OrdinalIgnoreCase))
                    lastChar = Convert.ToChar(arg.Substring(4));

                else if (arg.StartsWith("/H", StringComparison.OrdinalIgnoreCase))
                    break;

            }

            if (String.IsNullOrEmpty(outPath))
                outPath = @"..\..\Data";

            List<FontDescriptor> fontDescriptors = new List<FontDescriptor>();

            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 7), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 8), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 10), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 12), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 14), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 18), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 24), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 8), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 10), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 12), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 14), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 18), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Consolas", 24), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 8), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 10), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 12), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 14), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 18), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 24), null, '0', '9'));

            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 7), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 8), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 10), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 12), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 14), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 7), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 7), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 8), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 10), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 12), null, ' ', 'z'));

            //fontDescriptors.Add(new FontDescriptor(new Font("Fixedsys", 12), null, ' ', 'z'));

            //fontDescriptors.Add(new FontDescriptor(new Font("5x7 practical", 12), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 7, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 8, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 10, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 12, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 14, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 18, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("MS Sans Serif", 24, FontStyle.Bold), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 8, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 10, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 12, FontStyle.Bold), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 14, FontStyle.Bold), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 18, FontStyle.Bold), null, '0', '9'));
            fontDescriptors.Add(new FontDescriptor(new Font("Arial", 24, FontStyle.Bold), null, '0', '9'));

            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 7, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 8, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Tahoma", 10, FontStyle.Bold), null, ' ', 'z'));

            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 7, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 8, FontStyle.Bold), null, ' ', 'z'));
            fontDescriptors.Add(new FontDescriptor(new Font("Courier New", 10, FontStyle.Bold), null, ' ', 'z'));
            
            GenerateXmlCode(outPath, fontDescriptors);
        }

        private static void GenerateXmlCode(string path, List<FontDescriptor> fontDescriptors) {

            TextWriter writer;

            ICodeGenerator generator = new XmlCodeGenerator();

            foreach (FontDescriptor fontDescriptor in fontDescriptors) {
                string fileName = Path.Combine(path, fontDescriptor.FontIdString) + ".xfont";
                writer = new StreamWriter(fileName);
                try {
                    generator.GenerateFontSource(writer, fontDescriptor);
                }
                finally {
                    writer.Close();
                }
            }
        }

        private static void ShowCredits() {
        }

        private static void ShowHelp() {
        }
    }
}
