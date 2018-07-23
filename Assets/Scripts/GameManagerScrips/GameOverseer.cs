using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Keeps track of important references in the game
/// </summary>
public class GameOverseer : MonoBehaviour {
    public enum GameState { GamePlaying, GamePaused }

    #region static variables
    private static GameOverseer instance;
    public static GameOverseer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameOverseer>();
            }
            return instance;
        }
    }
    #endregion static variables

    public PlayerStats player { get; set; }
    public GameState currentGameState { get; set; }

    #region debug variables
    [Header("Debug Game Toggles")]
    //Displays the player's currents stats in a UI window in the top right corner
    public bool playerStatsWindow = false;
    //Displays active hitboxes visually in game
    public bool displayHitboxesVisually = false;
    //Changes the color of the hitboxes, based on their current state
    public bool displayHitboxStates = false;
    #endregion debug variables

    #region save game variables
    private static string SAVE_GAME_FILE_NAME;
    private static string BACKUP_SAVE_GAME_FILE_NAME;
    #endregion save game variables

    #region game settings variables

    #endregion game settings variables


    #region monobehaviour methods
    private void Awake()
    {
        SAVE_GAME_FILE_NAME = Path.Combine(Application.persistentDataPath, "ProjectRobotGirlsSaveData.dat");
        BACKUP_SAVE_GAME_FILE_NAME = Path.Combine(Application.persistentDataPath, "backup_ProjectRobotGirlsSaveData.dat");

        instance = this;
    }

    private void Start()
    {

        
    }

    private void OnApplicationQuit()
    {
        
    }
    #endregion monobehaviour methods

    #region save game methods
    public bool gameCurrentlySaving { get; private set; }


    /// <summary>
    /// Saves this particular instance of the game. Any changes before may be overwritten
    /// Takes in the name of the file that we are going to create
    /// </summary>
    public void SaveGameData(string saveGameDataName, int attemptCount = 0)
    {
        if (gameCurrentlySaving)
        {
            Debug.Log("Game currently saving. Please wait until it is complete.");
            return;
        }
        if (attemptCount >= 3)
        {
            Debug.LogError("We have attempted to save 3 times in a row... There may be something seriously wrong that you need to look into");
            return;
        }
        gameCurrentlySaving = true;
        string filePath = SAVE_GAME_FILE_NAME;
        FileStream fs = new FileStream(filePath, FileMode.Open);
        SaveData saveData = new SaveData();

        saveData.sceneName = SceneManager.GetActiveScene().name;

        saveData.healthPoints = player.currentHealth;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, saveData);

            fs.Close();
            fs = null;
            File.Copy(SAVE_GAME_FILE_NAME, BACKUP_SAVE_GAME_FILE_NAME);

        }
        catch (SerializationException e)
        {
            Debug.LogWarning("There was a problem saving our game data");
            Debug.LogError(e.StackTrace);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
            
        }
        gameCurrentlySaving = false;
    }

    public void ThreadedGameSave()
    {

    }

    /// <summary>
    /// Loads the selected instance of the gmae
    /// </summary>
    public void LoadGameData(string saveGameDataName)
    {
        try
        {
            SaveData saveData = LoadSaveDataBinaryFile(SAVE_GAME_FILE_NAME);
            if (saveData == null)
            {
                saveData = LoadSaveDataBinaryFile(BACKUP_SAVE_GAME_FILE_NAME);
            }
            if (saveData == null)
            {
                Debug.LogWarning("There was no save file found.");
                return;
            }
        }
        catch
        {

        }
    }

    private SaveData LoadSaveDataBinaryFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        FileStream fs = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            fs = new FileStream(path, FileMode.Open);
            SaveData sd = (SaveData)bf.Deserialize(fs);
            fs.Close();
            fs = null;
            return sd;
        }
        catch (System.Exception e)
        {
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
            Debug.LogError(path + "<- There was an error loading from this path. It may be corrupted.");
            Debug.LogError(e.StackTrace);
        }
        return null;
    }


    /// <summary>
    /// This class will hold important data that we will want to save about the game
    /// </summary>
    [System.Serializable]
    private class SaveData
    {
        //Name of the level or scene that this save took place
        public string sceneName;

        public float healthPoints;

        #region Game Settings Variables to Save
        public bool startGameInFullScreen = true;
        #endregion Game Settings Variables to Save
    }
    #endregion save game methods

    #region debug methods
    private void UpdateDebugSettings()
    {

    }
    #endregion debug methods
}