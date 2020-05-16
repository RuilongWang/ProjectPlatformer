using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDebugger
{
    public enum DebugSetting : uint
    {
        DRAW_COLLISION_BOXES = 0x00000001,
        DRAW_HITBOXES = 0X00000002,
    }

    public static bool bIsDebugInUse = true;
    private static uint CurrentlyActiveDebugSettings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DebugSettingToCheck"></param>
    /// <returns></returns>
    public static bool IsDebugSettingActive(DebugSetting DebugSettingToCheck)
    {
        if (!bIsDebugInUse) return false;

        return (CurrentlyActiveDebugSettings & (uint)DebugSettingToCheck) != 0;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="DebugSettingToToggle"></param>
    public static void ActivateDeactivateDebugSetting(DebugSetting DebugSettingToToggle)
    {
        if (IsDebugSettingActive(DebugSettingToToggle))
            CurrentlyActiveDebugSettings |= (uint)DebugSettingToToggle;
        else
            CurrentlyActiveDebugSettings &= ~(uint)DebugSettingToToggle;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DebugSettingToDeactivate"></param>
    public static void DeactivateDebugSetting(DebugSetting DebugSettingToDeactivate)
    {
        
    }
}
