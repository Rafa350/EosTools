namespace EosTools.v1.ResourceModel.Model {

    /// <summary>
    /// Intierficio que cap implementar per poder ser visiats.
    /// </summary>
    public interface IVisitable {

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        void AcceptVisitor(IVisitor visitor);
    }
}
