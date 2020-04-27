namespace EosTools.v1.ResourceModel.Model.FontTableResources {

    using System;
    using System.Collections.Generic;
    
    public sealed class FontTable {

        private readonly IEnumerable<FontTableItem> items;

        public FontTable(IEnumerable<FontTableItem> items) {

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            this.items = items;
        }

        public IEnumerable<FontTableItem> Items => items;
    }
}
