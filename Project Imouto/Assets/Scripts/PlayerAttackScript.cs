using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboMoves { Unset, LightAttack, HeavyAttack1, HeavyAttack2, SuperHeavyAttack }
public class PlayerAttackScript : MonoBehaviour
{

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float movementPerSecDuringSHAttack = 3.5f;


    [SerializeField]
    private BoxCollider swordCollider;

    private PlayerAnimationController playerAnimationController;
    private PlayerInputController playerInputController;
    private Rigidbody ourRB;
    private Animator animator;

    private AnimatorClipInfo[] clipInfo;


    private AnimatorStateInfo stateInfo;


    private bool heavy1Unlocked = true;
    private bool heavy2Unlocked = true;
    private bool superHeavyUnlocked = true;
    private Vector3 landPosition;
    private RaycastHit hitinfo;

    private bool airborneSuperHeavyAttack;
    private bool startedCombo = false;
    private ComboMoves[] movesThisCombo = new ComboMoves[3];
    private int comboPointer = 0;

    private void Awake()
    {
        GameObjectDirectory.PlayerAttackScript = this;
    }

    // Use this for initialization
    void Start()
    {
        playerAnimationController = GameObjectDirectory.PlayerAnimationController;
        playerInputController = GameObjectDirectory.PlayerInputController;
        animator = GetComponent<Animator>();
        ourRB = GetComponent<Rigidbody>();

        animator.GetBehaviour<AttackSubStateMonitor>().AssignAttackScriptReference(this);
    }

    // Update is called once per frame
    private void Update()
    {
        stateInfo = playerAnimationController.GetState();
    }

    void LateUpdate()
    {
        if (airborneSuperHeavyAttack)
            ourRB.MovePosition(transform.position + (transform.forward * movementPerSecDuringSHAttack * Time.deltaTime));
    }

    public void PlayerHasPressedAttack()
    {
        stateInfo = playerAnimationController.GetState();

        if (stateInfo.IsName("SuperHeavyAttack"))
            return;

        if (!startedCombo)
        {
            AddMoveToCombo(ComboMoves.LightAttack);
            return;
        }

        for (int i = 0; i < movesThisCombo.Length; i++)
        {
            // If we already have a light attack in this combo
            // return
            if (movesThisCombo[i] == ComboMoves.LightAttack)
                return;

            AddMoveToCombo(ComboMoves.LightAttack);
        }
    }

    public void PlayerHasPressedHeavyAttack()
    {
        bool heavyAttack1InCombo = false;
        bool heavyAttack2InCombo = false;

        stateInfo = playerAnimationController.GetState();

        if (!heavy1Unlocked || comboPointer >= movesThisCombo.Length || stateInfo.IsName("SuperHeavyAttack"))
            return;

        if (!startedCombo)
        {
            Debug.Log("We're starting the combo");
            // As this this an unlocked move and the beginning
            // of a new combo, add it and return
            AddMoveToCombo(ComboMoves.HeavyAttack1);
            return;
        }

        Debug.Log("About to loop through the combo list");
        // Let's see what attack they were planning
        for (int i = 0; i < movesThisCombo.Length; i++)
        {
            if (movesThisCombo[i] == ComboMoves.HeavyAttack1)
            {
                heavyAttack1InCombo = true;
                Debug.LogError("Heavy1 is in the combo already");
            }
            else if (movesThisCombo[i] == ComboMoves.HeavyAttack2)
            {
                heavyAttack2InCombo = true; ;
            }
        }

        if (!heavyAttack1InCombo && !stateInfo.IsName("HeavyAttack1"))
        {
            // Add the heavy 1 to the combo
            AddMoveToCombo(ComboMoves.HeavyAttack1);
        }
        else
        {
            // There's already a heavyattack1 in the combo
            // let's see if heavyattack2 is unlocked
            if (!heavy2Unlocked)
                return;

            Debug.Log("There's already a heavy1 in the combo, trying heavy2");
            // it's unlocked, so let's add it to the combo
            if (!heavyAttack2InCombo)
            {
                AddMoveToCombo(ComboMoves.HeavyAttack2);
            }
        }
    }

    public void PlayerHasPressedSuperHeavyAttack()
    {
        if (!superHeavyUnlocked)
            return;

        playerAnimationController.PlayerAttack(ComboMoves.SuperHeavyAttack);
    }


    public void SubmitUnlockedMoves(bool heavy1UnlockState, bool heavy2UnlockState, bool superHeavyUnlockState)
    {
        heavy1Unlocked = heavy1UnlockState;
        heavy2Unlocked = heavy2UnlockState;
        superHeavyUnlocked = superHeavyUnlockState;
    }

    public void AddMoveToCombo(ComboMoves newMove)
    {
        if (comboPointer >= movesThisCombo.Length)
            return;

        Debug.LogError("Adding " + newMove + " to the combo");
        startedCombo = true;
        movesThisCombo[comboPointer] = newMove;
        comboPointer++;

        playerAnimationController.PlayerAttack(newMove);

    }

    public void HitMonster(GameObject monster)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        int damageToInflict = 10;

        if (stateInfo.IsName("HeavyAttack1"))
        {
            damageToInflict = 20;
        }
        else if (stateInfo.IsName("HeavyAttack2"))
        {
            damageToInflict = 35;
        }

        monster.GetComponent<MonsterHealthScript>().TakeDamage(damageToInflict);
    }

    #region ANIMATION EVENTS AND STATE CALLBACKS

    internal void LeftTheAttackSubState()
    {
        comboPointer = 0;
        startedCombo = false;

        // Clear out all of the moves from the combo list
        for (int i = 0; i < movesThisCombo.Length; i++)
        {
            movesThisCombo[i] = ComboMoves.Unset;
        }

        animator.SetBool("Attacking", false);
        playerInputController.DisableMovemoentInput();
    }

    public void EnteredTheAttackSubstate()
    {
        playerInputController.EnableMovementInput();
    }

    public void SHAttackStarted()
    {
    }

    public void SHAttackTakeOff()
    {
        airborneSuperHeavyAttack = true;
    }

    public void SHAttackLanding()
    {
        airborneSuperHeavyAttack = false;

    }

    public void SHAttackEnded()
    {
    }

    public void LightAttackLanding()
    {
        // Check for hits
    }

    public void HeavyAttackStarted()
    {
        swordCollider.enabled = true;
    }

    public void HeavyAttackEnded()
    {
        swordCollider.enabled = false;
    }

#endregion
}
