namespace EosTools.v1.ResourceModel.Model.StringResources {

    using System.Collections.Generic;

    public sealed class Strings: IVisitable {

        private List<StringsItem> items = new List<StringsItem>();

        public Strings(params StringsItem[] items) {

            this.items.AddRange(items);
        }

        public Strings(IEnumerable<StringsItem> items) {

            this.items.AddRange(items);
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
