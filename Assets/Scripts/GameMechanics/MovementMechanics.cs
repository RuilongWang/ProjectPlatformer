using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMechanics : MonoBehaviour {

    #region main variables
    public float maxRunSpeed = 20f;
    public float maxWalkSpeed = 9f;
    #endregion main variables

    #region set at runtime
    public float horizontalInput { get; set; }
    public float verticalInput { get; set; }
    #endregion set at runtime

    #region monobehaviour methods
    private void Update()
    {

    }
    #endregion monobehaviour methos
}
