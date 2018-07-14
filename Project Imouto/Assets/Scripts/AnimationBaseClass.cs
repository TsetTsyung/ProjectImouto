using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBaseClass : MonoBehaviour {

    public abstract void StartWalkAnimation();
    public abstract void StartRunAnimation();
    public abstract void StartAttackAnimation();
    public abstract void StartDeathAnimation();
    public abstract void StartIdlingAnimation();
    public abstract float GetAttackAnimationClipTime();
    public abstract float GetTimeIntoAnimationForAttack();
}
