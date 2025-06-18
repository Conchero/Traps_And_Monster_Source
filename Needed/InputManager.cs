using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static InputManager instance = null;
    public static InputManager Instance { get => instance; }

    public Dictionary<int, float> sensitivityByDevice = new Dictionary<int, float>();


    public bool m_freeMousse;

    public struct Control
    {
        public string nameDevice;
        public string nameControl;
        public InputControl control;

        public Control(string _nameDevice, string _nameControl, InputControl _control)
        {
            this.nameDevice = _nameDevice;
            this.nameControl = _nameControl;
            this.control = _control;
        }
    }

    List<Control> listInput = new List<Control>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        if (!m_freeMousse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        AssignSensitivity();
    }

    public void AssignSensitivity()
    {
        foreach (InputDevice iD in InputSystem.devices)
        {
//            Debug.Log(iD.name + iD.GetHashCode());
            //  Debug.Log(iD.name);
            sensitivityByDevice.Add(iD.GetHashCode(), 3);
        }
    }

    public void ClearSensitivity()
    {
        sensitivityByDevice.Clear();
    }

    public float GetSensitivity(int _hashCode)
    {
        return sensitivityByDevice[_hashCode];
    }

    public void SetSensitivity(int _hashCode, float _sensitivity)
    {
        sensitivityByDevice[_hashCode] = _sensitivity;
    }

    public void AssignControl(string _device)
    {
        switch (InputSystem.GetDevice(_device).layout)
        {
            case "XInputControllerWindows":
                listInput.Add(new Control(_device, "Start", InputSystem.GetDevice(_device).GetChildControl("start")));
                //Bas
                listInput.Add(new Control(_device, "A", InputSystem.GetDevice(_device).GetChildControl("buttonSouth")));
                //Haut
                listInput.Add(new Control(_device, "Y", InputSystem.GetDevice(_device).GetChildControl("buttonNorth")));
                //gauche
                listInput.Add(new Control(_device, "X", InputSystem.GetDevice(_device).GetChildControl("buttonWest")));
                //Droite
                listInput.Add(new Control(_device, "B", InputSystem.GetDevice(_device).GetChildControl("buttonEast")));
                listInput.Add(new Control(_device, "LeftStick", InputSystem.GetDevice(_device).GetChildControl("leftStick")));
                listInput.Add(new Control(_device, "RightStick", InputSystem.GetDevice(_device).GetChildControl("rightStick")));
                listInput.Add(new Control(_device, "LeftTrigger", InputSystem.GetDevice(_device).GetChildControl("leftTrigger")));
                listInput.Add(new Control(_device, "RightTrigger", InputSystem.GetDevice(_device).GetChildControl("rightTrigger")));
                listInput.Add(new Control(_device, "LeftStickPress", InputSystem.GetDevice(_device).GetChildControl("leftStickPress")));
                listInput.Add(new Control(_device, "RightStickPress", InputSystem.GetDevice(_device).GetChildControl("rightStickPress")));
                listInput.Add(new Control(_device, "LeftBumper", InputSystem.GetDevice(_device).GetChildControl("leftShoulder")));
                listInput.Add(new Control(_device, "RightBumper", InputSystem.GetDevice(_device).GetChildControl("rightShoulder")));
                listInput.Add(new Control(_device, "Pad", InputSystem.GetDevice(_device).GetChildControl("dpad")));
                listInput.Add(new Control(_device, "Select", InputSystem.GetDevice(_device).GetChildControl("select")));
                break;
            case "DualShock4GamepadHID":
                listInput.Add(new Control(_device, "Start", InputSystem.GetDevice(_device).GetChildControl("start")));
                listInput.Add(new Control(_device, "A", InputSystem.GetDevice(_device).GetChildControl("buttonSouth")));
                listInput.Add(new Control(_device, "Y", InputSystem.GetDevice(_device).GetChildControl("buttonNorth")));
                listInput.Add(new Control(_device, "X", InputSystem.GetDevice(_device).GetChildControl("buttonWest")));
                listInput.Add(new Control(_device, "B", InputSystem.GetDevice(_device).GetChildControl("buttonEast")));
                listInput.Add(new Control(_device, "LeftStick", InputSystem.GetDevice(_device).GetChildControl("leftStick")));
                listInput.Add(new Control(_device, "RightStick", InputSystem.GetDevice(_device).GetChildControl("rightStick")));
                listInput.Add(new Control(_device, "LeftTrigger", InputSystem.GetDevice(_device).GetChildControl("leftTrigger")));
                listInput.Add(new Control(_device, "RightTrigger", InputSystem.GetDevice(_device).GetChildControl("rightTrigger")));
                listInput.Add(new Control(_device, "LeftStickPress", InputSystem.GetDevice(_device).GetChildControl("leftStickPress")));
                listInput.Add(new Control(_device, "RightStickPress", InputSystem.GetDevice(_device).GetChildControl("rightStickPress")));
                listInput.Add(new Control(_device, "LeftBumper", InputSystem.GetDevice(_device).GetChildControl("leftShoulder")));
                listInput.Add(new Control(_device, "RightBumper", InputSystem.GetDevice(_device).GetChildControl("rightShoulder")));
                listInput.Add(new Control(_device, "Pad", InputSystem.GetDevice(_device).GetChildControl("dpad")));
                listInput.Add(new Control(_device, "Select", InputSystem.GetDevice(_device).GetChildControl("select")));
                break;
            case "Keyboard":
                // Join a party
                listInput.Add(new Control(_device, "Start", InputSystem.GetDevice(_device).GetChildControl("space")));
                listInput.Add(new Control(_device, "A", InputSystem.GetDevice(_device).GetChildControl("enter")));
                listInput.Add(new Control(_device, "B", InputSystem.GetDevice(_device).GetChildControl("escape")));
                listInput.Add(new Control(_device, "RightStickPress", InputSystem.GetDevice(_device).GetChildControl("Q")));
                listInput.Add(new Control(_device, "X", InputSystem.GetDevice(_device).GetChildControl("E")));
                listInput.Add(new Control(_device, "Select", InputSystem.GetDevice(_device).GetChildControl("P")));
                listInput.Add(new Control(_device, "LeftBumper", InputSystem.GetDevice(_device).GetChildControl("T")));
                listInput.Add(new Control(_device, "RightBumper", InputSystem.GetDevice(_device).GetChildControl("Y")));
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isPressed(string _device, string _nameControl, bool testIsPressed)
    {
        bool test = false;
        for (int i = 0; i < listInput.Count; i++)
        {
            if (testIsPressed)
            {
                if (listInput[i].nameDevice == _device
                && listInput[i].nameControl == _nameControl
                && listInput[i].control.IsPressed())
                {
                    test = true;
                }
            }
            else if (!testIsPressed)
            {
                if (listInput[i].nameDevice == _device
                && listInput[i].nameControl == _nameControl
                && !listInput[i].control.IsPressed())
                {
                    test = true;
                }
            }
        }

        return test;
    }

    public Vector2 ValueJoyStick(string _device, string _nameControl)
    {
        Vector2 test = new Vector2();
        if(_device != null)
        {
            for (int i = 0; i < listInput.Count; i++)
            {
                if (listInput[i].nameDevice == _device
                    && listInput[i].nameControl == _nameControl)
                {
                    test = (Vector2)listInput[i].control.ReadValueAsObject();
                }
            }
        }

        return test;
    }

    public Vector2 ValuePAD(string _device, string _nameControl)
    {
        Vector2 test = new Vector2();
		if(_device != null)
		{
			for (int i = 0; i < listInput.Count; i++)
			{
				if (listInput[i].nameDevice == _device
					&& listInput[i].nameControl == _nameControl)
				{
					test = (Vector2)listInput[i].control.ReadValueAsObject();
				}
			}
		}

        return test;
    }

    public string GetLayoutDevice(string _device)
    {
        return InputSystem.GetDevice(_device).layout;
    }
}
