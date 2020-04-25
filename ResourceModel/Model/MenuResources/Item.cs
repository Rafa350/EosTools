namespace EosTools.v1.ResourceModel.Model.MenuResources {

    using System;
    
    public abstract class Item: IVisitable {

        private readonly string title;

        /// <summary>
        /// Constructor de l'bjecte.
        /// </summary>
        /// <param name="title">Titol.</param>
        /// 
        protected Item(string title) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            this.title = title;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Obte el titol del item.
        /// </summary>
        /// 
        public string Title => title;
    }
}
