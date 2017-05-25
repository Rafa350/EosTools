namespace EosTools.v1.ResourceModel.Model.MenuResources {

    public sealed class CommandItem: Item {

        private string command;

        public CommandItem(string title, string command) 
            : base(title) {

            this.command = command;
        }

        public CommandItem(string title, string displayFormat, string command)
            : base(title, displayFormat) {

            this.command = command;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Command {
            get {
                return command;
            }
        }
    }
}
