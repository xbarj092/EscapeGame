using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
static class SceneAutoLoader
{
    static SceneAutoLoader()
    {
        EditorApplication.playmodeStateChanged += OnPlayModeChanged;
    }

    [MenuItem("File/Scene Autoload/Select Master Scene...")]
    private static void SelectMasterScene()
    {
        string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
        if (!string.IsNullOrEmpty(masterScene))
        {
            MasterScene = masterScene;
            LoadMasterOnPlay = true;
        }
    }

    [MenuItem("File/Scene Autoload/----------------------")]
    private static void Separator1()
    { }

    [MenuItem("File/Scene Autoload/Load Master On Play", true)]
    private static bool ShowLoadMasterOnPlay()
    {
        return !LoadMasterOnPlay;
    }
    [MenuItem("File/Scene Autoload/Load Master On Play")]
    private static void EnableLoadMasterOnPlay()
    {
        LoadMasterOnPlay = true;
    }

    [MenuItem("File/Scene Autoload/Don't Load Master On Play", true)]
    private static bool ShowDontLoadMasterOnPlay()
    {
        return LoadMasterOnPlay;
    }
    [MenuItem("File/Scene Autoload/Don't Load Master On Play")]
    private static void DisableLoadMasterOnPlay()
    {
        LoadMasterOnPlay = false;
    }

    [MenuItem("File/Scene Autoload/-------------------------")]
    private static void Separator2()
    { }

    [MenuItem("File/Scene Autoload/Load current scene additively", true)]
    private static bool ShowLoadAdditiveCurrentScene()
    {
        return !LoadAdditiveCurrentScene;
    }
    [MenuItem("File/Scene Autoload/Load current scene additively")]
    private static void EnableLoadAdditiveCurrentScene()
    {
        LoadAdditiveCurrentScene = true;
    }

    [MenuItem("File/Scene Autoload/Don't Load current scene additively", true)]
    private static bool ShowDontLoadAdditiveCurrentScene()
    {
        return LoadAdditiveCurrentScene;
    }
    [MenuItem("File/Scene Autoload/Don't Load current scene additively")]
    private static void DisableLoadAdditiveCurrentScene()
    {
        LoadAdditiveCurrentScene = false;
    }

    private static void OnPlayModeChanged()
    {
        if (!LoadMasterOnPlay)
        {
            return;
        }

        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            PreviousScene = EditorApplication.currentScene;
            if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
            {
                if (!EditorApplication.OpenScene(MasterScene))
                {
                    Debug.LogError(string.Format("error: scene not found: {0}", MasterScene));
                    EditorApplication.isPlaying = false;
                }
            }
            else
            {
                EditorApplication.isPlaying = false;
            }

            if (LoadAdditiveCurrentScene)
            {
                var sceneName = PreviousScene.Split(new[] { '/', '.' });
                Application.LoadLevelAdditive(sceneName[sceneName.Length - 2]);
            }
        }
        if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (!EditorApplication.OpenScene(PreviousScene))
            {
                Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
            }
        }
    }

    private const string cEditorPrefLoadMasterOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
    private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
    private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";
    private const string cEditorPrefLoadAdditiveCurrentScene = "SceneAutoLoader.LoadAdditiveCurrentScene";

    private static bool LoadMasterOnPlay
    {
        get { return EditorPrefs.GetBool(cEditorPrefLoadMasterOnPlay, false); }
        set { EditorPrefs.SetBool(cEditorPrefLoadMasterOnPlay, value); }
    }

    private static string MasterScene
    {
        get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Master.unity"); }
        set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
    }

    private static string PreviousScene
    {
        get { return EditorPrefs.GetString(cEditorPrefPreviousScene, EditorApplication.currentScene); }
        set { EditorPrefs.SetString(cEditorPrefPreviousScene, value); }
    }

    private static bool LoadAdditiveCurrentScene
    {
        get { return EditorPrefs.GetBool(cEditorPrefLoadAdditiveCurrentScene, false); }
        set { EditorPrefs.SetBool(cEditorPrefLoadAdditiveCurrentScene, value); }
    }
}
