using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Button {
    Up,
    Down,
    Left,
    Right,
    B,
    A,
    Start,
    Select
}


public class InputManager : MonoBehaviour
{

    public static bool dialogueCheck;
    public static InputManager instance;
    public static bool[] isHoldingJoystickDirections = new bool[4];
    public static bool[] hasPressedJoystickDirections = new bool[4];
    public static bool[] releasedJoystickDirections = new bool[4];

    public static bool[] buttonDisabled = new bool[8];

    //Keyboard button mappings
    public static List<KeyCode> inputs = new List<KeyCode>(new KeyCode[]{
        KeyCode.UpArrow, 
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.X,
            KeyCode.Z,
            KeyCode.Return,
            KeyCode.RightShift
    }
    );

    //Controller button mappings (Xbox 360 specific)
    public static List<KeyCode> controllerInputs = new List<KeyCode>(new KeyCode[]{
        KeyCode.Joystick1Button13,
            KeyCode.Joystick1Button14,
            KeyCode.Joystick1Button11,
            KeyCode.Joystick1Button12,
            KeyCode.Joystick1Button0,
            KeyCode.Joystick1Button1,
            KeyCode.Joystick1Button7,
            KeyCode.Joystick1Button6
    }
    );


    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < 4; i++) {
            isHoldingJoystickDirections[i] = false;
            hasPressedJoystickDirections[i] = false;
            releasedJoystickDirections[i] = false;
        }
    }

    public void Update()
    {
        UpdateControllerInput();
    }

    public static bool PressedDPad()
    {
        return Pressed(Button.Up) || Pressed(Button.Down) || Pressed(Button.Left) || Pressed(Button.Right);
    }

    public static void DisableDPad()
    {
        Disable(Button.Up);
        Disable(Button.Down);
        Disable(Button.Left);
        Disable(Button.Right);

    }

    public static void EnableDPad()
    {
        Enable(Button.Up);
        Enable(Button.Down);
        Enable(Button.Left);
        Enable(Button.Right);
    }

    public void DisableForSeconds(Button button, float time)
    {
        StartCoroutine(DisableForSecondsFunction(button,time));
    }

    public static IEnumerator DisableForSecondsFunction(Button button, float time)
    {
        Disable(button);
        yield return new WaitForSeconds(time);
        Enable(button);
    }

    public static bool Pressed(Button button){
        if (GameData.instance.isPaused) return false;

        int index = (int)button;
        if (buttonDisabled[index]) return false;
        if (button == Button.Start && dialogueCheck) return false;
        if ((Input.GetKeyDown(InputManager.inputs[index]) || Input.GetKeyDown(InputManager.controllerInputs[index])) || (index < 4 && hasPressedJoystickDirections[index])){
            return true;
        }

        return false;
    }

    public static bool Held(Button button){
        if (GameData.instance.isPaused) return false;

         int index = (int)button;
        if (buttonDisabled[index]) return false;
        if (button == Button.Start && dialogueCheck) return false;
        if (index < 4 && isHoldingJoystickDirections[index]) return true;
        if (Input.GetKey(InputManager.inputs[index]) || Input.GetKey(InputManager.controllerInputs[index])){
            return true;
        }

        return false;
    }

    public static bool Released(Button button){
        if (GameData.instance.isPaused) return false;
        
         int index = (int)button;
        if (buttonDisabled[index]) return false;
        if (button == Button.Start && dialogueCheck) return false;
        if ((Input.GetKeyUp(InputManager.inputs[index]) || Input.GetKeyUp(InputManager.controllerInputs[index])) || (index < 4 && releasedJoystickDirections[index])){
            return true;
        }

        return false;
    }

    public static void Disable(Button button){
        buttonDisabled[(int)button] = true;
    }

    public static void Enable(Button button){
        buttonDisabled[(int)button] = false;
    }
    
    public void UpdateControllerInput()
    {
        if (Input.GetAxisRaw("JoystickY") > 0.5f || Input.GetAxisRaw("DpadY") > 0.5f)
        {
            if (hasPressedJoystickDirections[0])
            {
                isHoldingJoystickDirections[0] = true;
                hasPressedJoystickDirections[0] = false;
            }
            if (!isHoldingJoystickDirections[0]) hasPressedJoystickDirections[0] = true;
        }
        else
        {
            if (isHoldingJoystickDirections[0] || hasPressedJoystickDirections[0]) releasedJoystickDirections[0] = true;
            else releasedJoystickDirections[0] = false;
            isHoldingJoystickDirections[0] = false;
            hasPressedJoystickDirections[0] = false;
        }
        if (Input.GetAxisRaw("JoystickY") < -0.5f || Input.GetAxisRaw("DpadY") < -0.5f)
        {
            if (hasPressedJoystickDirections[1])
            {
                isHoldingJoystickDirections[1] = true;
                hasPressedJoystickDirections[1] = false;
            }
            if (!isHoldingJoystickDirections[1]) hasPressedJoystickDirections[1] = true;
        }
        else
        {
            if (isHoldingJoystickDirections[1] || hasPressedJoystickDirections[1]) releasedJoystickDirections[1] = true;
            else releasedJoystickDirections[1] = false;
            isHoldingJoystickDirections[1] = false;
            hasPressedJoystickDirections[1] = false;
        }
        if (Input.GetAxisRaw("JoystickX") < -0.5f || Input.GetAxisRaw("DpadX") < -0.5f)
        {
            if (hasPressedJoystickDirections[2])
            {
                isHoldingJoystickDirections[2] = true;
                hasPressedJoystickDirections[2] = false;
            }
            if (!isHoldingJoystickDirections[2]) hasPressedJoystickDirections[2] = true;
        }
        else
        {
            if (isHoldingJoystickDirections[2] || hasPressedJoystickDirections[2]) releasedJoystickDirections[2] = true;
            else releasedJoystickDirections[2] = false;
            isHoldingJoystickDirections[2] = false;
            hasPressedJoystickDirections[2] = false;
        }
        if (Input.GetAxisRaw("JoystickX") > 0.5f || Input.GetAxisRaw("DpadX") > 0.5f)
        {
            if (hasPressedJoystickDirections[3])
            {
                isHoldingJoystickDirections[3] = true;
                hasPressedJoystickDirections[3] = false;
            }
            if (!isHoldingJoystickDirections[3]) hasPressedJoystickDirections[3] = true;
        }
        else
        {
            if (isHoldingJoystickDirections[3] || hasPressedJoystickDirections[3]) releasedJoystickDirections[3] = true;
            else releasedJoystickDirections[3] = false;
            isHoldingJoystickDirections[3] = false;
            hasPressedJoystickDirections[3] = false;
        }
    }
}