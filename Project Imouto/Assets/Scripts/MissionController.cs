﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType { KillTarget, DeliverToTarget, PayTarget }
public enum EnemyType { Soldier, Skeleton, WolfRider }
public class MissionController : MonoBehaviour {

    [SerializeField]
    private MissionScritableObject[] missions;
    [SerializeField]
    private GameObject missionBoardPrefab;

    private List<MissionBoardInteractable> missionBoards;

    private OverlayController overlayController;
    private PlayerXPController playerXPController;
    private GameObject spawnedMissionBoard;
    private MissionBoardInteractable spawnedInteractableMissionBoard;
    private bool haveAnActiveMission = false;
    private string currentActiveMission = "";

    private void Awake()
    {
        GameObjectDirectory.MissionController = this;
        overlayController = GameObjectDirectory.OverlayController;
        playerXPController = GameObjectDirectory.PlayerXPController;
    }

    // Use this for initialization
    void Start () {
        SetupMissions();

	}


    // Update is called once per frame
    void Update () {
		
	}

    private void SetupMissions()
    {
        missionBoards = new List<MissionBoardInteractable>();
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].missionIsAvailable && !missions[i].missionIsActive)
            {
                spawnedMissionBoard = Instantiate(missionBoardPrefab, missions[i].missionBoardLocation, Quaternion.Euler(0, missions[i].missionBoardDirection, 0));

                spawnedInteractableMissionBoard = spawnedMissionBoard.GetComponent<MissionBoardInteractable>();

                spawnedInteractableMissionBoard.SetText("Mission", missions[i].missionName);

                missionBoards.Add(spawnedInteractableMissionBoard);
            }
        }
    }

    public void DisplayMissionScreen(string missionName)
    {
        Debug.LogWarning("Trying to display mission screen.  Searching for mission ." + missionName + ".");
        for (int i = 0; i < missions.Length; i++)
        {
            Debug.Log("we are at slot " + i + " and the name is ." + missions[i].missionName + ".");

            for (int j = 0; j < missionName.Length; j++)
            {
                Debug.Log(missionName[j] + " = " + (int)missionName[j]);
                Debug.Log(missions[i].missionName[j] + " = " + ((int)missions[i].missionName[j]).ToString());
            }
            bool result = missionName.Equals(missions[i].name);
            Debug.LogWarning("The result is " + result);
            int intResult = missionName.CompareTo(missions[i].name);
            Debug.Log("compareto comes back with " + intResult);
            if (result)
            {
                Debug.LogWarning("We've found the mission " + missionName);
                Debug.Log("overlayController is " + overlayController);
                overlayController.DisplayMissionInfoPanel(missions[i].missionName, missions[i].missionBriefing,
                    missions[i].missionType, missions[i].xpReward, missions[i].coinReward);

                i = missions.Length;
            }
        }

    }

    private void RemoveMissionBoard(string acceptedMissionName)
    {
        for (int i = 0; i < missionBoards.Count; i++)
        {
            if (missionBoards[i].GetMissionName() == acceptedMissionName)
            {
                Destroy(missionBoards[i].gameObject);
                missionBoards.RemoveAt(i);
                i = missionBoards.Count;
            }
        }
    }

    public void AcceptMission(string acceptedMissionName)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].name == acceptedMissionName)
            {
                haveAnActiveMission = true;
                currentActiveMission = acceptedMissionName;
                missions[i].missionIsActive = true;

                RemoveMissionBoard(acceptedMissionName);
            }
        }
    }


    public void MissionCompleted(string completedMissionName)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].name == completedMissionName)
            {
                // Show text and give winning
                playerXPController.AddXP(missions[i].xpReward);
                overlayController.DisplayMissionCompletedPanel(missions[i].completedMessage);
            }
        }
    }
}
