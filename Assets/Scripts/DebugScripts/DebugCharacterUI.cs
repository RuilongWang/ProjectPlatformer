using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor.MemoryProfiler;

/// <summary>
/// Character UI Canvas component that can be attached to a character actor  that can display various data
/// </summary>
public class DebugCharacterUI : DebugComponent
{
    [SerializeField]
    [Tooltip("Base canvas that will be used for debugging")]
    private Canvas DebugCharacterCanvas;
    [SerializeField]
    [Tooltip("Slider that shows current health of character")]
    private Slider DebugCharacterHealthSlider;
    [SerializeField]
    [Tooltip("Display Text to show physics stats")]
    private Text DebugPhysicsDisplayText;

    private GamePlayCharacter AssociatedCharacter;


    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();

        if (!Application.isEditor)//We do not want debug components to be placed in our characters during release.
        {
            Destroy(this.gameObject);
            return;
        }
        AssociatedCharacter = GetComponentInParent<GamePlayCharacter>();

        AssociatedCharacter.Delegate_HealthUpdated += UpdateHealthBarSlider;
        UpdateHealthBarSlider();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AssociatedCharacter.Delegate_HealthUpdated -= UpdateHealthBarSlider;
    }

    protected virtual void Update()
    {
        UpdatePhysicsDisplayStats();
    }
    #endregion monobehaviour methods

    #region visually update debug ui
    /// <summary>
    /// Visually updates our UI health bar to the current health of the character that is assigned to this UI
    /// </summary>
    private void UpdateHealthBarSlider()
    {
        DebugCharacterHealthSlider.value = AssociatedCharacter.CharacterCurrentHealth / AssociatedCharacter.CharacterMaxHealth;
    }

    /// <summary>
    /// Updates the Display text for physics information
    /// </summary>
    private void UpdatePhysicsDisplayStats()
    {
        string PhysicsDataString = "";
        PhysicsDataString += string.Format("Vel X: {0} Y: {1}\n", AssociatedCharacter.Rigid.Velocity.x.ToString("0.00"), 
            AssociatedCharacter.Rigid.Velocity.y.ToString("0.00"));
        DebugPhysicsDisplayText.text = PhysicsDataString;
    }
    #endregion visually update debug ui


    #region virtual methods
    /// <summary>
    /// Call this method when debug functionality is tuned on or if this individual component has been activated
    /// </summary>
    public override void OnBeginDebug()
    {
        DebugCharacterCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Call this method when debug functionality has been turned off or if this individual component have deactivated
    /// </summary>
    public override void OnEndDebug()
    {
        DebugCharacterCanvas.gameObject.SetActive(false);
    }

    public override DebugComponent.DebugType GetDebugType() { return DebugType.CHARACTER; }
    #endregion virtual methods
}
