using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuilderScript
{
    private const string BUILD_PATH = "{0}_{1}H{2}M_{3}";
    private const string DEFAULT_EXTENSION = ".exe";
    
    private static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Builds");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            return path;
        }
    }
    
    public static string ProductName
    {
        get
        {
            return PlayerSettings.productName;
        }
    }
    
    [MenuItem("Test/Menu")]
    public static void BuildWindows64()
    {
        GenericBuild(BuildTarget.StandaloneWindows64);
    }

    private static BuildPlayerOptions GetDefaultOptions()
    {
        return new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            options = BuildOptions.None
        };
    }

    private static void GenericBuild(BuildTarget target)
    {
        string path = Path.Combine(Path.Combine(BuildPathRoot, target.ToString()), string.Format(BUILD_PATH, ProductName, System.DateTime.Now.ToString("dd.MM.yyyy_HH"), System.DateTime.Now.ToString("mm"), target));
        string name = ProductName + DEFAULT_EXTENSION;
        
        BuildPlayerOptions buildPlayerOptions = GetDefaultOptions();
        buildPlayerOptions.target = target;
        buildPlayerOptions.locationPathName = Path.Combine(path, name);

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
