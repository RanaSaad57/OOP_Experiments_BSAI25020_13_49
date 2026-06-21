#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GitHubPagesBuild
{
    public static void BuildModules()
    {
        ModulesShowcaseBuilder.Build();
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;

        string output = Path.GetFullPath(Path.Combine(
            Application.dataPath, "../../docs/modules"));

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { ModulesShowcaseBuilder.ScenePath },
            locationPathName = output,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new BuildFailedException("Modules WebGL build failed.");
        }
    }
}
#endif
