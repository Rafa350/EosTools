namespace Media.PicFontGenerator.Generator {

    using System.IO;
    using Media.PicFontGenerator.Model;
    
    public interface ICodeGenerator {

        void GenerateFontSource(TextWriter writer, FontDescriptor fontDescriptor);
    }
}
