using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class FastCompileManager : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string PrefKey        = "FastCompileManager_Enabled";
    private const string MenuPathToggle = "Tools/Fast Compile/Enable Manual Compile Mode";
    private const string MenuPathCompile = "Tools/Fast Compile/Compile Now";

    static bool locked;

    public static bool IsEnabled
    {
        get => EditorPrefs.GetBool(PrefKey, false);
        set
        {
            EditorPrefs.SetBool(PrefKey, value);
            ApplyState(value);
        }
    }

    static FastCompileManager()
    {
        ApplyState(IsEnabled);
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    private static void OnAfterAssemblyReload()
    {
        if (IsEnabled)
        {
            Lock();
        }
    }

    [MenuItem(MenuPathCompile + " _F5")]
    public static void CompileNow()
    {
        Debug.Log("[KF_FastCompile] Trying to force compile...");
        if (!IsEnabled)
        {
            Debug.Log("[KF_FastCompile] Manual Mode is disabled. Unity already compiles automatically.");
            return;
        }

        Debug.Log("[KF_FastCompile] Forcing Compilation, Domain Reload and Scene Reload...");

        Unlock();

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            string currentScenePath = SceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(currentScenePath))
            {
                EditorSceneManager.OpenScene(currentScenePath);
            }
        }

        AssetDatabase.Refresh(); 

        EditorUtility.RequestScriptReload();

    }

    [MenuItem(MenuPathToggle)]
    public static void ToggleManualCompile()
    {
        IsEnabled = !IsEnabled;
        Debug.Log($"[KF_FastCompile] Manual Compile Mode is now: {(IsEnabled ? "ENABLED" : "DISABLED")}");
    }

    [MenuItem(MenuPathToggle, true)]
    public static bool ToggleManualCompileValidate()
    {
        Menu.SetChecked(MenuPathToggle, IsEnabled);
        return true;
    }

    private static void ApplyState(bool enabled)
    {
        if (enabled)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }

    private static void Lock()
    {
        if (locked) return;
        EditorApplication.LockReloadAssemblies();
        locked = true;
    }

    private static void Unlock()
    {
        if (!locked) return;
        EditorApplication.UnlockReloadAssemblies();
        locked = false;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!IsEnabled) return;

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Unlock();
            AssetDatabase.Refresh();
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            Lock();
        }
    }

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (IsEnabled)
        {
            Debug.Log("[KF_FastCompile] Disabling Manual Compile Mode for Build. Will re-enable after build completes.");
            Unlock();
        }
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (IsEnabled)
        {
            Debug.Log("[KF_FastCompile] Build completed. Restoring Manual Compile Mode.");
            Lock();
        }
    }
}