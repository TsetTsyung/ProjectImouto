using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Mission", menuName = "Project Imouto/Mission", order = 1)]
public class MissionScritableObject : ScriptableObject {

    public string missionName;
    public MissionType missionType;
    public bool missionIsAvailable;
    public bool missionIsActive;
    public string missionBriefing;
    public string completedMessage;
    public int xpReward;
    public int coinReward;
    public EnemyType[] enemies;
    public Vector3 missionBoardLocation;
    public float missionBoardDirection;
    public Vector3 enemyLocations;
    public Vector3 enemyWaypoints;
    public Vector3[] waypoints;
    public string[] waypointMessages;
    public Vector3 missionLocation;
    public string unlocksMissions;
}
