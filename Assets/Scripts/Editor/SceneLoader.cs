using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneLoader
{
    private static string mainmenuScenePath = "Assets/Scenes/Menu.unity";
    private static string GameplayScenePath = "Assets/Scenes/";

    [MenuItem("SceneHandler/Open Splash Scene _F1")]
    static void OpenMainMenuScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(mainmenuScenePath);
        }
    }

    [MenuItem("SceneHandler/Open Butterfly Gameplay Scene _F3")]
    static void OpenButterflyGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath+ "Butterfly.unity");
        }
    }

    [MenuItem("SceneHandler/Open Safari Gameplay Scene #F3")]
    static void OpenSafariGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Safari.unity");
        }
    }

    [MenuItem("SceneHandler/Open Insects Gameplay Scene %F3")]
    static void OpenPondGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Insects.unity");
        }
    }

    [MenuItem("SceneHandler/Open Reptile Gameplay Scene _F4")]
    static void OpenReptileGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Reptile.unity");
        }
    }

    [MenuItem("SceneHandler/Open Arctic Gameplay Scene #F4")]
    static void OpenArcticGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Arctic.unity");
        }
    }

    [MenuItem("SceneHandler/Open Aquarium Gameplay Scene %F4")]
    static void OpenAquariumGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Aquarium.unity");
        }
    }

    [MenuItem("SceneHandler/Open Farm Gameplay Scene _F6")]
    static void OpenFarmGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Farm.unity");
        }
    }

    [MenuItem("SceneHandler/Open Singapore Gameplay Scene #F6")]
    static void OpenSingaporeGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Singapore.unity");
        }
    }

    [MenuItem("SceneHandler/Open Mushroom Gameplay Scene %F6")]
    static void OpenMushroomsGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Mushrooms.unity");
        }
    }

    [MenuItem("SceneHandler/Open Garden Gameplay Scene #F6")]
    static void OpenGardenGameplayScene()
    {
        if (!EditorApplication.isPlaying && EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GameplayScenePath + "Garden.unity");
        }
    }

    [MenuItem("SceneHandler/PlayStop _F5")]
    private static void PlayStopButton()
    {
        if (!EditorApplication.isPlaying)
        {
            bool value = EditorApplication.SaveCurrentSceneIfUserWantsTo();
            if (value)
            {
                EditorApplication.OpenScene(mainmenuScenePath);
                EditorApplication.ExecuteMenuItem("Edit/Play");
            }
        }

    }

   [MenuItem("SceneHandler/Pause %F5")]
    private static void PauseButton()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExecuteMenuItem("Edit/Pause");
        }
    }
}