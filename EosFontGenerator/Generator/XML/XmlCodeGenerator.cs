namespace EosTools.v1.FontGeneratorApp.Generator.XML {

    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Xml;
    using EosTools.v1.FontGeneratorApp.Infrastructure;
    using EosTools.v1.FontGeneratorApp.Model;

    public sealed class XmlCodeGenerator: ICodeGenerator {

        public void GenerateFontSource(TextWriter writer, FontDescriptor fontDescriptor) {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            settings.Indent = true;
            settings.IndentChars = "    ";
            XmlWriter wr = XmlWriter.Create(writer, settings);
            try {
                FontData fontData = new FontData(fontDescriptor.Font);

                wr.WriteStartDocument();

                wr.WriteComment(" Creado con EosFontGenerator ");
                wr.WriteComment(" No modificar ");
                wr.WriteComment(String.Format(" Name  : {0} ", fontData.Name));
                wr.WriteComment(String.Format(" Size  : {0}pt ", fontDescriptor.Font.SizeInPoints));
                wr.WriteComment(String.Format(" Style : {0} ", fontDescriptor.Font.Style));

                wr.WriteStartElement("resources");
                wr.WriteStartElement("fontResource");
                wr.WriteAttributeString("version", "2.0");
                wr.WriteAttributeString("resourceId", fontData.Name.Replace(", ", ""));

                wr.WriteStartElement("font");
                wr.WriteAttributeString("name", fontData.Name);
                wr.WriteAttributeString("height", fontData.Height.ToString());
                wr.WriteAttributeString("ascent", fontData.Ascent.ToString());
                wr.WriteAttributeString("descent", fontData.Descent.ToString());

                foreach (CharacterDescriptor characterDescriptor in fontDescriptor.CharacterDescriptors) {

                    char ch = characterDescriptor.Character;
                    GlyphData glyphData = new GlyphData(characterDescriptor.Font, ch, GlyphFormat.L1);
                    GlyphBitmap glyphBitmap = glyphData.Glyph;

                    wr.WriteStartElement("char");
                    wr.WriteAttributeString("code", String.Format("0x{0:X2}", Convert.ToInt32(ch)));
                    wr.WriteAttributeString("advance", glyphData.Advance.ToString());

                    if (glyphBitmap != null) {
                        wr.WriteStartElement("bitmap");
                        wr.WriteAttributeString("format", "L1");
                        wr.WriteAttributeString("left", glyphBitmap.OffsetX.ToString());
                        wr.WriteAttributeString("top", glyphBitmap.OffsetY.ToString());
                        wr.WriteAttributeString("width", glyphBitmap.Width.ToString());
                        wr.WriteAttributeString("height", glyphBitmap.Height.ToString());
                        for (int y = 0; y < glyphBitmap.Height; y++) {
                            wr.WriteStartElement("scanLine");
                            StringBuilder sb = new StringBuilder();
                            try {
                                int black = Color.Black.ToArgb();
                                for (int x = 0; x < glyphBitmap.Width; x++) {
                                    if (glyphBitmap.Bitmap.GetPixel(x, y).ToArgb() == black)
                                        sb.Append('1');
                                    else
                                        sb.Append('.');
                                }
                            }
                            catch {
                            }

                            wr.WriteString(sb.ToString());
                            wr.WriteEndElement();
                        }
                        wr.WriteEndElement();
                    }

                    wr.WriteEndElement();
                }
                
                wr.WriteEndElement();
                
                wr.WriteEndElement();
                wr.WriteEndElement();
                wr.WriteEndDocument();
            }
            finally {
                wr.Close();
            }
        }
    }
}
