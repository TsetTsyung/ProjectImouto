using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfoScreenController : MonoBehaviour {

    [SerializeField]
    private Text missionNameText;
    [SerializeField]
    private Text missionBriefingText;
    [SerializeField]
    private Text missionTypeText;
    [SerializeField]
    private Text xpRewardText;
    [SerializeField]
    private Text coinRewardText;

    private string typeOfMissionHeader = "Type of mission: ";
    private string xpRewardHeader = "Experience: ";
    private string coinRewardHeader = "Gold: ";

    private string thisMissionName;

    private MissionController missionController;
    private OverlayController overlayController;

    // Use this for initialization
    void Start () {
        missionController = GameObjectDirectory.MissionController;
        overlayController = GameObjectDirectory.OverlayController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayMissionInfo(string missionName, string missionText, MissionType missionType, int xpReward, int coinReward)
    {
        thisMissionName = missionName;
        missionNameText.text = missionName;
        missionBriefingText.text = missionText;
        missionTypeText.text = typeOfMissionHeader + missionType.ToString();
        xpRewardText.text = xpRewardHeader + xpReward;
        coinRewardText.text = coinRewardHeader + coinReward;
    }

    public void AcceptMission()
    {
        missionController.AcceptMission(thisMissionName);
        overlayController.HideMissionInfoPanel();
    }

    public void DeclineMission()
    {
        overlayController.HideMissionInfoPanel();
    }
}
