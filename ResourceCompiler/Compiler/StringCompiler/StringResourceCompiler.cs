namespace EosTools.v1.ResourceCompiler.Compiler.StringCompiler {

    using EosTools.v1.ResourceModel.Model;
    using System;
    using System.IO;
    using System.Reflection;

    public sealed class StringResourceCompiler {

        private readonly Version version;

        public StringResourceCompiler() {

            version = Assembly.GetExecutingAssembly().GetName().Version;
        }

        public void Compile(StringResource resource, string outputPath, CompilerParameters parameters) {

            if (resource == null)
                throw new ArgumentNullException("resource");

        }

        private void GenerateHeader(StringResource resource, TextWriter writer) {

        }

        private void GenerateCode(StringResource resource, TextWriter writer) {

        }
    }
}