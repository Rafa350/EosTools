namespace EosTools.v1.ResourceModel.Model.TextDataResources {

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class Strings: IVisitable {

        private List<string> items = new List<string>();

        public Strings(params string[] items) {

            this.items.AddRange(items);
        }

        public Strings(IEnumerable<string> items) {

            this.items.AddRange(items);
        }

        public void AcceptVisitor(IVisitor visitor) {

            throw new NotImplementedException();
        }
    }
}
