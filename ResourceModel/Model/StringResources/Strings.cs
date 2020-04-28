namespace EosTools.v1.ResourceModel.Model.StringResources {

    using System.Collections.Generic;

    public sealed class Strings: IVisitable {

        private IEnumerable<StringsItem> items;

        public Strings(IEnumerable<StringsItem> items) {

            this.items = items;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public IEnumerable<StringsItem> Items => items;
    }
}
