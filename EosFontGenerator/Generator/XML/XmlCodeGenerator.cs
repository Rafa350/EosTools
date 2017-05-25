namespace Media.PicFontGenerator.Generator.XML {

    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Xml;
    using Media.PicFontGenerator.Model;
    
    public sealed class XmlCodeGenerator: ICodeGenerator {

        public void GenerateFontSource(TextWriter writer, FontDescriptor fontDescriptor) {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            settings.Indent = true;
            settings.IndentChars = "    ";
            XmlWriter wr = XmlWriter.Create(writer, settings);
            try {
                StringBuilder sbName = new StringBuilder();
                sbName.Append(fontDescriptor.Font.Name.Replace(" ", ""));
                if (fontDescriptor.Font.Bold)
                    sbName.Append(" Bold");
                if (fontDescriptor.Font.Italic)
                    sbName.Append(" Italic");
                sbName.AppendFormat(" {0}pt", fontDescriptor.Font.SizeInPoints);

                wr.WriteStartDocument();

                wr.WriteComment(" Creado con EosFontGenerator ");
                wr.WriteComment(" No modificar ");
                wr.WriteComment(String.Format(" Name:  {0} ", fontDescriptor.Font.Name));
                wr.WriteComment(String.Format(" Size:  {0}pt ", fontDescriptor.Font.SizeInPoints));
                wr.WriteComment(String.Format(" Style: {0} ", fontDescriptor.Font.Style));

                wr.WriteStartElement("resources");
                wr.WriteStartElement("fontResource");
                wr.WriteAttributeString("version", "2.0");
                wr.WriteAttributeString("resourceId", sbName.ToString().Replace(" ", ""));

                wr.WriteStartElement("font");
                wr.WriteAttributeString("name", sbName.ToString());
                wr.WriteAttributeString("height", fontDescriptor.Height.ToString());
                wr.WriteAttributeString("ascent", fontDescriptor.Ascent.ToString());
                wr.WriteAttributeString("descent", fontDescriptor.Descent.ToString());
                wr.WriteAttributeString("isBold", fontDescriptor.Font.Bold.ToString());
                wr.WriteAttributeString("isItalic", fontDescriptor.Font.Italic.ToString());

                foreach (CharacterDescriptor characterDescriptor in fontDescriptor.CharacterDescriptors) {
                    wr.WriteStartElement("char");
                    wr.WriteAttributeString("code", String.Format("0x{0:X2}", Convert.ToInt32(characterDescriptor.Character)));
                    wr.WriteAttributeString("left", characterDescriptor.Left.ToString());
                    wr.WriteAttributeString("top", characterDescriptor.Top.ToString());
                    wr.WriteAttributeString("width", characterDescriptor.Width.ToString());
                    wr.WriteAttributeString("height", characterDescriptor.Height.ToString());
                    wr.WriteAttributeString("advance", characterDescriptor.Advance.ToString());

                    if (characterDescriptor.Bitmap != null) {
                        wr.WriteStartElement("bitmap");
                        wr.WriteAttributeString("bpp", "1");
                        for (int y = 0; y < characterDescriptor.Height; y++) {
                            wr.WriteStartElement("scanLine");
                            StringBuilder sb = new StringBuilder();
                            try {
                                int black = Color.Black.ToArgb();
                                for (int x = 0; x < characterDescriptor.Width; x++) {
                                    if (characterDescriptor.Bitmap.GetPixel(x, y).ToArgb() == black)
                                        sb.Append('#');
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
