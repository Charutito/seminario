namespace HardenPeachGames.Renamer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class AddStringOperation : BaseRenameOperation
    {
        public AddStringOperation()
        {
            this.Prefix = string.Empty;
            this.Suffix = string.Empty;
        }

        public AddStringOperation(AddStringOperation operationToCopy)
        {
            this.Prefix = operationToCopy.Prefix;
            this.Suffix = operationToCopy.Suffix;
        }

        public override string MenuDisplayPath
        {
            get
            {
                return "Add String";
            }
        }

        public override int MenuOrder
        {
            get
            {
                return 1;
            }
        }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public override BaseRenameOperation Clone()
        {
            var clone = new AddStringOperation(this);
            return clone;
        }

        public override string Rename(string input, int relativeCount, bool includeDiff)
        {
            var modifiedName = input;
            if (!string.IsNullOrEmpty(this.Prefix))
            {
                var prefix = includeDiff ? BaseRenameOperation.ColorStringForAdd(this.Prefix) : this.Prefix;
                modifiedName = string.Concat(prefix, modifiedName);
            }

            if (!string.IsNullOrEmpty(this.Suffix))
            {
                var suffix = includeDiff ? BaseRenameOperation.ColorStringForAdd(this.Suffix) : this.Suffix;
                modifiedName = string.Concat(modifiedName, suffix);
            }

            return modifiedName;
        }

        public override BaseRenameOperation DrawGUI()
        {   
            var clone = new AddStringOperation(this);
            EditorGUILayout.LabelField("Additions", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            clone.Prefix = EditorGUILayout.TextField("Prefix", this.Prefix);
            clone.Suffix = EditorGUILayout.TextField("Suffix", this.Suffix);
            EditorGUI.indentLevel--;
            return clone;
        }
    }
}