namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.FontResources;
    using EosTools.v1.ResourceModel.Model.FormResources;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    using EosTools.v1.ResourceModel.Model.StringResources;

    public abstract class DefaultVisitor: IVisitor {

        public virtual void Visit(ResourcePool resources) {

            foreach (Resource resource in resources.Resources)
                resource.AcceptVisitor(this);
        }

        public virtual void Visit(MenuResource resource) {

            resource.Menu.AcceptVisitor(this);
        }

        public virtual void Visit(StringResource resource) {

            resource.Strings.AcceptVisitor(this);
        }

        public virtual void Visit(FontResource resource) {
            
            throw new NotImplementedException();
        }

        public virtual void Visit(FormResource resource) {

            throw new NotImplementedException();
        }

        public virtual void Visit(Form form) {
        }

        public virtual void Visit(Strings strings) {
        }

        public virtual void Visit(Menu menu) {

            foreach (Item item in menu.Items)
                item.AcceptVisitor(this);
        }

        public virtual void Visit(CommandItem item) {
        }

        public virtual void Visit(ExitItem item) {
        }

        public virtual void Visit(MenuItem item) {

            if (item.SubMenu != null)
                item.SubMenu.AcceptVisitor(this);
        }

        public void Visit(Font font) {

        }

        public void Visit(FontChar fontChar) {
        }
    }
}
