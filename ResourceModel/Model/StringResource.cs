namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.StringResources;

    public sealed class StringResource : Resource {

        private readonly Strings strings;

        public StringResource(string resourceId, string language, Strings strings): 
            base(resourceId, language) {

            if (strings == null)
                throw new ArgumentNullException("strings");

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
