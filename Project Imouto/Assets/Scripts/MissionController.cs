using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType { KillTarget, PayTarget, GoTo }
public enum EnemyType { Soldier, Skeleton, WolfRider }
public class MissionController : MonoBehaviour
{
    [SerializeField]
    private float distanceForGoToCompletion = 2f;
    [SerializeField]
    private MissionScritableObject[] originalMissions;
    [SerializeField]
    private GameObject missionBoardPrefab;
    [SerializeField]
    private GameObject payTargetPrefab;

    private List<MissionBoardInteractable> missionBoards;

    private bool[] playerCompletedMission;
    private MissionScritableObject[] missions;
    private OverlayController overlayController;
    private PlayerXPController playerXPController;
    private PlayerTreasuryController playerTreasuryController;
    private GameObject spawnedMissionBoard;
    private MissionBoardInteractable spawnedInteractableMissionBoard;
    private PlayerProfileController playerProfileController;
    private ItemSpawnerScript itemSpawner;
    private MonsterSpawner monsterSpawner;
    private bool haveAnActiveMission = false;
    private string currentMissionName = "";
    private MissionType currentMissionType;
    private int currentMissionSlot;
    private int currentWaypointPointer = 0;
    private bool currentlyFollowingWaypoints = false;
    private int amountPaidToMission = 0;

    private GameObject newMonster;
    private GameObject payTarget;

