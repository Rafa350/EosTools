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

        private Resource ProcessMenuResource(XmlNode resourceNode) {

            string resourceId = resourceNode.Attributes["resourceId"].Value;

            string language = CultureInfo.CurrentCulture.Name;

            if (resourceNode.Attributes["language"] != null)
                language = resourceNode.Attributes["language"].Value;

            XmlNode menuNode = resourceNode.SelectSingleNode("menu");
            return new MenuResource(resourceId, language, ProcessMenu(menuNode));
        }

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

        private MenuItem ProcessMenuItem(XmlNode menuItemNode) {

            string title = menuItemNode.Attributes["title"].Value;
            string displayFormat = null;
            if (menuItemNode.Attributes["displayFormat"] != null) {
                displayFormat = menuItemNode.Attributes["displayFormat"].Value;
                if (displayFormat == null)
                    displayFormat = String.Empty;
            }

            Menu menu = null;
            XmlNode menuNode = menuItemNode.SelectSingleNode("menu");
            if (menuNode != null)
                menu = ProcessMenu(menuNode);

            return new MenuItem(title, displayFormat, menu);
        }

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

        private ExitItem ProcessExitItem(XmlNode commandItemNode) {

            string title = commandItemNode.Attributes["title"].Value;

            return new ExitItem(title);
        }

        private FontResource ProcessFontResource(XmlNode resourceNode) {

            string language = CultureInfo.CurrentCulture.Name;

            string resourceId = resourceNode.Attributes["resourceId"].Value;
            
            if (resourceNode.Attributes["language"] != null)
                language = resourceNode.Attributes["language"].Value;

            XmlNode fontNode = resourceNode.SelectSingleNode("font");

            return new FontResource(resourceId, null, ProcessFont(fontNode));
        }

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

        private FontChar ProcessFontChar(XmlNode charNode) {

            int code = Int32.Parse(charNode.Attributes["code"].Value.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
            int left = Convert.ToInt32(charNode.Attributes["left"].Value);
            int top = Convert.ToInt32(charNode.Attributes["top"].Value);
            int width = Convert.ToInt32(charNode.Attributes["width"].Value);
            int height = Convert.ToInt32(charNode.Attributes["height"].Value);
            int advance = Convert.ToInt32(charNode.Attributes["advance"].Value);

            List<byte> bitmap = new List<byte>();
            foreach (XmlNode scanLineNode in charNode.SelectNodes("bitmap/scanLine")) {
                string scanLine = scanLineNode.InnerText;
                if (String.IsNullOrEmpty(scanLine))
                    bitmap.Add(0);
                else {
                    byte b = 0;
                    byte mask = 0x80;
                    for (int i = 0; i < scanLine.Length; i++) {
                        if (scanLine[i] == '#')
                            b |= (byte) (mask >> (i & 0x07));
                        if (((i % 8) == 7) || (i == scanLine.Length - 1)) {
                            bitmap.Add(b);
                            b = 0;
                        }
                    }
                }
            }

            return new FontChar(code, left, top, width, height, advance, bitmap.ToArray());
        }

        private FontTableResource ProcessFontTableResource(XmlNode resourceNode) {

            string resourceId = resourceNode.Attributes["resourceId"].Value;
            XmlNode fontTableNode = resourceNode.SelectSingleNode("fontTable");

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

        private FormResource ProcessFormResource(XmlNode resourceNode) {

            return null;
        }

        private Resource ProcessBitmapResource(XmlNode bitmapNode) {

            return null;
        }
    }
}
