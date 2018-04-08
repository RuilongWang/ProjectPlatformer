using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    #region monobehaviour methods
    private void Start()
    {
        GameOverseer.Instance.player = this;
    }

    #endregion monobehaviour methods
}
