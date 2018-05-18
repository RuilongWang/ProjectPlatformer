using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stats that are specific to the player. Items that we would want to save would be placed here
/// </summary>
[System.Serializable]
public class PlayerStats : CharacterStats {

    #region monobehaviour methods
    private void Start()
    {
        GameOverseer.Instance.player = this;
    }

    #endregion monobehaviour methods
}
