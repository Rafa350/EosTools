namespace EosTools.v1.ResourceModel.Model {

    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Coleccio de recursos.
    /// </summary>
    public sealed class ResourcePool: IVisitable {

        private readonly List<Resource> resources = new List<Resource>();

        /// <summary>
        /// Contructor de la coleccio de recursos.
        /// </summary>
        /// <param name="resources">Llista de recursos a afeigir a la coleccio.</param>
        /// 
        public ResourcePool(IEnumerable<Resource> resources) {

            if (resources == null)
                throw new ArgumentNullException("resources");

            this.resources.AddRange(resources);
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">Visitador a acceptar.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Enumera els recursos de la coleccio.
        /// </summary>
        /// 
        public IEnumerable<Resource> Resources {
            get {
                return resources;
            }
        }
    }
}
