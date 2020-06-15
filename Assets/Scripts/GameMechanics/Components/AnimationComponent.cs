using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    protected Animator AssociatedAnimator;
    private bool ShouldUpdateAnimatorManually { get { return !AssociatedAnimator.enabled; } }

    #region monobehaviour methods
    protected virtual void Awake()
    {
        AssociatedAnimator = GetComponent<Animator>();
        if (AssociatedAnimator.enabled)
        {
            Debug.LogWarning("You are using the Animator in debug mode. Please be sure to disable it when you are done.");
        }
    }


    protected virtual void Update()
    {
        if (ShouldUpdateAnimatorManually) AssociatedAnimator.Update(GameOverseer.DELTA_TIME);
    }

    #endregion monobehaviour methods
}
