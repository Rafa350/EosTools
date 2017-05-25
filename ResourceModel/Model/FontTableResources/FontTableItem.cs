namespace EosTools.v1.ResourceModel.Model.FontTableResources {

    using System;
    using System.Collections.Generic;
    
    public sealed class FontTableItem {

        private int fontId;
        private string fontName;

        public FontTableItem(int fontId, string fontName) {

            if (String.IsNullOrEmpty(fontName))
                throw new ArgumentNullException("fontName");

            this.fontId = fontId;
            this.fontName = fontName;
        }

        public int FontId {
            get {
                return fontId;
            }
        }

        public string FontName {
            get {
                return fontName;
            }
        }
    }
}
