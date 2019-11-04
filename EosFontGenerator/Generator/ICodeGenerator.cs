namespace EosTools.v1.FontGeneratorApp.Generator {

    using System.IO;
    using EosTools.v1.FontGeneratorApp.Model;
    
    public interface ICodeGenerator {

        void GenerateFontSource(TextWriter writer, FontDescriptor fontDescriptor);
    }
}
