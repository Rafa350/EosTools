namespace EosTools.v1.ResourceModel.Model.FontTableResources {

    using System;
    using System.Collections.Generic;
    
    public sealed class FontTable {

        private readonly List<FontTableItem> items = new List<FontTableItem>();

        public FontTable(IEnumerable<FontTableItem> items) {

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            this.items.AddRange(items);
        }

        public IEnumerable<FontTableItem> Items {
            get { 
                return items; 
            }
        }
    }
}
