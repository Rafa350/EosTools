namespace EosTools.v1.ResourceCompiler.Compiler {

    using System;
    using System.IO;
    using System.Text;
    
    internal sealed class CodeBuilder {

        private StringBuilder sb = new StringBuilder();

        public CodeBuilder Append(string text) {

            sb.Append(text);

            return this;
        }

        public CodeBuilder Append(string format, params object[] args) {

            sb.AppendFormat(format, args);

            return this;
        }

        public CodeBuilder Append(TextReader reader) {

            sb.Append(reader.ReadToEnd());

            return this;
        }

        public CodeBuilder AppendLine() {

            sb.AppendLine();

            return this;
        }

        public CodeBuilder AppendLine(string text) {

            return Append(text);
        }

        public CodeBuilder AppendLine(string format, params object[] args) {

            return Append(format, args);
        }

        public override string ToString() {

            return sb.ToString();
        }
    }
}
