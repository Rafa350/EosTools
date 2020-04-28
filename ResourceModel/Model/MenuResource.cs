namespace EosTools.v1.ResourceModel.Model {

    using System;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    
    /// <summary>
    /// Recurs que representa un menu.
    /// </summary>
    /// 
    public sealed class MenuResource: Resource {

        private readonly Menu menu;

        /// <summary>
        /// Contructor de recurs.
        /// </summary>
        /// <param name="id">Identificador del recurs.</param>
        /// <param name="languaje">llenguatge del recurs.</param>
        /// <param name="menu">El menu que conte aquest recurs.</param>
        /// 
        public MenuResource(string id, string languaje, Menu menu) :
            base(id, languaje) {

            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            this.menu = menu;
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
        /// El menu contingut en el recurs.
        /// </summary>
        /// 
        public Menu Menu => menu;
    }
}
