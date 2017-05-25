namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    
    public abstract class Item: IVisitable {

        private string title;
        private string displayFormat;

        public Item(string title) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.title = title;
        }

        public Item(string title, string displayFormat) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.title = title;
            this.displayFormat = displayFormat;
        }

        public abstract void AcceptVisitor(IVisitor visitor);

        public string Title {
            get {
                return title;
            }
        }

        public string DisplayFormat {
            get {
                return displayFormat;
            }
        }
    }
}
