using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    #region main variables
    private const string NEW_GAME_SCENE_NAME = "MainScene";
    #endregion main variables

    #region event methods
    public void OnNewGamePressed()
    {
        SceneManager.LoadScene(NEW_GAME_SCENE_NAME, LoadSceneMode.Single);
    }

    public void OnLoadGamePressed()
    {
        print("Load Game");
    }

    public void OnOptionsPressed()
    {
        print("Options");
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
    #endregion event methods
}
