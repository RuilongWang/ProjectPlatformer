using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour {
    public float speed = 100;
    private UIManager uiManager;
    
    #region monobehaviour methods
    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, uiManager.CurrentNodeSelected.pointerPosition.position, Time.deltaTime * speed);
    }

    private void OnEnable()
    {
        transform.position = uiManager.CurrentNodeSelected.pointerPosition.position;
    }
    #endregion monobehaviour methods
}
