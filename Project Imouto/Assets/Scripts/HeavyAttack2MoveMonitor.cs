using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack2MoveMonitor : StateMachineBehaviour {
    /*
    private PlayerAttackScript playerAttackScript;

    public void AssignAttackScriptReference(PlayerAttackScript _playerAttackScript)
    {
        Debug.Log("received the reference for the attack script");
        playerAttackScript = _playerAttackScript;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log("OnStateEnter on " + this + " has been called");
        playerAttackScript.EnteredMove(ComboMoves.HeavyAttack2);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.LogWarning("Apparently leaving HeavyAttack2");
        playerAttackScript.ExitedMove();
    }
    */
    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
