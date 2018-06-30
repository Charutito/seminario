namespace HardenPeachGames.Renamer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class EnumerateOperation : BaseRenameOperation
    {

        public EnumerateOperation()
        {
            this.StartingCount = 0;
            this.CountFormat = string.Empty;
        }

        public EnumerateOperation(EnumerateOperation operationToCopy)
        {
            this.StartingCount = operationToCopy.StartingCount;
            this.CountFormat = operationToCopy.CountFormat;
        }

        public override string MenuDisplayPath
        {
            get
            {
                return "Enumerate";
            }
        }

        public override int MenuOrder
        {
            get
            {
                return 4;
            }
        }

        public int StartingCount { get; set; }

        public string CountFormat { get; set; }

        public override BaseRenameOperation Clone()
        {
            var clone = new EnumerateOperation(this);
            return clone;
        }

        public override string Rename(string input, int relativeCount, bool includeDiff)
        {
            var modifiedName = input;
            var currentCount = this.StartingCount + relativeCount;

            if (!string.IsNullOrEmpty(this.CountFormat))
            {
                try
                {
                    var currentCountAsString = currentCount.ToString(this.CountFormat);

                    if (includeDiff)
                    {
                        currentCountAsString = string.Concat(BaseRenameOperation.ColorStringForAdd(currentCountAsString));
                    }

                    modifiedName = string.Concat(modifiedName, currentCountAsString);
                }
                catch (System.FormatException)
                {
                    
                }
            }

            return modifiedName;
        }

        public override BaseRenameOperation DrawGUI()
        {   
            var clone = new EnumerateOperation(this);
            EditorGUILayout.LabelField("Enumerating", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            clone.CountFormat = EditorGUILayout.TextField("Count Format", this.CountFormat);

            try
            {
                clone.StartingCount.ToString(clone.CountFormat);
            }
            catch (System.FormatException)
            {
                var helpBoxMessage = "Invalid Count Format. Typical formats are D1 for one digit with no " +
                                     "leading zeros, D2, for two, etc." +
                                     "\nSee https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx" +
                                     " for more formatting options.";
                EditorGUILayout.HelpBox(helpBoxMessage, MessageType.Warning);
            }

            clone.StartingCount = EditorGUILayout.IntField("Count From", this.StartingCount);

            EditorGUI.indentLevel--;
            return clone;
        }
    }
}