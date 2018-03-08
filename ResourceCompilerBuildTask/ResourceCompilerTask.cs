namespace EosTools.v1.ResourceCompiler.MSBuild {

    using System;
    using System.IO;
    using EosTools.v1.ResourceCompiler.Compiler;
    using EosTools.v1.ResourceModel.IO;
    using EosTools.v1.ResourceModel.Model;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class ResourceCompilerTask: Task {

        private string outputPath;
        private string parameters;
        private ITaskItem[] source;

        public override bool Execute() {

            foreach (ITaskItem item in source) {

                string outPath = String.IsNullOrEmpty(outputPath) ?
                    Path.GetDirectoryName(item.ItemSpec) :
                    outputPath;

                Log.LogMessage("EOS Resource Compiler");
                Log.LogMessage(String.Format("        Input file : '{0}'", item.ItemSpec));
                Log.LogMessage(String.Format("        Output path: '{0}'", outputPath));
                Log.LogMessage(String.Format("        Parameters : '{0}'", parameters));

                CompilerParameters compilerParameters = new CompilerParameters();
                foreach (string parameter in parameters.Split(';'))
                    compilerParameters.Add(parameter);

                try {
                    ResourcePool resources;
                    using (Stream stream = new FileStream(
                         item.ItemSpec, FileMode.Open, FileAccess.Read, FileShare.None)) {
                        IResourceReader reader = new ResourceReader(stream);
                        resources = reader.Read();
                    }

                    IResourceCompiler compiler = new ResourceCompiler();
                    compiler.Compile(resources, outPath, compilerParameters);
                }
                catch (Exception ex) {

                    Exception e = ex;
                    while (e != null) {
                        Log.LogMessage(e.Message);
                        e = e.InnerException;
                    }

                    throw;
                }
            }

            return true;
        }

        [Required]
        public ITaskItem[] Source {
            get {
                return source;
            }
            set {
                source = value;
            }
        }

        public string OutputPath {
            get {
                return outputPath;
            }
            set {
                outputPath = value;
            }
        }

        public string Parameters {
            get {
                return parameters;
            }
            set {
                parameters = value;
            }
        }
    }
}
