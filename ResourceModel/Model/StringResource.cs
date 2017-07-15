using System;

namespace EosTools.v1.ResourceModel.Model {

    public sealed class StringResource : Resource {

        public StringResource(string resourceId, string language): 
            base(resourceId, language) {

        }

        public override void AcceptVisitor(IVisitor visitor) {
            throw new NotImplementedException();
        }
    }
}
