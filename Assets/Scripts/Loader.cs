using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{ 
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void LoadScene(Scene scene)
    {
        Loader.targetScene = scene;
        SceneManager.LoadScene(Loader.Scene.LoadingScene.ToString());
    }

    public static void LoadCallback()
    {
        SceneManager.LoadScene(Loader.targetScene.ToString());
    }
}
