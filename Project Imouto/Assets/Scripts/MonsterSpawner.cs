using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject skeleton;
    [SerializeField]
    private GameObject soldier;
    [SerializeField]
    private GameObject wolfRider;
    [SerializeField]
    private int numberOfMonstersToMaintain;
    [SerializeField]
    private int numberOfWaypointsAllowed;
    [SerializeField]
    private Transform waypoints;

    private int numberOfMonstersAlive;
    private GameObject monstersCatalog;
    private RaycastHit hitInfo;
    private Ray ray;

    private void Awake()
    {
        GameObjectDirectory.MonsterSpawner = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnMonster(EnemyType enemyToSpawn, Vector3 spawnLoc)
    {
        GameObject returningMonster = null;

        ray = new Ray(new Vector3(spawnLoc.x, 200, spawnLoc.z), -Vector3.up);
        if (Physics.Raycast(ray, out hitInfo, 400))
        {
            spawnLoc = hitInfo.point; // Update the spawnloc to make sure the Monster is on the ground

            switch (enemyToSpawn)
            {
                case EnemyType.Soldier:
                    returningMonster = Instantiate(soldier, spawnLoc, Quaternion.identity);
                    break;
                case EnemyType.Skeleton:
                    returningMonster = Instantiate(skeleton, spawnLoc, Quaternion.identity);
                    break;
                case EnemyType.WolfRider:
                    returningMonster = Instantiate(wolfRider, spawnLoc, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

        if (numberOfWaypointsAllowed > 0)
        {
            
            //Vector3[] newWaypoints = new Vector3[]
            //returningMonster.GetComponent<PatrolingMobScript>().SetWaypoints()
        }

        numberOfMonstersAlive++;
        return returningMonster;
    }

    public  void ThisCreatureDied(GameObject creatureThatDied)
    {
        
    }
}
