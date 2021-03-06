﻿namespace EosTools.v1.ResourceCompilerApp.CmdLine {

    using System;

    public sealed class OptionInfo {

        private readonly OptionDefinition definition;
        private readonly string value;

        public OptionInfo(OptionDefinition definition, string value = null) {

            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            this.definition = definition;
            this.value = value;
        }

        public string Name {
            get {
                return definition.Name;
            }
        }

        public string Value {
            get {
                return value;
            }
        }
    }
}