    // variables that are mission specific
    // KillTargetMission
    private MonsterHealthScript targetMonster;
    private bool targetDestroyed = false;

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
        playerTreasuryController = GameObjectDirectory.PlayerTreasuryController;
        itemSpawner = GameObjectDirectory.ItemSpawner;
        monsterSpawner = GameObjectDirectory.MonsterSpawner;
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        //CreateMissionsList();
        //SetupMissions();
    }

    public void ReloadMissions()
    {
        RemoveAllMissionBoards();
        CreateMissionsList();
        SetupMissions();
    }


    private void CreateMissionsList()
    {
        missions = new MissionScritableObject[originalMissions.Length];

        bool[] completedMissions = playerProfileController.GetCompletedMissions();
        bool continuing = false;

        if (completedMissions != null && completedMissions.Length == missions.Length)
        {
            continuing = true;
        }

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
            missions[i].amountToPay = originalMissions[i].amountToPay;
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

            if (continuing)
            {
                missions[i].completed = completedMissions[i];
            }
            else
            {
                // It's a new game, essentially, so start again
                missions[i].completed = false;
            }

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
            if (missions[i].missionIsAvailable && !missions[i].missionIsActive && !missions[i].completed)
            {
                //Vector3 spawnLocation = missions[i].missionBoardLocation;
                Vector3 spawnLocation = Utilities.GetSurfacePoint(missions[i].missionBoardLocation);
                if(spawnLocation != Vector3.zero)
                {
                    spawnedMissionBoard = Instantiate(missionBoardPrefab, spawnLocation, Quaternion.Euler(0, missions[i].missionBoardDirection, 0));

                    spawnedInteractableMissionBoard = spawnedMissionBoard.GetComponent<MissionBoardInteractable>();

                    spawnedInteractableMissionBoard.SetText("Mission", missions[i].missionName);

                    missionBoards.Add(spawnedInteractableMissionBoard);
                }
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

    private void RemoveAllMissionBoards()
    {
        if (missionBoards == null)
            return;

        for (int i = 0; i < missionBoards.Count; i++)
        {
            missionBoards[i].transform.position = Vector3.zero;
            missionBoards[i].DisableText();
            Destroy(missionBoards[i].gameObject);
        }
    }

    private void RemoveMissionBoard(string acceptedMissionName)
    {
        for (int i = 0; i < missionBoards.Count; i++)
        {
            if (missionBoards[i].GetMissionName().Length == acceptedMissionName.Length && missionBoards[i].GetMissionName().Contains(acceptedMissionName))
            {
                missionBoards[i].DisableText();
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
                RemoveMissionBoard(acceptedMissionName);
            }
        }
    }


    public void MissionCompleted()
    {
        // Show text and give winning
        for (int i = 0; i < missions.Length; i++)
        {
            if(missions[i].missionName.Length == currentMissionName.Length && missions[i].missionName.Contains(currentMissionName))
            {
                missions[i].completed = true;
            }
        }

        playerXPController.AddXP(missions[currentMissionSlot].xpReward);
        playerTreasuryController.AddCoin(missions[currentMissionSlot].coinReward);
        itemSpawner.DisableLocationMarker();
        currentMissionName = "";
        overlayController.DisplayMissionCompletedPanel(missions[currentMissionSlot].completedMessage);
        amountPaidToMission = 0;

        if (payTarget != null)
            Destroy(payTarget.gameObject);
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

        // Spawn Monsters
        if (missions[currentMissionSlot].enemies.Length > 0)
        {
            SpawnMissionMonsters();
        }
    }

    private void BeginKillTargetMission()
    {
        SetupWaypoints();
        SpawnTargetMonster();
    }


    private void BeginDeliverToTargetMission()
    {
        SpawnPayTarget();
        SetupWaypoints();
    }


    private void BeginPayTargetMission()
    {
        amountPaidToMission = 0;
        
        SetupWaypoints();
        SpawnPayTarget();
    }

    private void BeginGoToMission()
    {
        SetupWaypoints();
    }

    private void SetupWaypoints()
    {
        // We're in the right mission, check for waypoints
        if (missions[currentMissionSlot].waypoints.Length != 0)
        {
            currentWaypointPointer = 0;
            currentlyFollowingWaypoints = true;
            itemSpawner.SpawnLocationMarker(missions[currentMissionSlot].waypoints[currentWaypointPointer]);
        }
        else
        {
            itemSpawner.SpawnLocationMarker(missions[currentMissionSlot].missionLocation);
        }
    }

    private void SpawnMissionMonsters()
    {
        newMonster = null;
        for (int i = 0; i < missions[currentMissionSlot].enemies.Length; i++)
        {
            newMonster = monsterSpawner.SpawnMonster(missions[currentMissionSlot].enemies[i], missions[currentMissionSlot].enemyLocations[i]);
            if (missions[currentMissionSlot].enemyWaypoints.Length > 0 && newMonster != null)
            {
                newMonster.GetComponent<PatrolingMobScript>().SetWaypoints(missions[currentMissionSlot].enemyWaypoints);
            }
        }
    }

    private void SpawnTargetMonster()
    {
        newMonster = null;
        targetMonster = null;

        newMonster = monsterSpawner.SpawnMonster(missions[currentMissionSlot].targetEnemy, missions[currentMissionSlot].targetLocation);

        targetMonster = newMonster.GetComponent<MonsterHealthScript>();
        targetMonster.SetAsTargetCreature();

        if (missions[currentMissionSlot].targetWaypoints.Length > 0)
        {
            newMonster.GetComponent<PatrolingMobScript>().SetWaypoints(missions[currentMissionSlot].targetWaypoints);
        }
    }

    private void SpawnPayTarget()
    {
        if (payTarget != null)
            Destroy(payTarget.gameObject);

        Vector3 spawnLoc = Utilities.GetSurfacePoint(missions[currentMissionSlot].missionLocation);
        if (spawnLoc != Vector3.zero)
        {
            payTarget = Instantiate(payTargetPrefab, spawnLoc, Quaternion.identity);
            payTarget.GetComponent<PayTargetInteractable>().SetMissionParameters(currentMissionName, missions[currentMissionSlot].amountToPay);
        }
        else
        {
            Debug.LogError("We could not get a valid spawnPoint");
        }
    }

    private void CheckMissionSuccess()
    {
        switch (currentMissionType)
        {
            case MissionType.KillTarget:
                CheckKillTargetMissionSuccess();
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

        // Check to see if we've killed the target.
        if (targetDestroyed)
            MissionCompleted();
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

        if (amountPaidToMission >= missions[currentMissionSlot].amountToPay)
        {
            MissionCompleted();
        }
    }

    private void CheckGoToMissionSuccess()
    {
        // Do checks for waypoint
        if (currentlyFollowingWaypoints)
        {
            CheckWaypointReached();
        }
        else
        {
            float distance = Vector3.Distance(playerXPController.transform.position, missions[currentMissionSlot].missionLocation);
            if (distance < distanceForGoToCompletion)
            {
                // Mission Completed
                MissionCompleted();
            }
        }
    }

    private void CheckWaypointReached()
    {
        float distance = Vector3.Distance(playerXPController.transform.position, missions[currentMissionSlot].waypoints[currentWaypointPointer]);
        if (distance <= distanceForGoToCompletion)
        {
            overlayController.DisplayMessageText(missions[currentMissionSlot].waypointMessages[currentWaypointPointer]);
            currentWaypointPointer++;
            if (currentWaypointPointer >= missions[currentMissionSlot].waypoints.Length)
            {
                // We've reached the end of the waypoints, move onto final location
                currentlyFollowingWaypoints = false;
                itemSpawner.SpawnLocationMarker(missions[currentMissionSlot].missionLocation);
            }
            else
            {
                itemSpawner.SpawnLocationMarker(missions[currentMissionSlot].waypoints[currentWaypointPointer]);
            }
        }
    }

    public void TargetCreatureDied(MonsterHealthScript creatureThatDied)
    {
        if (targetMonster == creatureThatDied)
            targetDestroyed = true;
    }

    public void PaidTarget(int _amountPaid)
    {
        amountPaidToMission = _amountPaid;
    }

    public void SaveAllMissionStatus()
    {
        bool[] completedMissions = new bool[missions.Length];

        for (int i = 0; i < missions.Length; i++)
            completedMissions[i] = missions[i].completed;

        playerProfileController.UpdateCompletedMissions(completedMissions);
    }
}
