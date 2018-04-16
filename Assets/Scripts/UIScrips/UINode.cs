using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINode : MonoBehaviour {
    [SerializeField]
    private UINode nodeUp;
    [SerializeField]
    private UINode nodeDown;
    [SerializeField]
    private UINode nodeLeft;
    [SerializeField]
    private UINode nodeRight;

    private UIManager uiManager;
    public bool uiNodeActive { get; private set; }

    private bool uiNodeSearched = false;

    #region monobehaviour methods
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        uiManager.allAttachedUINodes.Add(this);
    }
    #endregion monobehaviour methods

    public void SetNodeActive(bool uiNodeActive)
    {
        this.uiNodeActive = uiNodeActive;
    }

    #region get methods
    private UINode GetNodeHelper(UINode nodeToCheck)
    {
        return null;
    }

    /// <summary>
    /// Gets a ui element that is above this element
    /// </summary>
    /// <returns></returns>
    public UINode GetNodeUp()
    {
        if (nodeUp == null || uiNodeSearched)
        {
            uiNodeSearched = false;
            return null;
        }
        uiNodeSearched = true;
        UINode uiNodeToReturn = nodeUp;
        if (!nodeUp.uiNodeActive)
        {
            uiNodeToReturn = nodeUp.GetNodeUp();
        }
        uiNodeSearched = false;

        return uiNodeToReturn;
    }

    /// <summary>
    /// Gets a ui element that is below this ui element
    /// </summary>
    /// <returns></returns>
    public UINode GetNodeDown()
    {
        if (nodeDown == null || uiNodeSearched)
        {
            uiNodeSearched = false;
            return null;
        }
        uiNodeSearched = true;
        UINode uiNodeToReturn = nodeDown;
        if (!nodeDown.uiNodeActive)
        {
            uiNodeToReturn = nodeDown.GetNodeDown();
        }
        uiNodeSearched = false;

        return uiNodeToReturn;
    }

    /// <summary>
    /// Gets a ui elementt that is to the right of this ui element
    /// </summary>
    /// <returns></returns>
    public UINode GetNodeRight()
    {
        if (nodeRight == null || uiNodeSearched)
        {
            uiNodeSearched = false;
            return null;
        }
        uiNodeSearched = true;
        UINode uiNodeToReturn = nodeRight;
        if (!nodeRight.uiNodeActive)
        {
            uiNodeToReturn = nodeRight.GetNodeRight();
        }
        uiNodeSearched = false;

        return uiNodeToReturn;
    }

    /// <summary>
    /// Gets a ui element that is to the left of this element
    /// </summary>
    /// <returns></returns>
    public UINode GetNodeLeft()
    {
        if (nodeLeft == null || uiNodeSearched)
        {
            uiNodeSearched = false;
            return null;
        }
        uiNodeSearched = true;
        UINode uiNodeToReturn = nodeLeft;
        if (!nodeLeft.uiNodeActive)
        {
            uiNodeToReturn = nodeLeft.GetNodeLeft();
        }
        uiNodeSearched = false;

        return uiNodeToReturn;
    }
    #endregion get methods
}
