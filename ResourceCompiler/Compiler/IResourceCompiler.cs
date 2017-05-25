namespace EosTools.v1.ResourceCompiler.Compiler {

    using EosTools.v1.ResourceModel.Model;

    public interface IResourceCompiler {

        void Compile(ResourcePool resources, string outputFolder, CompilerParameters parameters);
    }
}
