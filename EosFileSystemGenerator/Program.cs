namespace EosTools.v1.FileSystemGeneratorApp {

    using System;
    using Microsoft.Extensions.CommandLineUtils;

    class Program {

        static int Main(string[] args) {

            var app = new CommandLineApplication();

            app.Name = "EosFileSystemGenerator";
            app.FullName = "";
            app.Description = "EOS embedded file system generator";

            var sourceFolder = app.Argument("<SOURCE_PATH>", "Path for input files");
            var outputFile = app.Argument("<OUTPUT_FILE>", "Output file");
            
            app.HelpOption("-? | -h | --help");
            app.VersionOption("-v | --version", "1.0");

            app.OnExecute(() => { 

                ROMFsGenerator generator = new ROMFsGenerator();
                generator.Generate(sourceFolder.Value, outputFile.Value);

                return 0;
            });

            try {
            
                return app.Execute(args);
            }

            catch (Exception e) {
                
                return 1;
            }
        }
    }
}
