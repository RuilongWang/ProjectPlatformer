using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region const button names


    public enum ButtonAction
    {
        JUMP,
    }
    #endregion const button names
    private Dictionary<ButtonAction, BufferedButtonInput> buttonDictionary = new Dictionary<ButtonAction, BufferedButtonInput>();

    #region monobehaviour methods
    private void Awake()
    {
        foreach (ButtonAction buttonAction in System.Enum.GetValues(typeof(ButtonAction)))
        {
            buttonDictionary.Add(buttonAction, new BufferedButtonInput());
        }
    }

    
    #endregion monobehaviour methods
    /// <summary>
    /// If this method is called and an action is return to be true,
    /// that action will then be set to false immediately after. Game Actions
    /// should only need to be called once per frame.
    /// </summary>
    /// <param name="buttonAction"></param>
    public bool GetButtonBufferedDown(ButtonAction buttonAction)
    {
        if (buttonDictionary[buttonAction].isButtonBufferedDown)
        {
            buttonDictionary[buttonAction].ResetButtonBufferedDown();
            return true;
        }
        return false;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonAction"></param>
    public bool GetButtonBufferedUp(ButtonAction buttonAction)
    {
        if (buttonDictionary[buttonAction].isButtonBufferedUp)
        {
            buttonDictionary[buttonAction].ResetButtonBufferedUp();
            return true;
        }
        return false;
    }

    public bool GetButton(ButtonAction buttonAction)
    {
        return buttonDictionary[buttonAction].button;
    }

    public bool GetButtonDown(ButtonAction buttonAction)
    {
        return buttonDictionary[buttonAction].buttonDown;
    }

    public bool GetButtonUp(ButtonAction buttonAction)
    {
        return buttonDictionary[buttonAction].buttonUp;
    }

    public float GetHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetVertical()
    {
        return Input.GetAxisRaw("Vertical");
    }
    #region get button methods

    #endregion get button methods
    /// <summary>
    /// Class to help with buffeeing button inputs. Players need a little leeway in their button presses
    /// so this should be fine. Also have access to the basic Unity button methods from here as well
    /// </summary>
    private class BufferedButtonInput
    {
        private const float BUTTON_BUFFER_TIME = 0.117f;//About a 7 frame buffer


        public ButtonAction buttonAction;
        public string buttonName;
        private float buttonDownTimer;
        private float buttonUpTimer;

        #region button status variables
        public bool isButtonBufferedDown
        {
            get
            {
                return buttonDownTimer > 0;
            }
        }

        public bool isButtonBufferedUp
        {
            get
            {
                return buttonUpTimer > 0;
            }
        }

        public bool buttonUp
        {
            get
            {
                return Input.GetButtonUp(buttonName);
            }
        }

        public bool buttonDown
        {
            get
            {
                return Input.GetButtonDown(buttonName);
            }
        }

        public bool button
        {
            get
            {
                return Input.GetButton(buttonName);
            }
        }
        #endregion button status variables

        public void UpdateButtonPress(float deltaTime)
        {
            buttonDownTimer = Mathf.MoveTowards(buttonDownTimer, 0, deltaTime);
            buttonUpTimer = Mathf.MoveTowards(buttonUpTimer, 0, deltaTime);

            if (buttonDown)
            {
                buttonDownTimer = BUTTON_BUFFER_TIME;
            }
            if (buttonUp)
            {
                buttonUpTimer = BUTTON_BUFFER_TIME;
            }
        }

        public void ResetButtonBufferedDown()
        {
            buttonDownTimer = 0;
        }

        public void ResetButtonBufferedUp()
        {
            buttonUpTimer = 0;
        }
    }
}
