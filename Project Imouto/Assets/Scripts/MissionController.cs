using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType { KillTarget, DeliverToTarget, PayTarget, GoTo }
public enum EnemyType { Soldier, Skeleton, WolfRider }
public class MissionController : MonoBehaviour
{
    [SerializeField]
    private float distanceForGoToCompletion = 2f;
    [SerializeField]
    private MissionScritableObject[] originalMissions;
    [SerializeField]
    private GameObject missionBoardPrefab;

    private List<MissionBoardInteractable> missionBoards;

    private MissionScritableObject[] missions;
    private OverlayController overlayController;
    private PlayerXPController playerXPController;
    private GameObject spawnedMissionBoard;
    private MissionBoardInteractable spawnedInteractableMissionBoard;
    private ItemSpawnerScript itemSpawner;
    private bool haveAnActiveMission = false;
    private string currentActiveMission = "";
    private MissionType currentActiveMissionType;
    private int currentMissionSlot;

    private void Awake()
    {
        GameObjectDirectory.MissionController = this;
    }

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
        playerXPController = GameObjectDirectory.PlayerXPController;
        itemSpawner = GameObjectDirectory.ItemSpawner;
        CreateMissionsList();
        SetupMissions();

    }

    private void CreateMissionsList()
    {
        missions = new MissionScritableObject[originalMissions.Length];
        for (int i = 0; i < originalMissions.Length; i++)
        {
            missions[i] = new MissionScritableObject();
            missions[i].coinReward = originalMissions[i].coinReward;
            missions[i].completedMessage = originalMissions[i].completedMessage;
            missions[i].enemies = originalMissions[i].enemies;
            missions[i].enemyLocations = originalMissions[i].enemyLocations;
            missions[i].enemyWaypoints = originalMissions[i].enemyWaypoints;
            missions[i].missionBoardDirection = originalMissions[i].missionBoardDirection;
            missions[i].missionBoardLocation = originalMissions[i].missionBoardLocation;
            missions[i].missionBriefing = originalMissions[i].missionBriefing;
            missions[i].missionIsActive = originalMissions[i].missionIsActive;
            missions[i].missionIsAvailable = originalMissions[i].missionIsAvailable;
            missions[i].missionLocation = originalMissions[i].missionLocation;
            missions[i].missionName = originalMissions[i].missionName;
            missions[i].missionType = originalMissions[i].missionType;
            missions[i].targetEnemy = originalMissions[i].targetEnemy;
            missions[i].targetLocation = originalMissions[i].targetLocation;
            missions[i].targetWaypoints = originalMissions[i].targetWaypoints;
            missions[i].unlocksMissions = originalMissions[i].unlocksMissions;
            missions[i].waypointMessages = originalMissions[i].waypointMessages;
            missions[i].waypoints = originalMissions[i].waypoints;
            missions[i].xpReward = originalMissions[i].xpReward;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (currentActiveMission != "")
            CheckMissionSuccess();
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
        for (int i = 0; i < missions.Length; i++)
        {
            // Check if the strings are the same length
            bool result = false;
            if (missionName.Length == missions[i].missionName.Length)
                // since they are, see if one contains the other
                if (missionName.Contains(missions[i].missionName))
                    // if so then we have a true result (string.equals was playing up)
                    result = true;

            if (result)
            {
                overlayController.DisplayMissionInfoPanel(missions[i].missionName, missions[i].missionBriefing,
                    missions[i].missionType, missions[i].xpReward, missions[i].coinReward);

                i = missions.Length;
            }
        }

    }

    private void RemoveMissionBoard(int slot, string acceptedMissionName)
    {
        for (int i = 0; i < missionBoards.Count; i++)
        {
            if (missionBoards[slot].GetMissionName().Length == acceptedMissionName.Length && missionBoards[slot].GetMissionName().Contains(acceptedMissionName))
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
            if (acceptedMissionName.Length == missions[i].missionName.Length && acceptedMissionName.Contains(missions[i].missionName))
            {
                haveAnActiveMission = true;
                currentActiveMission = acceptedMissionName;
                currentMissionSlot = i;
                missions[i].missionIsActive = true;
                BeginMission(i, missions[i].missionType, acceptedMissionName);
                RemoveMissionBoard(i, acceptedMissionName);
            }
        }
    }


    public void MissionCompleted(int slot, string completedMissionName)
    {
        if (missions[slot].missionName.Length == completedMissionName.Length && missions[slot].missionName.Contains(completedMissionName))
        {
            // Show text and give winning
            playerXPController.AddXP(missions[slot].xpReward);
            itemSpawner.DisableLocationMarker();
            currentActiveMission = "";
            overlayController.DisplayMissionCompletedPanel(missions[slot].completedMessage);
        }
    }


    private void BeginMission(int slot, MissionType missionType, string acceptedMissionName)
    {
        // This is where the fun starts.
        switch (missionType)
        {
            case MissionType.KillTarget:
                BeginKillTargetMission(slot, acceptedMissionName);
                currentActiveMissionType = MissionType.KillTarget;
                break;
            case MissionType.DeliverToTarget:
                BeginDeliverToTargetMission(slot, acceptedMissionName);
                currentActiveMissionType = MissionType.DeliverToTarget;
                break;
            case MissionType.PayTarget:
                BeginPayTargetMission(slot, acceptedMissionName);
                currentActiveMissionType = MissionType.PayTarget;
                break;
            case MissionType.GoTo:
                BeginGoToMission(slot, acceptedMissionName);
                currentActiveMissionType = MissionType.GoTo;
                break;
            default:
                break;
        }
    }

    private void BeginKillTargetMission(int slot, string missionName)
    {
    }

    private void BeginDeliverToTargetMission(int slot, string missionName)
    {
    }

    private void BeginPayTargetMission(int slot, string missionName)
    {
    }

    private void BeginGoToMission(int slot, string missionName)
    {
        if (missions[slot].missionName.Length == missionName.Length && missions[slot].missionName.Contains(missionName))
        {
            itemSpawner.SpawnLocationMarker(missions[slot].missionLocation);
        }
    }

    private void CheckMissionSuccess()
    {
        switch (currentActiveMissionType)
        {
            case MissionType.KillTarget:
                CheckKillTargetMissionSuccess();
                break;
            case MissionType.DeliverToTarget:
                CheckDeliverToTargetMissionSuccess();
                break;
            case MissionType.PayTarget:
                CheckPayTargetMissionSuccess();
                break;
            case MissionType.GoTo:
                CheckGoToMissionSuccess();
                break;
            default:
                break;
        }
    }


    private void CheckKillTargetMissionSuccess()
    {
    }

    private void CheckDeliverToTargetMissionSuccess()
    {
    }

    private void CheckPayTargetMissionSuccess()
    {
    }

    private void CheckGoToMissionSuccess()
    {
        // Do checks for waypoint
        float distance = Vector3.Distance(playerXPController.transform.position, missions[currentMissionSlot].missionLocation);
        if (distance < distanceForGoToCompletion)
        {
            // Mission Completed
            MissionCompleted(currentMissionSlot, currentActiveMission);
        }
    }
}
