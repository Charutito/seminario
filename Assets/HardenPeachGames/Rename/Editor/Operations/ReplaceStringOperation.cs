namespace HardenPeachGames.Renamer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class ReplaceStringOperation : BaseRenameOperation
    {

        public ReplaceStringOperation()
        {
            this.SearchString = string.Empty;
            this.SearchIsCaseSensitive = false;
            this.ReplacementString = string.Empty;
        }

        public ReplaceStringOperation(ReplaceStringOperation operationToCopy)
        {
            this.SearchString = operationToCopy.SearchString;
            this.SearchIsCaseSensitive = operationToCopy.SearchIsCaseSensitive;
            this.ReplacementString = operationToCopy.ReplacementString;
        }

        public override string MenuDisplayPath
        {
            get
            {
                return "Replace String";
            }
        }

        public override int MenuOrder
        {
            get
            {
                return 0;
            }
        }

        public string SearchString { get; set; }

        public bool SearchIsCaseSensitive { get; set; }

        public string ReplacementString { get; set; }

        public override BaseRenameOperation Clone()
        {
            var clone = new ReplaceStringOperation(this);
            return clone;
        }

        public override string Rename(string input, int relativeCount, bool includeDiff)
        {
            if (!string.IsNullOrEmpty(this.SearchString))
            {
                // Create capture group regex to extract the matched string
                // Escape the non-regex search string to prevent it from being interpretted as regex.
                var searchStringRegexPattern = string.Concat("(", Regex.Escape(this.SearchString), ")");
                var regexOptions = this.SearchIsCaseSensitive ? default(RegexOptions) : RegexOptions.IgnoreCase;

                var replacement = string.Empty;
                if (includeDiff)
                {
                    replacement = BaseRenameOperation.ColorStringForDelete("$1");
                    replacement += BaseRenameOperation.ColorStringForAdd(this.ReplacementString);
                }
                else
                {
                    replacement = this.ReplacementString;
                }

                return Regex.Replace(input, searchStringRegexPattern, replacement, regexOptions);
            }
            else
            {
                return input;
            }
        }

        public override BaseRenameOperation DrawGUI()
        {   
            var clone = new ReplaceStringOperation(this);
            EditorGUILayout.LabelField("Text Replacement", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            clone.SearchString = EditorGUILayout.TextField("Search for String", this.SearchString);
            clone.ReplacementString = EditorGUILayout.TextField("Replace with", this.ReplacementString);
            clone.SearchIsCaseSensitive = EditorGUILayout.Toggle("Case Sensitive", this.SearchIsCaseSensitive);
            EditorGUI.indentLevel--;
            return clone;
        }
    }
}