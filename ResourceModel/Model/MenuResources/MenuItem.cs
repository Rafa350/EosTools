namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    
    public sealed class MenuItem: CommandItemBase {

        private readonly Menu subMenu;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="menuId">El identificador del item.</param>
        /// <param name="title">El titol.</param>
        /// <param name="subMenu">El submenu.</param>
        /// 
        public MenuItem(string menuId, string title, Menu subMenu) 
            : base(menuId, title) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            this.subMenu = subMenu;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el submenu.
        /// </summary>
        /// 
        public Menu SubMenu => subMenu;
    }
}
