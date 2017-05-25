namespace EosTools.v1.ResourceCompilerApp {

    using System;
    using System.IO;
    using EosTools.v1.ResourceCompiler.Compiler;
    using EosTools.v1.ResourceCompilerApp.CmdLine;
    using EosTools.v1.ResourceModel.IO;
    using EosTools.v1.ResourceModel.Model;

    class Program {
    
        static void Main(string[] args) {
            try {
                CmdLineParser cmdLineParser = new CmdLineParser("EosResourceCompiler v1.0");
                cmdLineParser.Add(new ArgumentDefinition("source", 1, "Archivo de entrada.", true));
                cmdLineParser.Add(new OptionDefinition("O", "Carpeta de salida."));
                cmdLineParser.Add(new OptionDefinition("V", "Muestra informacion detallada."));
                cmdLineParser.Add(new OptionDefinition("P", "Parametro personalizado.", false, true));

                if (args.Length == 0) {
                    Console.WriteLine(cmdLineParser.HelpText);
                    Console.ReadKey(true);
                }

                else {
                    CompilerParameters parameters = new CompilerParameters();

                    string outputFolder = null;
                    string sourceFileName = null;

                    cmdLineParser.Parse(args);
                    foreach (OptionInfo optionInfo in cmdLineParser.Options) {
                        switch (optionInfo.Name) {
                            case "O":
                                outputFolder = optionInfo.Value;
                                break;

                            case "V":
                                parameters.Add("verbose");
                                break;

                            case "P":
                                parameters.Add(optionInfo.Value);
                                break;
                        }
                    }
                    foreach (ArgumentInfo argumentInfo in cmdLineParser.Arguments) {
                        switch (argumentInfo.Name) {
                            case "source":
                                sourceFileName = argumentInfo.Value;
                                break;
                        }
                    }

                    if (String.IsNullOrEmpty(sourceFileName))
                        throw new InvalidOperationException("No se especifico el fichero fuente.");

                    if (outputFolder == null)
                        outputFolder = Path.GetDirectoryName(sourceFileName);

                    ResourcePool resources;
                    using (Stream stream = new FileStream(
                        sourceFileName, FileMode.Open, FileAccess.Read, FileShare.None)) {
                        IResourceReader reader = new ResourceReader(stream);
                        resources = reader.Read();
                    }

                    IResourceCompiler compiler = new ResourceCompiler();
                    compiler.Compile(resources, outputFolder, parameters);
                }
            }

            catch (Exception e) {
                while (e != null) {
                    Console.WriteLine(e.Message);
                    e = e.InnerException;
                }
                Console.ReadKey(true);
            }
        }
    }
}
