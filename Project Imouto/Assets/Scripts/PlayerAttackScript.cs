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
    private float lightAttackLength = 1f;
    [SerializeField]
    private float heavyAttack1Length = 1.3f;
    [SerializeField]
    private float heavyAttack2Length = 1.7f;
    [SerializeField]
    private BoxCollider swordCollider;

    private PlayerAnimationController playerAnimationController;
    private PlayerInputController playerInputController;
    private Rigidbody ourRB;
    private Animator animator;

    private AnimatorClipInfo[] clipInfo;

    private AnimatorStateInfo stateInfo;

    private bool heavy1Unlocked = false;
    private bool heavy2Unlocked = false;
    private bool superHeavyUnlocked = false;
    private Vector3 landPosition;
    private RaycastHit hitinfo;

    private bool airborneSuperHeavyAttack;
    private bool startedCombo = false;
    private ComboMoves[] movesThisCombo = new ComboMoves[3];
    private int comboPointer = 0;
    private float comboTimeLeft = 0f;
    private int bonusDamage = 0;
    private int swordDamage = 0;
    private int lightAttackDamage = 10;
    private int heavyAttack1Damage = 20;
    private int heavyAttack2Damage = 35;

    private Collider[] lightAttackHits;
    private List<GameObject> monstersThatWereHit;

    //private int moveStartedPointer = 0;
    //private int moveEndedPointer = 0;

    #region STARTUP AND INITIALISATION
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
    }

    public void SubmitUnlockedMoves(bool heavy1UnlockState, bool heavy2UnlockState, bool superHeavyUnlockState)
    {
        heavy1Unlocked = heavy1UnlockState;
        heavy2Unlocked = heavy2UnlockState;
        superHeavyUnlocked = superHeavyUnlockState;
    }

    public void SubmitBonusDamage(int _bonusDamage)
    {
        bonusDamage = _bonusDamage;
    }

    public void UpdateSwordDamage(int newSwordDamage)
    {
        swordDamage = newSwordDamage;
    }
    #endregion

    #region HANDLING ATTACK INPUT
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
            //Debug.Log("We're starting the combo");
            // As this this an unlocked move and the beginning
            // of a new combo, add it and return
            AddMoveToCombo(ComboMoves.HeavyAttack1);
            return;
        }

        //Debug.Log("About to loop through the combo list");
        // Let's see what attack they were planning
        for (int i = 0; i < movesThisCombo.Length; i++)
        {
            //Debug.Log("Checking combo position " + i + " it's current value is " + movesThisCombo[i]);
            if (movesThisCombo[i] == ComboMoves.HeavyAttack1)
            {
                heavyAttack1InCombo = true;
                //Debug.LogError("Heavy1 is in the combo already");
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

            //Debug.Log("There's already a heavy1 in the combo, trying heavy2");
            // it's unlocked, so let's add it to the combo
            if (!heavyAttack2InCombo)
            {
                AddMoveToCombo(ComboMoves.HeavyAttack2);
            }
        }
    }

    public void PlayerHasPressedSuperHeavyAttack()
    {
        if (!superHeavyUnlocked || startedCombo || comboPointer > 0)
            return;

        playerAnimationController.PlayerAttack(ComboMoves.SuperHeavyAttack);
    }

    public void AddMoveToCombo(ComboMoves newMove)
    {
        if (comboPointer >= movesThisCombo.Length)
            return;

        //Debug.LogError("Adding " + newMove + " to the combo");
        if (!startedCombo)
        {
            if (newMove == ComboMoves.LightAttack)
            {
                comboTimeLeft = lightAttackLength;
            }
            else if (newMove == ComboMoves.HeavyAttack1)
            {
                comboTimeLeft = heavyAttack1Length;
            }
            else if (newMove == ComboMoves.HeavyAttack2)
            {
                comboTimeLeft = heavyAttack2Length;
            }
        }

        startedCombo = true;
        movesThisCombo[comboPointer] = newMove;
        comboPointer++;


        playerAnimationController.PlayerAttack(newMove);
        playerInputController.DisableMovementInput();
    }

    #endregion

    private void Update()
    {
        if (startedCombo)
        {
            comboTimeLeft -= Time.deltaTime;
            //Debug.Log("The combo has been started. The combo timer is now " + comboTimeLeft);
            if (comboTimeLeft <= 0f)
            {
                FinishCombo();
            }
        }
    }

    public void HitMonster(GameObject monster)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        int damageToInflict = lightAttackDamage + bonusDamage + swordDamage;

        if (stateInfo.IsName("HeavyAttack1"))
        {
            damageToInflict = heavyAttack1Damage + bonusDamage + swordDamage;
        }
        else if (stateInfo.IsName("HeavyAttack2"))
        {
            damageToInflict = heavyAttack2Damage + bonusDamage + swordDamage;
        }

        monster.GetComponent<MonsterHealthScript>().TakeDamage(damageToInflict);
    }

    private void FinishCombo()
    {
        //Debug.Log("FinishCombo has been called");
        comboPointer = 0;
        startedCombo = false;

        // Clear out all of the moves from the combo list
        for (int i = 0; i < movesThisCombo.Length; i++)
        {
            movesThisCombo[i] = ComboMoves.Unset;
        }

        playerAnimationController.ClearAllAttackAnimations();
        playerInputController.EnableMovementInput();
    }

    #region ANIMATION EVENTS AND STATE CALLBACKS

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
        monstersThatWereHit = new List<GameObject>();

        // Check for hits
        lightAttackHits = Physics.OverlapBox(transform.position + (transform.forward), new Vector3(2f, 2f, 2f));
        for (int i = 0; i < lightAttackHits.Length; i++)
        {
            if (lightAttackHits[i].CompareTag("Monster"))
            {
                if (!monstersThatWereHit.Contains(lightAttackHits[i].gameObject))
                {
                    monstersThatWereHit.Add(lightAttackHits[i].gameObject);
                }
            }
        }

        for (int i = 0; i < monstersThatWereHit.Count; i++)
        {
            monstersThatWereHit[i].GetComponent<MonsterHealthScript>().TakeDamage(lightAttackDamage + bonusDamage);
        }
    }

    public void HeavyAttackStarted()
    {
        swordCollider.enabled = true;
    }

    public void HeavyAttackEnded()
    {
        swordCollider.enabled = false;
    }

    public void EnteredMoveClip(int moveNumber)
    {
        switch (moveNumber)
        {
            case 0:
                comboTimeLeft = lightAttackLength;
                break;
            case 1:
                comboTimeLeft = heavyAttack1Length;
                break;
            case 2:
                comboTimeLeft = heavyAttack2Length;
                break;
            default:
                break;
        }
        //moveStartedPointer++;
        //Debug.LogWarning("moveStartedPointer = " + moveStartedPointer);
    }

    public void ExitedMoveClip()
    {
        //Debug.LogWarning(this + " called");
        //moveEndedPointer++;
        //Debug.LogWarning("Calling the FinsihCombo Method, moveEndedPointer = " +moveEndedPointer);
        //FinishCombo();
    }

    #endregion
}
