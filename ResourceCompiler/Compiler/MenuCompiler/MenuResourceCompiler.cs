namespace EosTools.v1.ResourceCompiler.Compiler.MenuCompiler {

    using EosTools.v1.ResourceCompiler.Compiler;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Compila un fitxer de recursos de menu, i genera els fitxers font i 
    /// de capcelera
    /// </summary>
    /// 
    public sealed class MenuResourceCompiler {

        private const string defOutputExtension = "c";
        private const string defOutputHeaderExtension = "h";

        private readonly Version version;

        private int offset = 0;
        private IDictionary<Item, int> itemOffsets;
        private IList<string> commands;

        public MenuResourceCompiler() {

            version = Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Compila el recurs.
        /// </summary>
        /// <param name="resource">Recurs del menu.</param>
        /// <param name="outputPath">Carpeta del fitxer de sortida.</param>
        /// <param name="parameters">Parametres de compilacio.</param>
        /// 
        public void Compile(MenuResource resource, string outputPath, CompilerParameters parameters) {

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

        private void GenerateHeader(MenuResource resource, TextWriter writer) {

            // Calcula la llista de comandes
            //
            commands = MenuUtils.GetCommandList(resource);

            writer.WriteLine("/*************************************************************************");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Archivo generado desde un archivo de recursos");
            writer.WriteLine(" *       No modificar!");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Fecha de generación  : {0}", DateTime.Now);
            writer.WriteLine(" *       Nombre del generador : {0}", "EosResourceCompiler");
            writer.WriteLine(" *       Version del generador: {0}", version);
            writer.WriteLine(" *");
            writer.WriteLine(" ************************************************************************/");
            writer.WriteLine();
            writer.WriteLine();

            for (int i = 0; i < commands.Count; i++)
                writer.WriteLine("#define {0} {1}", commands[i], 100 + i);
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("extern const unsigned char menu{0}[];", resource.ResourceId);
        }

        private void GenerateSource(MenuResource menuResource, TextWriter writer) {

            // Calcula els offsets al elements del menu
            //
            itemOffsets = MenuUtils.GetOffsetDictionary(menuResource);

            writer.WriteLine("/*************************************************************************");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Archivo generado desde un archivo de recursos");
            writer.WriteLine(" *       No modificar!");
            writer.WriteLine(" *");
            writer.WriteLine(" *       Fecha de generación  : {0}", DateTime.Now);
            writer.WriteLine(" *       Nombre del generador : {0}", "EosResourceCompiler");
            writer.WriteLine(" *       Version del generador: {0}", version);
            writer.WriteLine(" *");
            writer.WriteLine(" ************************************************************************/");
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("#include \"{0}.h\"", menuResource.ResourceId);
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("const unsigned char menu{0}[] = {{", menuResource.ResourceId);
            writer.WriteLine();
            GenerateMenu(writer, menuResource.Menu);
            writer.WriteLine("};");
        }

        private void GenerateMenu(TextWriter writer, Menu menu) {

            writer.WriteLine("                // MENUINFO");
            writer.WriteLine("  /* {0:X4} */    0x{1:X2}, ", 
                offset,
                menu.Items.Count);
            offset += 1;

            writer.Write("  /* {0:X4} */    0x{1:X2}, ", 
                offset,
                menu.Title.Length);
            int byteCount = 1;
            foreach (char ch in menu.Title) {
                if (byteCount == 0)
                    writer.Write("              ");
                writer.Write("'{0}', ", ch);
                byteCount++;
                if (byteCount == 8) {
                    writer.WriteLine();
                    byteCount = 0;
                }
            }
            if (byteCount != 0)
                writer.WriteLine();
            offset += menu.Title.Length + 1;

            writer.WriteLine("                // ITEMMAP");
            foreach (Item item in menu.Items) {
                int itemOffset = itemOffsets[item];
                writer.WriteLine("  /* {0:X4} */    0x{1:X2}, 0x{2:X2}, ", 
                    offset, 
                    itemOffset % 256, 
                    itemOffset / 256);
                offset += 2;
            }

            foreach (Item item in menu.Items) {
                if (item is CommandItem)
                    GenerateCommandItem(writer, item as CommandItem);
                else if (item is MenuItem)
                    GenerateMenuItem(writer, item as MenuItem);
                else
                    GenerateExitItem(writer, item as ExitItem);
            }
        }

        private void GenerateCommandItem(TextWriter writer, CommandItem item) {

            writer.WriteLine("                // COMMANDITEMINFO");

            writer.WriteLine("  /* {0:X4} */    0x00,",
                offset);
            offset += 1;

            writer.Write("  /* {0:X4} */    0x{1:X2}, ",
                offset,
                item.Title.Length);
            int byteCount = 1;
            foreach (char ch in item.Title) {
                if (byteCount == 0)
                    writer.Write("              ");
                writer.Write("'{0}', ", ch);
                byteCount++;
                if (byteCount == 8) {
                    writer.WriteLine();
                    byteCount = 0;
                }
            }
            if (byteCount != 0)
                writer.WriteLine();
            offset += item.Title.Length + 1;

            writer.WriteLine("  /* {0:X4} */    {1}, ",
                offset,
                item.Command);
            offset += 1;
        }

        private void GenerateExitItem(TextWriter writer, ExitItem item) {

            writer.WriteLine("                // EXITITEMINFO");

            writer.WriteLine("  /* {0:X4} */    0x02,",
                offset);
            offset += 1;

            writer.Write("  /* {0:X4} */    0x{1:X2}, ",
                offset,
                item.Title.Length);
            int byteCount = 1;
            foreach (char ch in item.Title) {
                if (byteCount == 0)
                    writer.Write("              ");
                writer.Write("'{0}', ", ch);
                byteCount++;
                if (byteCount == 8) {
                    writer.WriteLine();
                    byteCount = 0;
                }
            }
            if (byteCount != 0)
                writer.WriteLine();
            offset += item.Title.Length + 1;
        }

        private void GenerateMenuItem(TextWriter writer, MenuItem item) {

            writer.WriteLine("                // MENUITEMINFO");

            writer.WriteLine("  /* {0:X4} */    0x01, ",
                offset);
            offset += 1;

            writer.Write("  /* {0:X4} */    0x{1:X2}, ",
                offset,
                item.Title.Length);
            int byteCount = 1;
            foreach (char ch in item.Title) {
                if (byteCount == 0)
                    writer.Write("              ");
                writer.Write("'{0}', ", ch);
                byteCount++;
                if (byteCount == 8) {
                    writer.WriteLine();
                    byteCount = 0;
                }
            }
            if (byteCount != 0)
                writer.WriteLine();
            offset += item.Title.Length + 1;

            if (item.SubMenu != null)
                GenerateMenu(writer, item.SubMenu);
        }
    }
}
