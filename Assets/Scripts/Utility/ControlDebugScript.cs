using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDebugScript : MonoBehaviour {
    [SerializeField]
    CharacterControlScript characterControlScript;

    [SerializeField]
    Button runButton;
    [SerializeField]
    Text runTypeText;
    [SerializeField]
    Text controlTypeText;

    [SerializeField]
    Text vehicleText;
	// Use this for initialization
	void Start () 
    {
        SetTypeText();
        SetControlText();
        SetVehicleText();
	}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public void OnRunTypeButtonsPressed ()
    {
        characterControlScript.autoForward = !characterControlScript.autoForward;
        SetTypeText();
    }

    public void OnControlTypeButtonPressed ()
    {
        ControlType currentControlType = characterControlScript.characterControlType;
        currentControlType += 1;
        if (Convert.ToInt32(currentControlType) >= Enum.GetNames(typeof(ControlType)).Length)
        {
            currentControlType = 0;
        }
        characterControlScript.characterControlType = currentControlType;

        SetControlText();
    }

    public void OnVehicleButtonPressed ()
    {
        TrackManager.Instance.withVehicles = !TrackManager.Instance.withVehicles;
        TrackManager.Instance.RemoveAllVehicles();
        SetVehicleText();
    }


    void SetTypeText ()
    {
        if (characterControlScript.autoForward)
        {
            runTypeText.text = "Auto";
        }
        else
        {
            runTypeText.text = "Manual";
        }
    }

    void SetControlText ()
    {
        controlTypeText.text = characterControlScript.characterControlType.ToString();
    }

    void SetVehicleText ()
    {
        vehicleText.text = "With Vehicle";
        if (!TrackManager.Instance.withVehicles)
        {
            runButton.interactable = true;
            vehicleText.text = "Without Vehicle";
        }
        else
        {
            characterControlScript.autoForward = false;
            SetTypeText();
            runButton.interactable = false;
            TrackManager.Instance.GenerateVehicle();
        }
    }
}
