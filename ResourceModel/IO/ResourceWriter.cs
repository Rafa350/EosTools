namespace EosTools.v1.ResourceModel.IO {

    using System;
    using System.IO;
    using System.Xml;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    
    public sealed class ResourceWriter: IResourceWriter {

        private readonly Stream stream;

        private sealed class ResourceVisitor: DefaultVisitor {

            private readonly XmlWriter writer;

            public ResourceVisitor(XmlWriter writer) {

                this.writer = writer;
            }

            public override void Visit(MenuResource resource) {

                writer.WriteStartElement("menuResource");
                writer.WriteAttributeString("resourceId", resource.ResourceId);
                writer.WriteAttributeString("language", resource.Languaje);

                base.Visit(resource);

                writer.WriteEndElement();
            }

            public override void Visit(Menu menu) {

                writer.WriteStartElement("menu");
                writer.WriteAttributeString("title", menu.Title);

                base.Visit(menu);

                writer.WriteEndElement();
            }

            public override void Visit(CommandItem item) {

                writer.WriteStartElement("commandItem");
                writer.WriteAttributeString("title", item.Title);
                writer.WriteAttributeString("command", item.Command.ToString());

                base.Visit(item);

                writer.WriteEndElement();
            }

            public override void Visit(MenuItem item) {

                writer.WriteStartElement("menuItem");
                writer.WriteAttributeString("title", item.Title);

                base.Visit(item);

                writer.WriteEndElement();
            }
        }

        public ResourceWriter(Stream stream) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            this.stream = stream;
        }

        public void Write(ResourcePool resources) {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            settings.Indent = true;
            settings.IndentChars = "    ";
            XmlWriter writer = XmlWriter.Create(stream, settings);
            try {
                writer.WriteStartDocument();
                writer.WriteStartElement("resources");

                ResourceVisitor visitor = new ResourceVisitor(writer);
                foreach (Resource resource in resources.Resources)
                    resource.AcceptVisitor(visitor);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            finally {
                writer.Close();
            }
        }
    }
}
