namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Descriptor d'un recurs de menu.
    /// </summary>
    /// 
    public sealed class Menu: IVisitable {

        private readonly string title;
        private readonly List<Item> items = new List<Item>();

        /// <summary>
        /// Contructor del menu.
        /// </summary>
        /// <param name="title">Titol de menu.</param>
        /// 
        public Menu(string title) {

            if (String.IsNullOrEmpty("title"))
                throw new ArgumentNullException("title");

            this.title = title;
        }

        /// <summary>
        /// Contructor del menu.
        /// </summary>
        /// <param name="title">Titol del menu.</param>
        /// <param name="items">Llista d'items del menu.</param>
        /// 
        public Menu(string title, params Item[] items) {

            if (String.IsNullOrEmpty("title"))
                throw new ArgumentNullException("title");

            this.title = title;
            this.items.AddRange(items);
        }

        /// <summary>
        /// Contructor del menu.
        /// </summary>
        /// <param name="title">Titol del menu.</param>
        /// <param name="items">Llista d'items del menu.</param>
        /// 
        public Menu(string title, IEnumerable<Item> items) {

            if (String.IsNullOrEmpty("title"))
                throw new ArgumentNullException("title");

            this.title = title;
            this.items.AddRange(items);
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el titol.
        /// </summary>
        /// 
        public string Title {
            get {
                return title;
            }
        }

        /// <summary>
        /// Obte els subitems.
        /// </summary>
        /// 
        public IReadOnlyCollection<Item> Items {
            get {
                return items;
            }
        }
    }
}
