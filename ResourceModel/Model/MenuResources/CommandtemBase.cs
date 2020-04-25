namespace EosTools.v1.ResourceModel.Model.MenuResources {

    public abstract class CommandItemBase: Item {

        private readonly string menuId;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="menuId">Identificador del item.</param>
        /// <param name="title">Titol del item.</param>
        /// 
        protected CommandItemBase(string menuId, string title) 
            : base(title) {

            this.menuId = menuId;
        }

        /// <summary>
        /// Obte el identificador del item.
        /// </summary>
        /// 
        public string MenuId => menuId;
    }
}
