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
    
    public Image tutorial_checkMarkHandLeft;
    public Image tutorial_checkMarkHandRight;
    public Image tutorial_checkMarkArrow;
    public Image tutorial_checkMarkJoystick;

    public void Start()
    {


        OpenControls();


    }

    public void OpenControls()
    {

        if (GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, 1))
        {


            tutorial_checkMarkHandLeft.enabled = false;
            tutorial_checkMarkHandRight.enabled = true;

            checkMarkHandLeft.enabled = false;
            checkMarkHandRight.enabled = true;
            ChangeModeJoystick(1);
        }
        else
        {
            tutorial_checkMarkHandLeft.enabled = true;
            tutorial_checkMarkHandRight.enabled = false;

            checkMarkHandLeft.enabled = true;
            checkMarkHandRight.enabled = false;
            ChangeModeJoystick(0);
        }
        if (GamePrefs.GetBool(GameUtils.PREFS_JOYSTICK_CONTROL_HAND, 1))
        {


            tutorial_checkMarkArrow.enabled = true;
            tutorial_checkMarkJoystick.enabled = false;
            checkMarkArrow.enabled = true;
            checkMarkJoystick.enabled = false;


            ChangeModeDirection(1);
        }
        else
        {


            tutorial_checkMarkArrow.enabled = false;
            tutorial_checkMarkJoystick.enabled = true;
            checkMarkArrow.enabled = false;
            checkMarkJoystick.enabled = true;
            ChangeModeDirection(0);

        }
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
