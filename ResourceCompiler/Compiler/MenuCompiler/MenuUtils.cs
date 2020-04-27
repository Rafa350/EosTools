namespace EosTools.v1.ResourceCompiler.Compiler.MenuCompiler {

    using System.Linq;
    using System.Collections.Generic;
    using EosTools.v1.ResourceModel.Model;
    using EosTools.v1.ResourceModel.Model.MenuResources;
    
    internal static class MenuUtils {

        private class CommandVisitor: DefaultVisitor {

            private readonly IList<string> commands;

            public CommandVisitor(IList<string> commands) {

                this.commands = commands;
            }

            public override void Visit(CommandItem item) {

                if (commands.IndexOf(item.MenuId) == -1)
                    commands.Add(item.MenuId);
            }
        }

        private class OffsetVisitor: DefaultVisitor {

            private int offset = 0;
            private readonly IDictionary<Item, int> itemOffsets;

            public OffsetVisitor(IDictionary<Item, int> offsets) {

                this.itemOffsets = offsets;
            }

            public override void Visit(Menu menu) {

                offset += 1;                          // Increment per la capcelera
                offset += menu.Title.Length + 1;      // Increment pel titol
                if (menu.Items != null)
                    offset += menu.Items.Count() * 2;   // Increment per la taula d'items

                base.Visit(menu);
            }

            public override void Visit(MenuItem item) {

                itemOffsets.Add(item, offset);

                offset += 1;                          // Increment per la capcelera
                offset += item.Title.Length + 1;      // Increment pel titol

                base.Visit(item);
            }

            public override void Visit(CommandItem item) {

                itemOffsets.Add(item, offset);

                offset += 1;                          // Increment per la capcelera
                offset += item.Title.Length + 1;      // Increment pel titol
                offset += 1;                          // Increment pel codi de la comanda

                base.Visit(item);
            }

            public override void Visit(ExitItem item) {

                itemOffsets.Add(item, offset);

                offset += 1;                          // Increment per la capcelera
                offset += item.Title.Length + 1;      // Increment pel titol

                base.Visit(item);
            }
        }

        public static IList<string> GetCommandList(MenuResource resource) {

            List<string> commands = new List<string>();
            CommandVisitor visitor = new CommandVisitor(commands);
            visitor.Visit(resource.Menu);
            return commands;
        }

        public static IDictionary<Item, int> GetOffsetDictionary(MenuResource resource) {

            Dictionary<Item, int> offsets = new Dictionary<Item, int>();
            OffsetVisitor visitor = new OffsetVisitor(offsets);
            visitor.Visit(resource.Menu);
            return offsets;
        }
    }
}
