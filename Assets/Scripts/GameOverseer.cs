using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Keeps track of important references in the game
/// </summary>
public class GameOverseer : MonoBehaviour {
    public enum GameState { GamePlaying, GamePaused }

    public CharacterStats player;
    public GameState currentGameState { get; set; }

    #region save game variables
    private static string SAVE_GAME_FILE_NAME = "ProjectRobotGirlsSaveData.dat";
    #endregion save game variables


    #region monobehaviour methods
    private void Awake()
    {
        
    }
    #endregion monobehaviour methods

    /// <summary>
    /// Saves this particular instance of the game. Any changes before may be overwritten
    /// Takes in the name of the file that we are going to create
    /// </summary>
    public void SaveGameData(string saveGameDataName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_GAME_FILE_NAME);
    }

    /// <summary>
    /// Loads the selected instance of the gmae
    /// </summary>
    public void LoadGameData(string saveGameDataName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_GAME_FILE_NAME);
    }

    [System.Serializable]
    private struct SaveData
    {
        public float healthPoints;
    }
}
