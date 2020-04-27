namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Descriptor d'un recurs de menu.
    /// </summary>
    /// 
    public sealed class Menu: IVisitable {

        private readonly string title;
        private readonly IEnumerable<Item> items;

        /// <summary>
        /// Contructor del menu.
        /// </summary>
        /// <param name="title">Titol del menu.</param>
        /// <param name="items">Coleccio d'items del menu.</param>
        /// 
        public Menu(string title, IEnumerable<Item> items) {

            if (String.IsNullOrEmpty("title"))
                throw new ArgumentNullException(nameof(title));

            this.title = title;
            this.items = items;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el titol.
        /// </summary>
        /// 
        public string Title => title;

        /// <summary>
        /// Obte els items.
        /// </summary>
        /// 
        public IEnumerable<Item> Items => items;
    }
}
