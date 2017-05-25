namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.FormResources;
    
    public sealed class FormResource: Resource {

        private readonly Form form;

        public FormResource(string resourceId, string language, Form form) : 
            base(resourceId, language) {

            if (form == null)
                throw new ArgumentNullException("form");

            this.form = form;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public Form Form {
            get {
                return form;
            }
        }
    }
}

