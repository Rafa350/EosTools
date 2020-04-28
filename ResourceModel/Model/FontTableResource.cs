namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.FontTableResources;
    
    public sealed class FontTableResource: Resource {

        private readonly FontTable fontTable;

        public FontTableResource(string id, string languaje, FontTable fontTable)
            : base(id, languaje) {

            if (fontTable == null)
                throw new ArgumentNullException(nameof(fontTable));

            this.fontTable = fontTable;
        }

        public override void AcceptVisitor(IVisitor visitor) {
        }

        public FontTable FontTable {
            get {
                return fontTable;
            }
        }
    }
}
