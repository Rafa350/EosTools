namespace EosTools.v1.ResourceModel.IO {

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using EosTools.v1.ResourceModel.IO.Xml;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.BitmapResources;
    using EosTools.v1.ResourceModel.Model.FontResources;
    using EosTools.v1.ResourceModel.Model.FontTableResources;
    using EosTools.v1.ResourceModel.Model.MenuResources;

    public class ResourceReader: IResourceReader {

        private static readonly XmlSchemaSet schemas;
        private readonly XmlNamespaceManager namespaceManager;
        private readonly XmlReader reader;

        /// <summary>
        /// Constructor estatic de la clase
        /// </summary>
        /// 
        static ResourceReader() {

            schemas = new XmlSchemaSet();

            string schemaResourceName = "EosTools.v1.ResourceModel.IO.Schemas.Resource.xsd";
            Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(schemaResourceName);
            if (resourceStream == null)
                throw new Exception(String.Format("No se encontro el recurso '{0}'", schemaResourceName));
            XmlSchema schema = XmlSchema.Read(resourceStream, null);
            schemas.Add(schema);

            schemas.Compile();
        }

        public ResourceReader(Stream stream) {

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            settings.CloseInput = false;
            settings.Schemas = schemas;
            settings.ValidationType = schemas == null ? ValidationType.None : ValidationType.Schema;
            settings.ConformanceLevel = ConformanceLevel.Document;

            reader = XmlReader.Create(stream, settings);

            namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace(String.Empty, "http://MikroPic.com/schemas/eosTools/v3/Resources.xsd");
        }

        public ResourcePool Read() {

            try {

                XmlDocument document = new XmlDocument();
                document.Load(reader);

                List<Resource> resources = new List<Resource>();

                XmlNode node = document.DocumentElement;
                foreach (XmlNode childNode in node.ChildNodes) {
                    switch (childNode.Name) {
                        case "menuResource":
                            resources.Add(ProcessMenuResource(childNode));
                            break;

                        case "fontResource":
                            resources.Add(ProcessFontResource(childNode));
                            break;

                        case "fontTableResource":
                            resources.Add(ProcessFontTableResource(childNode));
                            break;

                        case "formResource":
                            resources.Add(ProcessFormResource(childNode));
                            break;

                        case "bitmapResource":
                            resources.Add(ProcessBitmapResource(childNode));
                            break;
                    }
                }

                return new ResourcePool(resources);
            }

            catch (XmlSchemaValidationException ex) {

                throw new Exception(
                    String.Format("Error de validacion en linea {0}, posicion {1}", ex.LineNumber, ex.LinePosition), ex);
            }

            catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// Procesa un node 'menuResource'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>El recurs.</returns>
        /// 
        private Resource ProcessMenuResource(XmlNode node) {

            string resourceId = node.AttributeAsString("resourceId");

            string language = node.AttributeExists("language") ?
                node.AttributeAsString("language") :
                CultureInfo.CurrentCulture.Name;

            XmlNode childNode = node.FirstChild;
            return new MenuResource(resourceId, language, ProcessMenu(childNode));
        }

        /// <summary>
        /// Procesa un node 'menu'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>El menu</returns>
        /// 
        private Menu ProcessMenu(XmlNode node) {

            string title = node.AttributeAsString("title");

            List<Item> items = new List<Item>();
            foreach (XmlNode childNode in node.ChildNodes) { 
                switch (childNode.Name) {
                    case "menuItem":
                        items.Add(ProcessMenuItem(childNode));
                        break;

                    case "commandItem":
                        items.Add(ProcessCommandItem(childNode));
                        break;

                    case "exitItem":
                        items.Add(ProcessExitItem(childNode));
                        break;
                }
            }

            return new Menu(title, items);
        }

        /// <summary>
        /// Procesa un node 'menuItem'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'MenuItem</returns>
        /// 
        private MenuItem ProcessMenuItem(XmlNode node) {

            string title = node.AttributeAsString("title");
            string menuId = node.AttributeExists("id") ?
                node.AttributeAsString("id") : "0";

            XmlNode childNode = node.FirstChild;
            Menu subMenu = childNode == null ?
                null :
                ProcessMenu(childNode);

            return new MenuItem(menuId, title, subMenu);
        }

        /// <summary>
        /// Procesa un node 'commandItem'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'CommandItem'</returns>
        /// 
        private CommandItem ProcessCommandItem(XmlNode node) {

            string title = node.AttributeAsString("title");
            string menuId = node.AttributeAsString("id");

            return new CommandItem(menuId, title);
        }

        /// <summary>
        /// Procesa un node 'exitItem'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'ExitItem'</returns>
        /// 
        private ExitItem ProcessExitItem(XmlNode node) {

            string title = node.AttributeAsString("title");

            return new ExitItem(title);
        }

        /// <summary>
        /// Procesa un node 'fontResource'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'FontResource'</returns>
        /// 
        private FontResource ProcessFontResource(XmlNode node) {

            string resourceId = node.AttributeAsString("resourceId");

            string language = node.AttributeExists("language") ?
                node.AttributeAsString("language") :
                CultureInfo.CurrentCulture.Name;

            XmlNode childNode = node.FirstChild;
            return new FontResource(resourceId, null, ProcessFont(childNode));
        }

        /// <summary>
        /// Procesa un node 'font'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'Font'</returns>
        /// 
        private Font ProcessFont(XmlNode node) {

            string name = node.AttributeAsString("name");
            int height = node.AttributeAsInteger("height");
            int ascent = node.AttributeAsInteger("ascent");
            int descent = node.AttributeAsInteger("descent");

            List<FontChar> chars = new List<FontChar>();
            foreach (XmlNode childNode in node.ChildNodes)
                if (childNode.Name == "char")
                    chars.Add(ProcessFontChar(childNode));

            return new Font(name, height, ascent, descent, chars);
        }

        /// <summary>
        /// Procesa un node 'char'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte 'FontChar'</returns>
        /// 
        private FontChar ProcessFontChar(XmlNode node) {

            int code = Int32.Parse(node.Attributes["code"].Value.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
            int advance = node.AttributeAsInteger("advance");

            // Comprova si es un font amb bitmap
            //
            XmlNode bitmapNode = node.SelectSingleNode("bitmap", namespaceManager);
            if (bitmapNode != null) {
                int left = Convert.ToInt32(bitmapNode.Attributes["left"].Value);
                int top = Convert.ToInt32(bitmapNode.Attributes["top"].Value);
                int width = Convert.ToInt32(bitmapNode.Attributes["width"].Value);
                int height = Convert.ToInt32(bitmapNode.Attributes["height"].Value);

                List<byte> bitmap = new List<byte>();
                foreach (XmlNode scanLineNode in bitmapNode.SelectNodes("scanLine", namespaceManager)) {
                    string scanLine = scanLineNode.InnerText;
                    if (String.IsNullOrEmpty(scanLine))
                        bitmap.Add(0);
                    else {
                        byte b = 0;
                        byte mask = 0x80;
                        for (int i = 0; i < scanLine.Length; i++) {
                            if (scanLine[i] != '.')
                                b |= (byte)(mask >> (i & 0x07));
                            if (((i % 8) == 7) || (i == scanLine.Length - 1)) {
                                bitmap.Add(b);
                                b = 0;
                            }
                        }
                    }
                }

                return new FontChar(code, left, top, width, height, advance, bitmap.ToArray());
            }

            // Comprova si es un fomt amb glyh
            //
            XmlNode glyphNode = node.SelectSingleNode("glyph", namespaceManager);
            if (glyphNode != null) {

                return null;
            }

            // Es un font sense imatge
            //
            return new FontChar(code, 0, 0, 0, 0, advance, null);
        }

        private FontTableResource ProcessFontTableResource(XmlNode node) {

            string resourceId = node.AttributeAsString("resourceId");
            XmlNode fontTableNode = node.SelectSingleNode("fontTable", namespaceManager);

            return new FontTableResource(resourceId, null, ProcessFontTable(fontTableNode));
        }

        private FontTable ProcessFontTable(XmlNode node) {

            List<FontTableItem> items = new List<FontTableItem>();
            foreach (XmlNode childNode in node.SelectNodes("font", namespaceManager))
                items.Add(ProcessFontTableItem(childNode));
                
            return new FontTable(items);
        }

        private FontTableItem ProcessFontTableItem(XmlNode fontTableItemNode) {

            int fontId = Convert.ToInt32(fontTableItemNode.Attributes["fontId"].Value);
            string fontName = fontTableItemNode.Attributes["fontName"].Value;

            return new FontTableItem(fontId, fontName);
        }

        /// <summary>
        /// Procesa un node 'formResource'
        /// </summary>
        /// <param name="formResourceNode">El node a procesar.</param>
        /// <returns>L'objecte 'FormResource'</returns>
        /// 
        private FormResource ProcessFormResource(XmlNode formResourceNode) {

            return null;
        }

        /// <summary>
        /// Procesa un node 'bitmapResource'
        /// </summary>
        /// <param name="node">El node a procesar.</param>
        /// <returns>L'objecte BitmapResource.</returns>
        /// 
        private BitmapResource ProcessBitmapResource(XmlNode node) {

            string resourceId = node.AttributeAsString("resourceId");

            string language = node.AttributeExists("language") ?
                node.AttributeAsString("language") :
                CultureInfo.CurrentCulture.Name;

            XmlNode childNode = node.SelectSingleNode("bitmap", namespaceManager);
            return new BitmapResource(resourceId, language, ProcessBitmapNode(childNode));
        }

        /// <summary>
        /// Procesa un node 'bitmap'
        /// </summary>
        /// <param name="bitmapNode">El node a procesar.</param>
        /// <returns>L'objecte 'Bitmap'</returns>
        /// 
        private Bitmap ProcessBitmapNode(XmlNode bitmapNode) {

            string source = bitmapNode.Attributes["source"].Value;
            BitmapFormat format = (BitmapFormat) Enum.Parse(typeof(BitmapFormat), bitmapNode.Attributes["format"].Value, true);

            return new Bitmap(source, format);
        }
    }
}
