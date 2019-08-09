using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inputs : MonoBehaviour
{

    public static bool dialogueCheck;
    public static Inputs instance;
    public static bool[] isHoldingJoystickDirections = new bool[4];
    public static bool[] hasPressedJoystickDirections = new bool[4];
    public static bool[] releasedJoystickDirections = new bool[4];

    public static void Disable(string button)
    {
        buttonDisabled[keyindices[button]] = true;
    }

    public static void Enable(string button)
    {
        buttonDisabled[keyindices[button]] = false;
    }
    public static bool[] buttonDisabled = new bool[8];
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
    public static Dictionary<string, int> keyindices = new Dictionary<string, int>(){
    {"up", 0},
    {"down",1},
    {"left",2},
    {"right",3},
    {"b",4},
    {"a",5},
    {"start",6},
    {"select",7}
    };
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
    public static bool pressedDpad()
    {
        return pressed("up") || pressed("down") || pressed("left") || pressed("right");
    }
    public static void disableDpad()
    {
        Disable("up");
        Disable("down");
        Disable("left");
        Disable("right");

    }
    public static void enableDpad()
    {
        Enable("up");
        Enable("down");
        Enable("left");
        Enable("right");

    }
    public void DisableForSeconds(string key, float time)
    {
        StartCoroutine(DisableForSecondsFunction(key,time));
    }
    public static IEnumerator DisableForSecondsFunction(string key, float time)
    {
        Disable(key);
        yield return new WaitForSeconds(time);
        Enable(key);
    }
    public static bool pressed(string button)
    {

        if (GameData.instance.isPaused) return false;

        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if ((Input.GetKeyDown(Inputs.inputs[index]) || Input.GetKeyDown(Inputs.controllerInputs[index])) || (index < 4 && hasPressedJoystickDirections[index]))
        {
            return true;
        }
        return false;

    }
    public static bool held(string button)
    {
        if (GameData.instance.isPaused) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (index < 4 && isHoldingJoystickDirections[index]) return true;
        if (Input.GetKey(Inputs.inputs[index]) || Input.GetKey(Inputs.controllerInputs[index])) return true;

        return false;

    }
    public static bool released(string button)
    {
        if (GameData.instance.isPaused) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if ((Input.GetKeyUp(Inputs.inputs[index]) || Input.GetKeyUp(Inputs.controllerInputs[index])) || (index < 4 && releasedJoystickDirections[index]))
        {
            return true;
        }

        return false;

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