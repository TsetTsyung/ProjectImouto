using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCompletedScreenController : MonoBehaviour {

    [SerializeField]
    private Text completedMessageText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMessage(string completedMessage)
    {
        completedMessageText.text = completedMessage;
    }
}
