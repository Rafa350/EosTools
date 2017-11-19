namespace EosTools.v1.ResourceModel.Model {

    using EosTools.v1.ResourceModel.Model.FontResources;
    using EosTools.v1.ResourceModel.Model.FormResources;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    using EosTools.v1.ResourceModel.Model.StringResources;

    public interface IVisitor {

        void Visit(ResourcePool resources);
        void Visit(MenuResource resource);
        void Visit(FontResource resource);
        void Visit(BitmapResource resource);
        void Visit(FormResource resource);
        void Visit(StringResource resource);

        void Visit(Font font);
        void Visit(FontChar fontChar);

        void Visit(Form form);

        void Visit(Menu menu);
        void Visit(CommandItem item);
        void Visit(MenuItem item);
        void Visit(ExitItem item);
    }
}
