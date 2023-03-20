using UnityEditor;

namespace StarterAssets
{
    public static class ScriptingDefineUtils
    {
		[System.Obsolete]
		public static bool CheckScriptingDefine(string scriptingDefine)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return defines.Contains(scriptingDefine);
        }

		[System.Obsolete]
		public static void SetScriptingDefine(string scriptingDefine)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!defines.Contains(scriptingDefine))
            {
                defines += $";{scriptingDefine}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
            }
        }

		[System.Obsolete]
		public static void RemoveScriptingDefine(string scriptingDefine)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (defines.Contains(scriptingDefine))
            {
                string newDefines = defines.Replace(scriptingDefine, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefines);
            }
        }
    }
}