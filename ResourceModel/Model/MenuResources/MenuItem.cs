namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    
    public sealed class MenuItem: Item {

        private readonly Menu subMenu;

        public MenuItem(string title, Menu subMenu) 
            : base(title) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.subMenu = subMenu;
        }

        public MenuItem(string title, string displayFormat, Menu subMenu)
            : base(title, displayFormat) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.subMenu = subMenu;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public Menu SubMenu {
            get { return subMenu; }
        }
    }
}
