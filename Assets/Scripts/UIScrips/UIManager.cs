using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


/// <summary>
/// UI Managers are used primarily for menus that invoolve selectable options.
/// This should be used in options screens, pause menus or other selection menus
/// </summary>
public class UIManager : MonoBehaviour {
    public enum UIDirection { Up, Down, Left, Right }
    private const float Minimum_Axis_Threshold = 0.5f;
    public List<UINode> allAttachedUINodes { get; private set; }
    [Tooltip("This is the default node that will be selected upon start up of this ui menu. If you want it to be reset to this ui node, use the reset method when entering")]
    public UINode defaultNode;
    private UINode currentNodeSelected;
    public UINode CurrentNodeSelected
    {
        get
        {
            if (currentNodeSelected == null)
            {
                return defaultNode;
            }
            else
            {
                return currentNodeSelected;
            }
        }
    }
      
    private bool autoScrollActive;

    #region monobehaviour methods
    private void Awake()
    {
        allAttachedUINodes = new List<UINode>();
        currentNodeSelected = defaultNode;
    }

    private void Update()
    {
        if (autoScrollActive)
        {
            return;
        }
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        if (vInput > Minimum_Axis_Threshold)
        {
            StartCoroutine(ScrollUIElements(UIDirection.Up));
        }
        else if (vInput < -Minimum_Axis_Threshold)
        {
            StartCoroutine(ScrollUIElements(UIDirection.Down));
        }
        else if (hInput > Minimum_Axis_Threshold)
        {
            StartCoroutine(ScrollUIElements(UIDirection.Right));
        }
        else if (hInput < -Minimum_Axis_Threshold)
        {
            StartCoroutine(ScrollUIElements(UIDirection.Left));
        }

        if (Input.GetButtonDown("Submit"))
        {
            currentNodeSelected.OnNodeSelected();
        }
    }
    #endregion monobehaviour methods

    #region scrolling logic
    private IEnumerator ScrollUIElements(UIDirection uiDirection)
    {
        autoScrollActive = true;
        float timeBeforeAutoScroll = .6f;
        MoveToNextElemt(uiDirection);

        while (timeBeforeAutoScroll > 0)
        {
            
            if (!IsScrollActive(uiDirection))
            {
                autoScrollActive = false;
                yield break;
            }
            timeBeforeAutoScroll -= Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }

        timeBeforeAutoScroll = .07f;
        float nextScrollTimer = 0;

        while (IsScrollActive(uiDirection))
        {
            if (nextScrollTimer <= 0)
            {
                nextScrollTimer = timeBeforeAutoScroll;
                MoveToNextElemt(uiDirection);
            }
            yield return new WaitForEndOfFrame();
            nextScrollTimer -= Time.unscaledDeltaTime;
        }
        autoScrollActive = false;   
    }

    private void MoveToNextElemt(UIDirection direction)
    {
        UINode uiNodeToMoveTo = null;
        switch (direction)
        {
            case UIDirection.Up:
                uiNodeToMoveTo = currentNodeSelected.GetNodeUp();
                break;
            case UIDirection.Down:
                uiNodeToMoveTo = currentNodeSelected.GetNodeDown();
                break;
            case UIDirection.Right:
                uiNodeToMoveTo = currentNodeSelected.GetNodeRight();
                break;
            case UIDirection.Left:
                uiNodeToMoveTo = currentNodeSelected.GetNodeLeft();
                break;
        }

        if (uiNodeToMoveTo == null)
        {
            return;
        }
        this.currentNodeSelected = uiNodeToMoveTo;
    }

    private bool IsScrollActive(UIDirection direction)
    {
        if (!this.gameObject.activeSelf)
        {
            return false;
        }
        switch (direction)
        {
            case UIDirection.Up:
                if (Input.GetAxisRaw("Vertical") < Minimum_Axis_Threshold)
                {
                    return false;
                }
                return true;
            case UIDirection.Down:
                if (Input.GetAxisRaw("Vertical") > -Minimum_Axis_Threshold)
                {
                    return false;
                }
                return true;
            case UIDirection.Right:
                if (Input.GetAxisRaw("Horizontal") < Minimum_Axis_Threshold)
                {
                    return false;
                }
                return true;
            case UIDirection.Left:
                if (Input.GetAxisRaw("Horizontal") > -Minimum_Axis_Threshold)
                {
                    return false;
                }
                return true;
        }
        return false;
    }
    #endregion scrolling logic
}
