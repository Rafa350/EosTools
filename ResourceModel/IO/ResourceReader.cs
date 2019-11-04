namespace EosTools.v1.ResourceModel.IO {

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.FontResources;
    using EosTools.v1.ResourceModel.Model.FontTableResources;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    using EosTools.v1.ResourceModel.Model.BitmapResources;

    public class ResourceReader: IResourceReader {

        private readonly Stream stream;

        public ResourceReader(Stream stream) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            this.stream = stream;
        }

        public ResourcePool Read() {

            XmlDocument document = new XmlDocument();
            document.Load(stream);

            List<Resource> resources = new List<Resource>();
            foreach (XmlNode resourceNode in document.SelectSingleNode("resources")) {
                switch (resourceNode.Name) {
                    case "menuResource":
                        resources.Add(ProcessMenuResource(resourceNode));
                        break;

                    case "fontResource":
                        resources.Add(ProcessFontResource(resourceNode));
                        break;

                    case "fontTableResource":
                        resources.Add(ProcessFontTableResource(resourceNode));
                        break;

                    case "formResource":
                        resources.Add(ProcessFormResource(resourceNode));
                        break;

                    case "bitmapResource":
                        resources.Add(ProcessBitmapResource(resourceNode));
                        break;
                }
            }

            return new ResourcePool(resources);
        }

        /// <summary>
        /// Procesa un node 'menuResource'
        /// </summary>
        /// <param name="menuResourceNode">El node a procesar.</param>
        /// <returns>El recurs.</returns>
        /// 
        private Resource ProcessMenuResource(XmlNode menuResourceNode) {

            string resourceId = menuResourceNode.Attributes["resourceId"].Value;

            string language = CultureInfo.CurrentCulture.Name;

            if (menuResourceNode.Attributes["language"] != null)
                language = menuResourceNode.Attributes["language"].Value;

            XmlNode menuNode = menuResourceNode.SelectSingleNode("menu");
            return new MenuResource(resourceId, language, ProcessMenu(menuNode));
        }

        /// <summary>
        /// Procesa un node 'menu'
        /// </summary>
        /// <param name="menuNode">El node a procesar.</param>
        /// <returns>El menu</returns>
        /// 
        private Menu ProcessMenu(XmlNode menuNode) {

            string title = menuNode.Attributes["title"].Value;

            List<Item> items = new List<Item>();
            foreach (XmlNode itemNode in menuNode.SelectNodes("menuItem|commandItem|exitItem")) {
                switch (itemNode.Name) {
                    case "menuItem":
                        items.Add(ProcessMenuItem(itemNode));
                        break;

                    case "commandItem":
                        items.Add(ProcessCommandItem(itemNode));
                        break;

                    case "exitItem":
                        items.Add(ProcessExitItem(itemNode));
                        break;
                }
            }

            return new Menu(title, items);
        }

        /// <summary>
        /// Procesa un node 'menuItem'
        /// </summary>
        /// <param name="menuItemNode">El node a procesar.</param>
        /// <returns>L'objecte 'MenuItem</returns>
        /// 
        private MenuItem ProcessMenuItem(XmlNode menuItemNode) {

            string title = menuItemNode.Attributes["title"].Value;

            string displayFormat = null;
            if (menuItemNode.Attributes["displayFormat"] != null) {
                displayFormat = menuItemNode.Attributes["displayFormat"].Value;
                if (displayFormat == null)
                    displayFormat = String.Empty;
            }

            Menu subMenu = null;
            XmlNode menuNode = menuItemNode.SelectSingleNode("menu");
            if (menuNode != null)
                subMenu = ProcessMenu(menuNode);

            return new MenuItem(title, displayFormat, subMenu);
        }

        /// <summary>
        /// Procesa un node 'commandItem'
        /// </summary>
        /// <param name="commandItemNode">El node a procesar.</param>
        /// <returns>L'objecte 'CommandItem'</returns>
        /// 
        private CommandItem ProcessCommandItem(XmlNode commandItemNode) {

            string title = commandItemNode.Attributes["title"].Value;
            string displayFormat = null;
            if (commandItemNode.Attributes["displayFormat"] != null) {
                displayFormat = commandItemNode.Attributes["displayFormat"].Value;
                if (displayFormat == null)
                    displayFormat = String.Empty;
            }
            string command = commandItemNode.Attributes["command"].Value;

            return new CommandItem(title, displayFormat, command);
        }

        /// <summary>
        /// Procesa un node 'exitItem'
        /// </summary>
        /// <param name="exitItemNode">El node a procesar.</param>
        /// <returns>L'objecte 'ExitItem'</returns>
        /// 
        private ExitItem ProcessExitItem(XmlNode exitItemNode) {

            string title = exitItemNode.Attributes["title"].Value;

            return new ExitItem(title);
        }

        /// <summary>
        /// Procesa un node 'fontResource'
        /// </summary>
        /// <param name="fontResourceNode">El node a procesar.</param>
        /// <returns>L'objecte 'FontResource'</returns>
        /// 
        private FontResource ProcessFontResource(XmlNode fontResourceNode) {

            string resourceId = fontResourceNode.Attributes["resourceId"].Value;

            string language = CultureInfo.CurrentCulture.Name;
            if (fontResourceNode.Attributes["language"] != null)
                language = fontResourceNode.Attributes["language"].Value;

            XmlNode fontNode = fontResourceNode.SelectSingleNode("font");

            return new FontResource(resourceId, null, ProcessFont(fontNode));
        }

        /// <summary>
        /// Procesa un node 'font'
        /// </summary>
        /// <param name="fontNode">El node a procesar.</param>
        /// <returns>L'objecte 'Font'</returns>
        /// 
        private Font ProcessFont(XmlNode fontNode) {

            string name = fontNode.Attributes["name"].Value;
            int height = Convert.ToInt32(fontNode.Attributes["height"].Value);
            int ascent = Convert.ToInt32(fontNode.Attributes["ascent"].Value);
            int descent = Convert.ToInt32(fontNode.Attributes["descent"].Value);

            List<FontChar> chars = new List<FontChar>();
            foreach (XmlNode charNode in fontNode.SelectNodes("char"))
                chars.Add(ProcessFontChar(charNode));

            return new Font(name, height, ascent, descent, chars);
        }

        /// <summary>
        /// Procesa un node 'char'
        /// </summary>
        /// <param name="charNode">El node a procesar.</param>
        /// <returns>L'objecte 'FontChar'</returns>
        /// 
        private FontChar ProcessFontChar(XmlNode charNode) {

            int code = Int32.Parse(charNode.Attributes["code"].Value.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
            int advance = Convert.ToInt32(charNode.Attributes["advance"].Value);

            // Comprova si es un fomt amb bitmap
            //
            XmlNode bitmapNode = charNode.SelectSingleNode("bitmap");
            if (bitmapNode != null) {
                int left = Convert.ToInt32(bitmapNode.Attributes["left"].Value);
                int top = Convert.ToInt32(bitmapNode.Attributes["top"].Value);
                int width = Convert.ToInt32(bitmapNode.Attributes["width"].Value);
                int height = Convert.ToInt32(bitmapNode.Attributes["height"].Value);

                List<byte> bitmap = new List<byte>();
                foreach (XmlNode scanLineNode in bitmapNode.SelectNodes("scanLine")) {
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
            XmlNode glyphNode = charNode.SelectSingleNode("glyph");
            if (glyphNode != null) {

                return null;
            }

            // Es un font sense imatge
            //
            return new FontChar(code, 0, 0, 0, 0, advance, null);
        }

        private FontTableResource ProcessFontTableResource(XmlNode fontTableResourceNode) {

            string resourceId = fontTableResourceNode.Attributes["resourceId"].Value;
            XmlNode fontTableNode = fontTableResourceNode.SelectSingleNode("fontTable");

            return new FontTableResource(resourceId, null, ProcessFontTable(fontTableNode));
        }

        private FontTable ProcessFontTable(XmlNode fontTableNode) {

            List<FontTableItem> items = new List<FontTableItem>();
            foreach (XmlNode fontTableItemNode in fontTableNode.SelectNodes("font"))
                items.Add(ProcessFontTableItem(fontTableItemNode));
                
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
        /// <param name="bitmapResourceNode">El node a procesar.</param>
        /// <returns>L'objecte BitmapResource.</returns>
        /// 
        private BitmapResource ProcessBitmapResource(XmlNode bitmapResourceNode) {

            string resourceId = bitmapResourceNode.Attributes["resourceId"].Value;

            string language = CultureInfo.CurrentCulture.Name;
            if (bitmapResourceNode.Attributes["language"] != null)
                language = bitmapResourceNode.Attributes["language"].Value;

            XmlNode bitmapNode = bitmapResourceNode.SelectSingleNode("bitmap");
            return new BitmapResource(resourceId, language, ProcessBitmapNode(bitmapNode));
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
