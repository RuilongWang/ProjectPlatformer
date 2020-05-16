using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDebug : MonoBehaviour
{


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameDebugger.ActivateDeactivateDebugSetting(GameDebugger.DebugSetting.DRAW_COLLISION_BOXES);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameDebugger.ActivateDeactivateDebugSetting(GameDebugger.DebugSetting.DRAW_HITBOXES);
        }
    }
}
