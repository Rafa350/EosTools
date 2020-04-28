namespace EosTools.v1.ResourceCompiler.Compiler {

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class CompilerParameters {

        private readonly Dictionary<string, string> items = new Dictionary<string, string>();

        public void Add(string text) {

            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (text.Contains("=")) {
                string[] s = text.Split(new char[] { '=' });
                Add(s[0], s[1]);
            }
            else
                Add(text, null);
        }

        public void Add(string name, string value) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            items.Add(name, value);
        }

        public void Populate(object dataObject) {

            if (dataObject == null)
                throw new ArgumentNullException(nameof(dataObject));

            Type dataObjectType = dataObject.GetType();
            foreach (KeyValuePair<string, string> kv in items) {
                PropertyInfo propInfo = dataObjectType.GetProperty(kv.Key, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo == null)
                    throw new InvalidOperationException(
                        String.Format("No es posible asignar el parametro '{0}'.", kv.Key));
                propInfo.SetValue(dataObject, kv.Value, null);
            }
        }

        public bool Exists(string name) {

            return items.ContainsKey(name);
        }

        public IEnumerable<string> Names {
            get {
                return items.Keys;
            }
        }

        public string this[string name] {
            get {
                return items[name];
            }
        }
    }
}
