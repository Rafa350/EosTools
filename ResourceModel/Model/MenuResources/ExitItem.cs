namespace EosTools.v1.ResourceModel.Model.MenuResources {

    public sealed class ExitItem: Item {

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="title">El titol.</param>
        /// 
        public ExitItem(string title)
            : base(title) {
        }
        
        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
