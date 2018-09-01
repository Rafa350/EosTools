namespace EosTools.v1.ResourceModel.Model {

    using EosTools.v1.ResourceModel.Model.BitmapResources;
    using EosTools.v1.ResourceModel.Model.FontResources;
    using EosTools.v1.ResourceModel.Model.FormResources;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    using EosTools.v1.ResourceModel.Model.StringResources;
    using System;

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

        /// <summary>
        /// Visita un 'BitmapResource'
        /// </summary>
        /// <param name="resource">L'objecte a visitar.</param>
        /// 
        public virtual void Visit(BitmapResource resource) {

            resource.Bitmap.AcceptVisitor(this);
        }

        /// <summary>
        /// Visita un 'Bitmap'
        /// </summary>
        /// <param name="bitmap">L'objecte a visitar.</param>
        /// 
        public virtual void Visit(Bitmap bitmap) {
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
