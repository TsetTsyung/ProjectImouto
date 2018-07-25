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
    private string currentMissionName = "";
    private MissionType currentMissionType;
    private int currentMissionSlot;
    private int currentWaypointPointer = 0;
    private bool currentlyFollowingWaypoints = false;

    // variables that are mission specific
    // KillTargetMission
    private MonsterHealthScript targetMonster;

    // DeliverTo


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
            missions[i] = ScriptableObject.CreateInstance<MissionScritableObject>();
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
        if (currentMissionName != "")
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
                currentMissionName = acceptedMissionName;
                currentMissionSlot = i;
                currentMissionType = missions[i].missionType;
                missions[i].missionIsActive = true;
                BeginMission();
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
            currentMissionName = "";
            overlayController.DisplayMissionCompletedPanel(missions[slot].completedMessage);
        }
    }


    private void BeginMission()
    {
        // This is where the fun starts.
        switch (currentMissionType)
        {
            case MissionType.KillTarget:
                BeginKillTargetMission();
                currentMissionType = MissionType.KillTarget;
                break;
            case MissionType.DeliverToTarget:
                BeginDeliverToTargetMission();
                currentMissionType = MissionType.DeliverToTarget;
                break;
            case MissionType.PayTarget:
                BeginPayTargetMission();
                currentMissionType = MissionType.PayTarget;
                break;
            case MissionType.GoTo:
                BeginGoToMission();
                currentMissionType = MissionType.GoTo;
                break;
            default:
                break;
        }
    }

    private void BeginKillTargetMission()
    {
        // We're good to go

        // Spawn Target Monsters


        // Spawn Monsters
        if (missions[currentMissionSlot].enemies.Length > 0)
        {
            SpawnMissionMonsters();
        }
    }

    private void SpawnMissionMonsters()
    {

    }

    private void BeginDeliverToTargetMission()
    {
    }

    private void BeginPayTargetMission()
    {
    }

    private void BeginGoToMission()
    {
        if (missions[slot].missionName.Length == missionName.Length && missions[slot].missionName.Contains(missionName))
        {
            // We're in the right mission, check for waypoints
            if (missions[slot].waypoints.Length != 0)
            {
                currentWaypointPointer = 0;
                currentlyFollowingWaypoints = true;
                itemSpawner.SpawnLocationMarker(missions[slot].waypoints[currentWaypointPointer]);
            }
            else
            {
                itemSpawner.SpawnLocationMarker(missions[slot].missionLocation);
            }
        }
    }

    private void CheckMissionSuccess()
    {
        switch (currentMissionType)
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
        // Do checks for waypoint
        if (currentlyFollowingWaypoints)
            CheckWaypointReached();
    }

    private void CheckDeliverToTargetMissionSuccess()
    {
        // Do checks for waypoint
        if (currentlyFollowingWaypoints)
            CheckWaypointReached();
    }

    private void CheckPayTargetMissionSuccess()
    {
        // Do checks for waypoint
        if (currentlyFollowingWaypoints)
            CheckWaypointReached();
    }

    private void CheckGoToMissionSuccess()
    {
        // Do checks for waypoint
        if (currentlyFollowingWaypoints)
            CheckWaypointReached();

        float distance = Vector3.Distance(playerXPController.transform.position, missions[currentMissionSlot].missionLocation);
        if (distance < distanceForGoToCompletion)
        {
            // Mission Completed
            MissionCompleted(currentMissionSlot, currentMissionName);
        }
    }

    private void CheckWaypointReached()
    {
        float distance = Vector3.Distance(playerXPController.transform.position, missions[currentMissionSlot].waypoints[currentWaypointPointer]);
        if (distance <= distanceForGoToCompletion)
        {
            overlayController.DisplayMessageText(missions[currentMissionSlot].waypointMessages[currentWaypointPointer]);
            currentWaypointPointer++;
            if (currentWaypointPointer > missions[currentMissionSlot].waypoints.Length)
            {
                // We've reached the end of the waypoints, move onto final location
                currentlyFollowingWaypoints = false;
                itemSpawner.SpawnLocationMarker(missions[currentMissionSlot].missionLocation);
            }
        }
    }
}
