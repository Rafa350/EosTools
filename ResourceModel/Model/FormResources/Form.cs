namespace EosTools.v1.ResourceModel.Model.FormResources {

    using System;
    
    public sealed class Form: IVisitable {

        public Form() {
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
