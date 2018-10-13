using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuilderScript
{
    private const string BUILD_PATH = "Builds/{0}/{1}/";
    
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
