using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    public Image checkMarkHandLeft;
    public Image checkMarkHandRight;
    public Image checkMarkArrow;
    public Image checkMarkJoystick;

    public void Start()
    {
        checkMarkHandLeft.enabled = !GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_MODE);
        checkMarkHandRight.enabled = !GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_MODE);
        checkMarkArrow.enabled = !GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_HAND);
        checkMarkJoystick.enabled = !GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_HAND);
    }
    public bool JoystickMode
    {
        get
        {
            return GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, 0);
        }
        set
        {
            GamePrefs.SetBool(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, value);
            GameManager.instance.SetControls();
        }
    }


    public void ChangeModeDirection(int mode)
    {
        PlayerPrefs.SetInt(GameUtils.PREFS_JOYSTICK_CONTROL_HAND, mode);
        GameManager.instance.SetControls();
    }

    public void ChangeModeJoystick(int mode)
    {
        PlayerPrefs.SetInt(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, mode);
        GameManager.instance.SetControls();
    }


    /* public void OpenControls()
     {
         leftJoystick.GetComponent<Image>().enabled = !JoystickMode;
         righttJoystick.GetComponent<Image>().enabled = JoystickMode;
         //righttJoystick.isOn = JoystickMode;
     }*/
}
