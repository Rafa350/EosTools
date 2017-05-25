namespace EosTools.v1.ResourceModel.Model.MenuResources {

    public sealed class ExitItem: Item {

        public ExitItem(string title)
            : base(title) {
        }
        
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
