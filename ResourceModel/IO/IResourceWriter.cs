namespace EosTools.v1.ResourceModel.IO {

    using EosTools.v1.ResourceModel.Model;

    public interface IResourceWriter {

        void Write(ResourcePool resources);
    }
}
