using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject skeleton;
    [SerializeField]
    private float skeletonRandomSpawnChance;
    [SerializeField]
    private GameObject soldier;
    [SerializeField]
    private float soldierRandomSpawnChance;
    [SerializeField]
    private GameObject wolfRider;
    [SerializeField]
    private float wolfRiderRandomSpawnChance;
    [SerializeField]
    private float distanceFromPlayerForRandomSpawns;
    [SerializeField]
    private int numberOfMonstersToMaintain;
    [SerializeField]
    private float timeBetweenMonsterCountAndSpawn;
    [SerializeField]
    private int numberOfWaypointsAllowed;
    [SerializeField]
    private Transform[] waypoints;

    private int numberOfMonstersAlive;
    private List<GameObject> monstersList;
    private float totalSpawnChanceValue;
    private float monsterCountAndSpawnTimer;

    private Transform playerTransform;

    private void Awake()
    {
        GameObjectDirectory.MonsterSpawner = this;
    }

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObjectDirectory.PlayerHealthController.transform;
        totalSpawnChanceValue = soldierRandomSpawnChance + skeletonRandomSpawnChance + wolfRiderRandomSpawnChance;
        monsterCountAndSpawnTimer = timeBetweenMonsterCountAndSpawn;
        //SpawnRandomMonsters(numberOfMonstersToMaintain);
    }

    // Update is called once per frame
    void Update()
    {
        monsterCountAndSpawnTimer -= Time.deltaTime;
        if(monsterCountAndSpawnTimer <= 0f)
        {
            // Perform Count
            CountAndSpawnMonsters();
            monsterCountAndSpawnTimer = timeBetweenMonsterCountAndSpawn;
        }
    }

    private void SpawnRandomMonsters(int numberOfMonstersToSpawn)
    {
        if (waypoints.Length <= 0)
            return;

        Vector3 spawnLoc = Vector3.zero;

        for (int i = 0; i < numberOfMonstersToSpawn; i++)
        {
            spawnLoc = waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position;

            while (Vector3.Distance(playerTransform.position, spawnLoc) <= distanceFromPlayerForRandomSpawns)
            {
                spawnLoc = waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position;
            }

            float randomValue = UnityEngine.Random.Range(0, totalSpawnChanceValue);
            if (randomValue >= 0 && randomValue < soldierRandomSpawnChance)
            {
                // Spawn a Soldier
                SpawnMonster(EnemyType.Soldier, waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position);
                
            }
            else if (randomValue >= soldierRandomSpawnChance && randomValue < skeletonRandomSpawnChance)
            {
                // Spawn a Skeleton
                SpawnMonster(EnemyType.Skeleton, waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position);
            }
            else
            {
                // Spawn a WolfRider
                SpawnMonster(EnemyType.WolfRider, waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position);
            }
        }
    }

    public void RedoAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        for (int i = 0; i < monsters.Length; i++)
        {
            Destroy(monsters[i].gameObject);
        }

        SpawnRandomMonsters(numberOfMonstersToMaintain);
    }

    public GameObject SpawnMonster(EnemyType enemyToSpawn, Vector3 spawnLoc)
    {
        GameObject returningMonster = null;
        spawnLoc = Utilities.GetSurfacePoint(spawnLoc);
        
        if(spawnLoc != Vector3.zero)
        {
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
        else
        {
            Debug.LogError("We couldn't get a valid Spawn Point");
        }

        if (numberOfWaypointsAllowed > 0)
        {
            Vector3[] newWaypoints = new Vector3[UnityEngine.Random.Range(1, numberOfWaypointsAllowed)];

            for (int i = 0; i < newWaypoints.Length; i++)
            {                
                newWaypoints[i] = waypoints[UnityEngine.Random.Range(0, waypoints.Length)].position;
            }

            returningMonster.GetComponent<PatrolingMobScript>().SetWaypoints(newWaypoints);
        }

        numberOfMonstersAlive++;
        return returningMonster;
    }

    public  void ThisCreatureDied(GameObject creatureThatDied)
    {
        numberOfMonstersAlive--;

        CountAndSpawnMonsters();
    }

    private void CountAndSpawnMonsters()
    {
        if(numberOfMonstersAlive < numberOfMonstersToMaintain)
        {
            SpawnRandomMonsters(numberOfMonstersToMaintain - numberOfMonstersAlive);
        }
    }
}
