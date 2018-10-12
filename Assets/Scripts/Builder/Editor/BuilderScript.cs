using System.Linq;
using UnityEditor;
using UnityEngine;


public class BuilderScript
{
    private const string BUILD_PATH = "Builds/{0}/{1}/";
    
    private static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), ProductName);
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            return path;
        }
    }
    
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
        string pathToDeploy = string.Format(BUILD_PATH, Application.version, target);
        
        BuildPlayerOptions buildPlayerOptions = GetDefaultOptions();
        buildPlayerOptions.target = target;
        buildPlayerOptions.locationPathName = pathToDeploy;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
