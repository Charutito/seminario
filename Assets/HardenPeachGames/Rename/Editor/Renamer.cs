namespace HardenPeachGames.Renamer
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Renamer
    {
        private List<IRenameOperation> renameOperations;

        private List<IRenameOperation> RenameOperations
        {
            get
            {
                if (this.renameOperations == null)
                {
                    this.renameOperations = new List<IRenameOperation>();
                }

                return this.renameOperations;
            }
        }

        public void SetRenameOperation(IRenameOperation operation)
        {
            var operationList = new List<IRenameOperation>();
            operationList.Add(operation);
            this.SetRenameOperations(operationList);
        }

        public void SetRenameOperations(List<IRenameOperation> operations)
        {
            this.RenameOperations.Clear();
            this.RenameOperations.AddRange(operations);
        }

        public string[] GetRenamedStrings(bool includeDiff, params string[] originalNames)
        {
            var renamedStrings = new string[originalNames.Length];

            for (int i = 0; i < originalNames.Length; ++i)
            {
                renamedStrings[i] = this.GetRenamedString(originalNames[i], i, includeDiff);
            }

            return renamedStrings;
        }

        private string GetRenamedString(string originalName, int count, bool includeDiff)
        {
            var modifiedName = originalName;

            foreach (var op in this.RenameOperations)
            {
                modifiedName = op.Rename(modifiedName, count, includeDiff);
            }

            return modifiedName;
        }
    }
}