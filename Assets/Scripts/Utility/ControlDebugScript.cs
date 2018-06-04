using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDebugScript : MonoBehaviour {
    [SerializeField]
    CharacterControlScript characterControlScript;

    [SerializeField]
    Text runTypeText;
    [SerializeField]
    Text controlTypeText;

	// Use this for initialization
	void Start () 
    {
        SetTypeText();
        SetControlText();
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
}
