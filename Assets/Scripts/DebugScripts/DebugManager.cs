using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private bool DebugManagerOn;
    private Dictionary<DebugComponent.DebugType, bool> DebugTypesActive = new Dictionary<DebugComponent.DebugType, bool>();

    #region monobehavoiur methods
    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }   
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
    }
    #endregion monobehaviour methods
    /// <summary>
    /// 
    /// </summary>
    private void TurnOnOrOffDebugCategory()
    {

    }
}
