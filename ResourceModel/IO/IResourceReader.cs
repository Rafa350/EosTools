namespace EosTools.v1.ResourceModel.IO {

    using EosTools.v1.ResourceModel.Model;

    public interface IResourceReader {

        ResourcePool Read();
    }
}
