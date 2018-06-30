namespace HardenPeachGames.Renamer
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    using UnityObject = UnityEngine.Object;

    [InitializeOnLoad]
    public static class RBSelection
    {
        private static List<UnityObject> selectedObjectsSortedByTime;

        static RBSelection()
        {
            selectedObjectsSortedByTime = new List<UnityObject>();
            Selection.selectionChanged += RefreshObjectsToRename;
        }

        public static List<UnityObject> SelectedObjectsSortedByTime
        {
            get
            {
                return new List<UnityObject>(selectedObjectsSortedByTime);
            }
        }

        private static void RefreshObjectsToRename()
        {
            var unitySelectedObjects = new List<UnityObject>(Selection.objects);
            var addedObjects = GetObjectsInBNotInA(selectedObjectsSortedByTime, unitySelectedObjects);
            var removedObjects = GetObjectsInBNotInA(unitySelectedObjects, selectedObjectsSortedByTime);

            foreach (var removedObject in removedObjects)
            {
                selectedObjectsSortedByTime.Remove(removedObject);
            }

            foreach (var addedObject in addedObjects)
            {
                selectedObjectsSortedByTime.Add(addedObject);
            }
        }

        private static List<UnityObject> GetObjectsInBNotInA(List<UnityObject> listA, List<UnityObject> listB)
        {
            var newObjects = new List<UnityObject>();
            foreach (var obj in listB)
            {
                if (!listA.Contains(obj))
                {
                    newObjects.Add(obj);
                }
            }

            return newObjects;
        }
    }
}