using System.Collections;
using System.Collections.Generic;

public static class SettingsOverseer {
    public enum ScreenRatio
    {
        Screen4X3,
        Screen16X10,
        Screen16X9,
    }
    #region variables to save
    public static ScreenRatio currentlySelectedScreenRatio = ScreenRatio.Screen16X9;
    public static bool beginGameInFullScreenMode = true;
    #endregion variables to save

    #region const variables
    public const string SETTINGS_SAVE_FILE_NAME = "ProjectRobotGirlsSettingsSave.dat";
    public const string BACKUP_SETTINGS_SAVE_FILE_NAME = "backup_ProjectRobotGirlsSettingsSave.dat";//In case something goes wrong, we have a back file that we can load from
    #endregion const variables

    public static void LoadGameSettings()
    {

    }

    /// <summary>
    /// This will create a file that will save the game settings that will
    /// be used on game startup, such as resolution, full screen, sound volume, etc
    /// </summary>
    public static void SaveGameSettings()
    {

    }

    [System.Serializable]
    private class SettingsSaveData
    {
        public ScreenRatio currentlySelectedScreenRatio = ScreenRatio.Screen16X9;
        public bool beginGameInFullScreenMode = true;
    }
}
