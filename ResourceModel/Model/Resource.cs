namespace EosTools.v1.ResourceModel.Model {

    /// <summary>
    /// Clase abstracta base per a tots els tipus de recursos.
    /// </summary>
    /// 
    public abstract class Resource: IVisitable {

        private readonly string resourceId;
        private readonly string languaje;

        /// <summary>
        /// Contructor del recurs.
        /// </summary>
        /// <param name="resourceId">Identificador del recurs.</param>
        /// <param name="languaje">Codi del llenguatge del recurs.</param>
        /// 
        public Resource(string resourceId, string languaje) {

            this.resourceId = resourceId;
            this.languaje = languaje;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Obte el identificador del recurs.
        /// </summary>
        /// 
        public string ResourceId {
            get {
                return resourceId;
            }
        }

        /// <summary>
        /// Obte el llenguatge del recurs.
        /// </summary>
        /// 
        public string Languaje {
            get {
                return languaje;
            }
        }
    }
}
