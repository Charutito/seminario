namespace HardenPeachGames.Renamer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Tool that tries to allow renaming mulitple selections by parsing similar substrings
    /// </summary>
    public class RenamerWindow : EditorWindow
    {
        private const string AssetsMenuPath = "Game/Renamer";
        private const string GameObjectMenuPath = "GameObject/Game/Renamer";

        private Vector2 renameOperationsPanelScrollPosition;
        private Vector2 previewPanelScrollPosition;
        private List<UnityEngine.Object> objectsToRename;

        private List<BaseRenameOperation> renameOperationsFactory;

        private Renamer renamer;
        private List<BaseRenameOperation> renameOperationsToApply;

        [MenuItem(AssetsMenuPath, false, 1011)]
        [MenuItem(GameObjectMenuPath, false, 49)]
        private static void ShowRenameSpritesheetWindow()
        {
            EditorWindow.GetWindow<RenamerWindow>(true, "Renamer ", true);
        }

        [MenuItem(AssetsMenuPath, true)]
        [MenuItem(GameObjectMenuPath, true)]
        private static bool IsAssetSelectionValid()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }

            // Allow a rename if any valid object is selected. The invalid ones won't be renamed.
            foreach (var selection in Selection.objects)
            {
                if (ObjectIsValidForRename(selection))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ObjectIsValidForRename(UnityEngine.Object obj)
        {
            if (AssetDatabase.Contains(obj))
            {
                // Create -> Prefab results in assets that have no name. Typically you can't have Assets that have no name,
                // so we will just ignore them for the utility.
                return !string.IsNullOrEmpty(obj.name);
            }

            if (obj.GetType() == typeof(GameObject))
            {
                return true;
            }

            return false;
        }

        private static void DrawPreviewTitle()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(32.0f);
            EditorGUILayout.LabelField("Diff", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("New Name", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawPreviewRow(PreviewRowInfo info)
        {
            // Draw the icon
            EditorGUILayout.BeginHorizontal(GUILayout.Height(18.0f));
            GUILayout.Space(8.0f);
            if (info.Icon != null)
            {
                GUIStyle boxStyle = GUIStyle.none;
                GUILayout.Box(info.Icon, boxStyle, GUILayout.Width(16.0f), GUILayout.Height(16.0f));
            }

            // Display diff
            var diffStyle = info.NamesAreDifferent ? EditorStyles.boldLabel : new GUIStyle(EditorStyles.label);
            diffStyle.richText = true;
            EditorGUILayout.LabelField(info.DiffName, diffStyle);

            // Display new name
            var style = info.NamesAreDifferent ? EditorStyles.boldLabel : new GUIStyle(EditorStyles.label);
            EditorGUILayout.LabelField(info.NewName, style);

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawDivider()
        {
            GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(2.0f));
        }

        private static Texture GetIconForObject(UnityEngine.Object unityObject)
        {
            var pathToObject = AssetDatabase.GetAssetPath(unityObject);
            Texture icon = null;
            if (string.IsNullOrEmpty(pathToObject))
            {
                if (unityObject.GetType() == typeof(GameObject))
                {
                    icon = EditorGUIUtility.FindTexture("GameObject Icon");
                }
                else
                {
                    icon = EditorGUIUtility.FindTexture("DefaultAsset Icon");
                }
            }
            else
            {
                icon = AssetDatabase.GetCachedIcon(pathToObject);
            }

            return icon;
        }

        private void OnEnable()
        {
            this.minSize = new Vector2(600.0f, 300.0f);

            this.previewPanelScrollPosition = Vector2.zero;

            this.renamer = new Renamer();
            this.renameOperationsToApply = new List<BaseRenameOperation>();
            this.renameOperationsToApply.Add(new ReplaceStringOperation());

            // Cache all valid Rename Operations
            this.renameOperationsFactory = new List<BaseRenameOperation>();
            var assembly = Assembly.Load(new AssemblyName("Assembly-CSharp-Editor"));
            var typesInAssembly = assembly.GetTypes();
            foreach (var type in typesInAssembly)
            {
                if (type.IsSubclassOf(typeof(BaseRenameOperation)))
                {
                    var renameOp = (BaseRenameOperation)System.Activator.CreateInstance(type);
                    this.renameOperationsFactory.Add(renameOp);
                }
            }

            this.renameOperationsFactory.Sort((x, y) =>
            {
                return x.MenuOrder.CompareTo(y.MenuOrder);
            });

            Selection.selectionChanged += this.Repaint;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= this.Repaint;
        }

        private void RefreshObjectsToRename()
        {
            if (this.objectsToRename == null)
            {
                this.objectsToRename = new List<UnityEngine.Object>();
            }

            this.objectsToRename.Clear();

            foreach (var selectedObject in RBSelection.SelectedObjectsSortedByTime)
            {
                if (ObjectIsValidForRename(selectedObject))
                {
                    this.objectsToRename.Add(selectedObject);
                }
            }
        }

        private void OnGUI()
        {
            this.RefreshObjectsToRename();

            EditorGUILayout.HelpBox(
                "Renamer permite renombrar multiples selecciones de objetos que esten o no en la escena.",
                MessageType.None);

            if (this.objectsToRename.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No seleccionaste ningun objeto, selecciona al menos un Asset o un objeto de la escena para renombrarlo.",
                    MessageType.Error);
                return;
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            this.renameOperationsPanelScrollPosition =
                EditorGUILayout.BeginScrollView(
                this.renameOperationsPanelScrollPosition,
                GUILayout.MinWidth(300.0f),
                GUILayout.MaxWidth(500.0f));

            for (int i = 0; i < this.renameOperationsToApply.Count; ++i)
            {
                this.renameOperationsToApply[i] = this.renameOperationsToApply[i].DrawGUI();
                DrawDivider();
            }

            var renameOpsAsInterfaces = new List<IRenameOperation>();
            foreach (var renameOp in this.renameOperationsToApply)
            {
                renameOpsAsInterfaces.Add((IRenameOperation)renameOp);
            }

            this.renamer.SetRenameOperations(renameOpsAsInterfaces);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Operation", GUILayout.Width(150.0f)))
            {
                // Add enums to the menu
                var menu = new GenericMenu();
                for (int i = 0; i < this.renameOperationsFactory.Count; ++i)
                {
                    var renameOp = this.renameOperationsFactory[i];
                    var content = new GUIContent(renameOp.MenuDisplayPath);
                    menu.AddItem(content, false, this.OnAddRenameOperationConfirmed, renameOp);
                }

                menu.ShowAsContext();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();

            GUILayout.Box(string.Empty, GUILayout.ExpandHeight(true), GUILayout.Width(3.0f));

            EditorGUILayout.BeginVertical();

            this.previewPanelScrollPosition = EditorGUILayout.BeginScrollView(this.previewPanelScrollPosition);

            // Note that something about the way we draw the preview title, requires it to be included in the scroll view in order
            // for the scroll to measure horiztonal size correctly.
            DrawPreviewTitle();

            var previewRowData = this.GetPreviewRowDataFromObjectsToRename();
            for (int i = 0; i < previewRowData.Length; ++i)
            {
                DrawPreviewRow(previewRowData[i]);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30.0f);
            if (GUILayout.Button("Rename", GUILayout.Height(24.0f)))
            {
                this.RenameAssets();
                this.Close();
            }

            GUILayout.Space(30.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        private void OnAddRenameOperationConfirmed(object operation)
        {
            var operationAsRenameOp = operation as BaseRenameOperation;
            if (operationAsRenameOp == null)
            {
                throw new System.ArgumentException(
                    "RenamerWindow tried to add a new RenameOperation using a type that is not a subclass of BaseRenameOperation." +
                    " Operation type: " +
                    operation.GetType().ToString());
            }

            // Construct the Rename op
            var renameOp = operationAsRenameOp.Clone();
            this.renameOperationsToApply.Add(renameOp);

            // Scroll to the bottom to focus the newly created operation.
            this.renameOperationsPanelScrollPosition = new Vector2(0.0f, 10000000.0f);
            this.Repaint();
        }

        private PreviewRowInfo[] GetPreviewRowDataFromObjectsToRename()
        {
            var previewRowInfos = new PreviewRowInfo[this.objectsToRename.Count];
            var selectedNames = this.GetNamesFromObjectsToRename();
            var namePreviews = this.renamer.GetRenamedStrings(false, selectedNames);
            var nameDiffs = this.renamer.GetRenamedStrings(true, selectedNames);

            for (int i = 0; i < selectedNames.Length; ++i)
            {
                var info = new PreviewRowInfo();
                info.OriginalName = selectedNames[i];
                info.DiffName = nameDiffs[i];
                info.NewName = namePreviews[i];
                info.Icon = GetIconForObject(this.objectsToRename[i]);

                previewRowInfos[i] = info;
            }

            return previewRowInfos;
        }

        private string[] GetNamesFromObjectsToRename()
        {
            int namesCount = this.objectsToRename.Count;
            var names = new string[namesCount];
            for (int i = 0; i < namesCount; ++i)
            {
                names[i] = this.objectsToRename[i].name;
            }

            return names;
        }

        private void RenameAssets()
        {
            // Record all the objects to undo stack, though this unfortunately doesn't capture Asset renames
            Undo.RecordObjects(this.objectsToRename.ToArray(), "Renamer");

            var names = this.GetNamesFromObjectsToRename();
            var newNames = this.renamer.GetRenamedStrings(false, names);

            for (int i = 0; i < newNames.Length; ++i)
            {
                var infoString = string.Format(
                                     "Renaming asset {0} of {1}",
                                     i,
                                     newNames.Length);

                EditorUtility.DisplayProgressBar(
                    "Renaming Assets...",
                    infoString,
                    i / (float)newNames.Length);

                this.RenameObject(this.objectsToRename[i], newNames[i]);
            }

            EditorUtility.ClearProgressBar();
        }

        private void RenameObject(UnityEngine.Object obj, string newName)
        {
            if (AssetDatabase.Contains(obj))
            {
                this.RenameAsset(obj, newName);
            }
            else
            {
                this.RenameGameObject(obj, newName);
            }
        }

        private void RenameGameObject(UnityEngine.Object gameObject, string newName)
        {
            gameObject.name = newName;
        }

        private void RenameAsset(UnityEngine.Object asset, string newName)
        {
            var pathToAsset = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(pathToAsset, newName);
        }

        private struct PreviewRowInfo
        {
            public Texture Icon { get; set; }

            public string OriginalName { get; set; }

            public string DiffName { get; set; }

            public string NewName { get; set; }

            public bool NamesAreDifferent
            {
                get
                {
                    return this.NewName != this.OriginalName;
                }
            }
        }
    }
}