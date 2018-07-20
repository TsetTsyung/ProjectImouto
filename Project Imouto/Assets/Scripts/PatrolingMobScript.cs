using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MobPatrolState { Stationary, Patroling, Chasing, ReturningToPatrol, Attacking, Waiting }
public class PatrolingMobScript : MonoBehaviour
{

    [SerializeField]
    private AnimationBaseClass animationController;
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
    private float waypointWaitingTime;
    [SerializeField]
    private float timeTilReplanPath;
    [SerializeField]
    private int damage = 18;
    [SerializeField]
    private float swingRange = 1f;

    private GameControllerScript gameController;
    private PlayerHealthController playerHealthController;
    private Transform playerTransform;
    private bool creatureActive = true;
    private int currentWaypoint;
    private MobPatrolState ourPatrolState;
    private float distanceToPlayerSQRD;
    private float replanTimer;
    private float waitingTimer;
    private float nextAttackTimer;
    private float nextHitContactTimer;
    private bool attemptedHitThisLoop = false;
    private float timeBetweenAttacks;
    private float timeIntoAnimationForAttack;

    // Use this for initialization
    void Start()
    {
        gameController = GameObjectDirectory.GameController;
        playerHealthController = GameObjectDirectory.PlayerHealthController;

        if (playerHealthController != null)
            playerTransform = playerHealthController.transform;

        timeBetweenAttacks = animationController.GetAttackAnimationClipTime();
        timeIntoAnimationForAttack = animationController.GetTimeIntoAnimationForAttack();

        ActivateCreature();
    }

    public void ActivateCreature()
    {
        creatureActive = true;
        ourNavAgent.enabled = true;

        if (waypoints.Length >= 1)
        {
            ourPatrolState = MobPatrolState.Patroling;
            animationController.StartWalkAnimation();
            transform.position = waypoints[waypoints.Length - 1].position;

            ourNavAgent.SetDestination(waypoints[0].position);

            currentWaypoint = 0;
        }
        else
        {
            ourPatrolState = MobPatrolState.Waiting;
        }
    }

    public void DeactivateCreature()
    {
        creatureActive = false;
        ourNavAgent.enabled = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // THERE ARE MUCH BETTER WAYS OF DOING THIS, BUT THIS IS THE QUICK AND DIRTY METHOD.
    //
    //////////////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        if (!creatureActive)
            return;

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

                ourPatrolState = MobPatrolState.Waiting;
                animationController.StartIdlingAnimation();
                waitingTimer = waypointWaitingTime;
            }

            CheckForPlayerAlert();
        }
        else if (ourPatrolState == MobPatrolState.Waiting)
        {
            waitingTimer -= Time.deltaTime;
            if (waitingTimer <= 0f)
            {
                if (waypoints.Length >= 1)
                {
                    ourPatrolState = MobPatrolState.Patroling;
                    animationController.StartWalkAnimation();
                    transform.position = waypoints[waypoints.Length - 1].position;

                    ourNavAgent.SetDestination(waypoints[0].position);

                    currentWaypoint = 0;
                }
            }

            CheckForPlayerAlert();
        }
        else if (ourPatrolState == MobPatrolState.Chasing)
        {
            replanTimer -= Time.deltaTime;
            if (replanTimer <= 0f)
            {
                if (gameController.PlayerIsAlive)
                {
                    SetPathToPlayer();
                }
                else
                {
                    ReturnToPatrol();
                }
            }

            if (distanceToPlayerSQRD <= (attackRange * attackRange))
            {
                CommenceAttackCycle();
            }
            else if (distanceToPlayerSQRD > (chaseRange * chaseRange))
            {
                ReturnToPatrol();
            }
        }
        else if (ourPatrolState == MobPatrolState.ReturningToPatrol)
        {
            if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].position) <= (nextWaypointDistance * nextWaypointDistance))
            {
                // Move on to the next waypoint
                currentWaypoint++;
                if (currentWaypoint > waypoints.Length)
                    currentWaypoint = 0;

                ourNavAgent.SetDestination(waypoints[currentWaypoint].position);
                ourNavAgent.speed = patrolSpeed;

                ourPatrolState = MobPatrolState.Patroling;
                animationController.StartWalkAnimation();
            }
        }
        else if (ourPatrolState == MobPatrolState.Attacking)
        {
            if (!gameController.PlayerIsAlive)
                ReturnToPatrol();

            // Decrement the timers
            nextAttackTimer -= Time.deltaTime;

            if (!attemptedHitThisLoop)
            {
                nextHitContactTimer -= Time.deltaTime;
                if (nextHitContactTimer <= 0)
                {
                    attemptedHitThisLoop = true;

                    // Try to see if the player is within the attack range
                    if (distanceToPlayerSQRD <= (attackRange * attackRange))
                        playerHealthController.PlayerHasTakenDamage(damage);
                }
            }

            if (nextAttackTimer <= 0f)
            {
                // Check the location of the player
                if (distanceToPlayerSQRD > attackRange * attackRange)
                {
                    SetPathToPlayer();
                    animationController.StartRunAnimation();
                    ourPatrolState = MobPatrolState.Chasing;
                }
                else
                {
                    nextAttackTimer = timeBetweenAttacks;
                    nextHitContactTimer = timeIntoAnimationForAttack;
                    attemptedHitThisLoop = false;
                }
            }
        }
    }

    private void ReturnToPatrol()
    {
        if (waypoints.Length >= 1)
        {
            // Return to patrol
            ourNavAgent.SetDestination(waypoints[currentWaypoint].position);
            ourNavAgent.speed = returnToPatrolSpeed;
            animationController.StartRunAnimation();
            ourPatrolState = MobPatrolState.ReturningToPatrol;
        }
        else
        {
            animationController.StartIdlingAnimation();
            ourPatrolState = MobPatrolState.Waiting;
        }
    }

    private void CommenceAttackCycle()
    {
        // Set our state to attacking
        ourPatrolState = MobPatrolState.Attacking;

        // Start attack animation
        animationController.StartAttackAnimation();

        // Start timer for the weapon to make contact
        nextHitContactTimer = timeIntoAnimationForAttack;

        // Set timer for the next attack
        nextAttackTimer = timeBetweenAttacks;

        attemptedHitThisLoop = false;
    }

    private void CheckForPlayerAlert()
    {
        // Check if player has come within 'alert range'
        if (gameController.PlayerIsAlive && distanceToPlayerSQRD <= (alertRange * alertRange))
        {
            // Enter chase state
            ourPatrolState = MobPatrolState.Chasing;

            // start the running animation
            animationController.StartRunAnimation();

            SetPathToPlayer();
            ourNavAgent.speed = chaseSpeed;

            replanTimer = timeTilReplanPath;
        }
    }

    private void SetPathToPlayer()
    {
        ourNavAgent.SetDestination(playerTransform.position);
    }
}
