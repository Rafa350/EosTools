﻿namespace EosTools.v1.ResourceModel.Model.MenuResources {

    public class CommandItem: CommandItemBase {

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="menuId">Identificador del item.</param>
        /// <param name="title">Titol del item.</param>
        /// 
        public CommandItem(string menuId, string title) 
            : base(menuId, title) {
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
