﻿namespace EosTools.v1.ResourceCompilerApp.CmdLine {

    using System;

    public sealed class OptionDefinition {

        private readonly string name;
        private readonly string description;
        private readonly bool required;
        private readonly bool multiple;

        public OptionDefinition(string name, string description = null, bool required = false, bool multiple = false) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.description = description;
            this.required = required;
            this.multiple = multiple;
        }

        public string Name {
            get {
                return name;
            }
        }

        public string Description {
            get {
                return description;
            }
        }

        public bool Required {
            get {
                return required;
            }
        }

        public bool Multiple {
            get {
                return multiple;
            }
        }
    }
}
