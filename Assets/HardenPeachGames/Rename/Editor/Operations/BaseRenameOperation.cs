namespace HardenPeachGames.Renamer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public abstract class BaseRenameOperation : IRenameOperation
    {
        private const string AddedTextColorTag = "<color=green>";
        private const string DeletedTextColorTag = "<color=red>";
        private const string EndColorTag = "</color>";

        public abstract string MenuDisplayPath { get; }

        public abstract int MenuOrder { get; }

        public abstract string Rename(string input, int relativeCount, bool includeDiff);

        public abstract BaseRenameOperation DrawGUI();

        public abstract BaseRenameOperation Clone();

        protected static string ColorStringForAdd(string stringToColor)
        {
            return string.Concat(AddedTextColorTag, stringToColor, EndColorTag);
        }

        protected static string ColorStringForDelete(string stringToColor)
        {
            return string.Concat(DeletedTextColorTag, stringToColor, EndColorTag);
        }
    }
}