namespace EosTools.v1.ResourceCompiler.Compiler {

    using System;
    using EosTools.v1.ResourceCompiler.Compiler.FontCompiler;
    using EosTools.v1.ResourceCompiler.Compiler.MenuCompiler;
    using EosTools.v1.ResourceCompiler.Compiler.BitmapCompiler;
    using EosTools.v1.ResourceModel.Model;

    public sealed class ResourceCompiler: IResourceCompiler {

        private sealed class ResourceVisitor: DefaultVisitor {

            private readonly string outputFolder;
            private readonly CompilerParameters parameters;

            public ResourceVisitor(string outputFolder, CompilerParameters parameters) {

                this.outputFolder = outputFolder;
                this.parameters = parameters;
            }

            public override void Visit(FontResource resource) {

                FontResourceCompiler compiler = new FontResourceCompiler();
                compiler.Compile(resource, outputFolder, parameters);
            }

            public override void Visit(BitmapResource resource) {

                BitmapResourceCompiler compiler = new BitmapResourceCompiler();
                compiler.Compile(resource, outputFolder, parameters);
            }

            public override void Visit(MenuResource resource) {

                MenuResourceCompiler compiler = new MenuResourceCompiler();
                compiler.Compile(resource, outputFolder, parameters);
            }
        }

        public void Compile(ResourcePool resources, string outputFolder, CompilerParameters parameters) {

            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            if (String.IsNullOrEmpty(outputFolder))
                throw new ArgumentNullException(nameof(outputFolder));

            ResourceVisitor visitor = new ResourceVisitor(outputFolder, parameters);
            visitor.Visit(resources);
        }
    }
}
