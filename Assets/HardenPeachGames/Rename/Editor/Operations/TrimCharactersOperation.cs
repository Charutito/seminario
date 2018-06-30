namespace HardenPeachGames.Renamer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class TrimCharactersOperation : BaseRenameOperation
    {

        public TrimCharactersOperation()
        {
            this.NumFrontDeleteChars = 0;
            this.NumBackDeleteChars = 0;
        }

        public TrimCharactersOperation(TrimCharactersOperation operationToCopy)
        {
            this.NumFrontDeleteChars = operationToCopy.NumFrontDeleteChars;
            this.NumBackDeleteChars = operationToCopy.NumBackDeleteChars;
        }

        public override string MenuDisplayPath
        {
            get
            {
                return "Trim Characters";
            }
        }

        public override int MenuOrder
        {
            get
            {
                return 3;
            }
        }

        public int NumFrontDeleteChars { get; set; }

        public int NumBackDeleteChars { get; set; }

        public override BaseRenameOperation Clone()
        {
            var clone = new TrimCharactersOperation(this);
            return clone;
        }

        public override string Rename(string input, int relativeCount, bool includeDiff)
        {
            var modifiedName = input;

            // Trim Front chars
            string trimmedFrontChars = string.Empty;
            if (this.NumFrontDeleteChars > 0)
            {
                var numCharsToDelete = Mathf.Min(this.NumFrontDeleteChars, input.Length);
                trimmedFrontChars = input.Substring(0, numCharsToDelete);

                modifiedName = modifiedName.Remove(0, numCharsToDelete);
            }

            // Trim Back chars
            string trimmedBackChars = string.Empty;
            if (this.NumBackDeleteChars > 0)
            {
                var numCharsToDelete = Mathf.Min(this.NumBackDeleteChars, modifiedName.Length);
                int startIndex = modifiedName.Length - numCharsToDelete;
                trimmedBackChars = modifiedName.Substring(startIndex, numCharsToDelete);

                modifiedName = modifiedName.Remove(startIndex, numCharsToDelete);
            }

            if (includeDiff)
            {
                if (!string.IsNullOrEmpty(trimmedFrontChars))
                {
                    modifiedName = string.Concat(BaseRenameOperation.ColorStringForDelete(trimmedFrontChars), modifiedName);
                }

                if (!string.IsNullOrEmpty(trimmedBackChars))
                {
                    modifiedName = string.Concat(modifiedName, BaseRenameOperation.ColorStringForDelete(trimmedBackChars));
                }
            }

            return modifiedName;
        }

        public override BaseRenameOperation DrawGUI()
        {   
            var clone = new TrimCharactersOperation(this);
            EditorGUILayout.LabelField("Trimming", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            clone.NumFrontDeleteChars = EditorGUILayout.IntField("Delete From Front", this.NumFrontDeleteChars);
            clone.NumFrontDeleteChars = Mathf.Max(0, clone.NumFrontDeleteChars);
            clone.NumBackDeleteChars = EditorGUILayout.IntField("Delete from Back", this.NumBackDeleteChars);
            clone.NumBackDeleteChars = Mathf.Max(0, clone.NumBackDeleteChars);
            EditorGUI.indentLevel--;
            return clone;
        }
    }
}