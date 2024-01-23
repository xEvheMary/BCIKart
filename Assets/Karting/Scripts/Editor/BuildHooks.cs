using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LSL4Unity.Editor
{
public class BuildHooks
{
	private const string LIB_LSL_NAME = "liblsl";
	private const string PLUGIN_DIR   = "Plugins";
	private const string X86_FOLDER   = "x86";
	private const string X64_FOLDER   = "x86_64";
	private const string IOS_FOLDER   = "iOS";

	/// <summary> Called after the build. See also <see cref="PostProcessBuildAttribute"/>. </summary>
	/// <param name="target"> The build target. </param>
	/// <param name="path"> The build path. </param>
	[PostProcessBuild(1)] public static void OnPostprocessBuild(BuildTarget target, string path)
	{
		if (path != null) {
			var buildDir     = Path.GetFileNameWithoutExtension(path);
			var buildHostDir = path.Replace(Path.GetFileName(path), "");
			var dataDir      = buildDir + "_Data";
			var dataPath     = Path.Combine(buildHostDir, dataDir);
			var pluginDir    = Path.Combine(dataPath,     PLUGIN_DIR);

			switch (target) {
				case BuildTarget.StandaloneWindows:
					RenameLibFile(pluginDir, X86_FOLDER, LSLEditorIntegration.LIB32_NAME, LSLEditorIntegration.LIB64_NAME, LSLEditorIntegration.DLL_ENDING);
					break;
				case BuildTarget.StandaloneWindows64:
					RenameLibFile(pluginDir, X64_FOLDER, LSLEditorIntegration.LIB64_NAME, LSLEditorIntegration.LIB32_NAME, LSLEditorIntegration.DLL_ENDING);
					break;
				case BuildTarget.StandaloneLinux64:
					RenameLibFile(pluginDir, X64_FOLDER, LSLEditorIntegration.LIB64_NAME, LSLEditorIntegration.LIB32_NAME, LSLEditorIntegration.SO_ENDING);
					break;
				case BuildTarget.StandaloneOSX:
					RenameLibFile(pluginDir, IOS_FOLDER, LSLEditorIntegration.LIB64_NAME, LSLEditorIntegration.LIB32_NAME, LSLEditorIntegration.BUNDLE_ENDING);
					break;
			}
		}
	}

	/// <summary> Renames the library file. </summary>
	/// <param name="pluginDir"> The plugin directory. </param>
	/// <param name="srcName"> Name of the source file. </param>
	/// <param name="oldName"> Name of the old. </param>
	/// <param name="extension"> The extension. </param>
	private static void RenameLibFile(string pluginDir, string subDir, string srcName, string oldName, string extension)
	{
		var oldDir  = Path.Combine(pluginDir, subDir);
		var oldFile = Path.Combine(oldDir,    oldName + extension);
		Debug.Log("[LSL BUILD Hook] Delete obsolete file: " + oldFile);
		File.Delete(oldFile);


		var srcFile = Path.Combine(oldDir,    srcName + extension);
		var dstFile = Path.Combine(pluginDir, LIB_LSL_NAME + extension);
		Debug.Log($"[LSL BUILD Hook] Renaming: {srcFile} to {dstFile}");
		File.Move(srcFile, dstFile);
	}
}
}
