namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.StringResources;

    public sealed class StringResource : Resource {

        private readonly Strings strings;

        public StringResource(string id, string language, Strings strings): 
            base(id, language) {

            if (strings == null)
                throw new ArgumentNullException(nameof(strings));

            this.strings = strings;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public Strings Strings {
            get {
                return strings;
            }
        }
    }
}
