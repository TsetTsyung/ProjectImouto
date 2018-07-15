using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MobPatrolState { Stationary, Patroling, Chasing, ReturningToPatrol, Attacking }
public class PatrolingMobScript : MonoBehaviour
{

    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private NavMeshAgent ourNavAgent;
    [SerializeField]
    private float nextWaypointDistance = 0.5f;
    [SerializeField]
    private float alertRange = 7.5f;
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private float chaseRange = 15f;
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float chaseSpeed;
    [SerializeField]
    private float returnToPatrolSpeed;
    [SerializeField]
    private float timeTilReplanPath;

    private PlayerHealthController playerHealthController;
    private Transform playerTransform;
    private int currentWaypoint;
    private MobPatrolState ourPatrolState;
    private float distanceToPlayerSQRD;
    private float replanTimer;

    // Use this for initialization
    void Start()
    {
        playerHealthController = GameObjectDirectory.PlayerHealthController;

        if (playerHealthController != null)
            playerTransform = playerHealthController.transform;

        if (waypoints.Length >= 1)
        {
            ourPatrolState = MobPatrolState.Patroling;

            transform.position = waypoints[waypoints.Length - 1].position;

            ourNavAgent.SetDestination(waypoints[0].position);

            currentWaypoint = 0;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // THERE ARE MUCH BETTER WAYS OF DOING THIS, BUT THIS IS THE QUICK AND DIRTY METHOD.
    //
    //////////////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {

        // Get range to player for attack or returning to patrol
        distanceToPlayerSQRD = Vector3.SqrMagnitude(transform.position - playerTransform.position);

        if (ourPatrolState == MobPatrolState.Patroling)
        {
            // Check distance to next waypoint
            if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].position) <= (nextWaypointDistance * nextWaypointDistance))
            {
                // Move on to the next waypoint
                currentWaypoint++;
                if (currentWaypoint > waypoints.Length)
                    currentWaypoint = 0;

                ourNavAgent.SetDestination(waypoints[currentWaypoint].position);
            }

            // Check if player has come within 'alert range'
            if (distanceToPlayerSQRD <= (alertRange * alertRange))
            {
                //Debug.LogWarning("Giving Chase");
                // Enter chase state
                ourPatrolState = MobPatrolState.Chasing;

                SetPathToPlayer();
                ourNavAgent.speed = chaseSpeed;

                replanTimer = timeTilReplanPath;
            }
        }
        else if (ourPatrolState == MobPatrolState.Chasing)
        {
            replanTimer -= Time.deltaTime;
            if (replanTimer <= 0f)
            {
                SetPathToPlayer();
            }

            if (distanceToPlayerSQRD <= (attackRange * attackRange))
            {
                // ATTACK
                //ourPatrolState = MobPatrolState.Attacking;

                // Start attack animation
            }
            else if (distanceToPlayerSQRD > (chaseRange * chaseRange))
            {
                //Debug.Log("Returning to patrol");
                // Return to patrol
                ourNavAgent.SetDestination(waypoints[currentWaypoint].position);
                ourNavAgent.speed = returnToPatrolSpeed;
                ourPatrolState = MobPatrolState.ReturningToPatrol;
            }
        }
        else if (ourPatrolState == MobPatrolState.ReturningToPatrol)
        {
            if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].position) <= (nextWaypointDistance * nextWaypointDistance))
            {
                // Move on to the next waypoint
                //Debug.Log("Resuming Patrol");
                currentWaypoint++;
                if (currentWaypoint > waypoints.Length)
                    currentWaypoint = 0;

                ourNavAgent.SetDestination(waypoints[currentWaypoint].position);
                ourNavAgent.speed = patrolSpeed;

                ourPatrolState = MobPatrolState.Patroling;
            }
        }
        else if (ourPatrolState == MobPatrolState.Attacking)
        {
            // Check if animation is finished.
        }
    }

    private void SetPathToPlayer()
    {
        //Debug.LogWarning("Setting path to player");
        ourNavAgent.SetDestination(playerTransform.position);
    }
}
