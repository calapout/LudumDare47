using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bytes;

public class EntityAnimationController : MonoBehaviour
{
    public class BaseCreatureAnimState : BaseAnimState
    {
        public static readonly BaseCreatureAnimState Walk = new BaseCreatureAnimState("walk", 1f);
        public static readonly BaseCreatureAnimState Attack = new BaseCreatureAnimState("attack", 1f, 1);
        public static readonly BaseCreatureAnimState Die = new BaseCreatureAnimState("die", 1f, -1);
        public static readonly BaseCreatureAnimState Idle = new BaseCreatureAnimState("idle", 1f, -1);

        protected BaseCreatureAnimState(string pClipName, float pSpeed, int pNbVariations = -1) : base(pClipName, pSpeed, pNbVariations)
        { }
    }

    protected GenericAnimationStateMachine _AnimStateMachine;
    [SerializeField] protected string animPrefix;
    [SerializeField] protected Collider2D[] attackColliders;
    [SerializeField] protected bool testAnims = false;

    private void Start()
    {
        _AnimStateMachine = GetComponent<GenericAnimationStateMachine>();
        StopMovingAnimation();

        if (testAnims)
        {
            Animate.Delay(2f, StartMovingAnimation);
            Animate.Delay(4f, PlayAttackAnim);
            Animate.Delay(8f, () => { PlayDieAnim(() => { Destroy(gameObject); }); });
        }
    }

    /// <summary>
    /// Used by animator events.
    /// </summary>
    public void SetAttackCollidersActive()
    {
        SetAttackCollidersActive_Private(true);
    }

    /// <summary>
    /// Used by animator events.
    /// </summary>
    public void SetAttackCollidersInactive()
    {
        SetAttackCollidersActive_Private(false);
    }

    private void SetAttackCollidersActive_Private(bool val)
    {
        foreach (Collider2D col in attackColliders)
        {
            col.enabled = val;
        }
    }

    public void StartMovingAnimation()
    {
        _AnimStateMachine.SetLoopedState(BaseCreatureAnimState.Walk, animPrefix, true);
    }

    /// <summary>
    /// Also starts Idle animation.
    /// </summary>
    public void StopMovingAnimation()
    {
        _AnimStateMachine.SetLoopedState(BaseCreatureAnimState.Idle, animPrefix, true);
    }

    public void PlayDieAnim(System.Action callback, bool sound = false)
    {
        _AnimStateMachine.enabled = false;
        Utils.PlayAnimatorClip(GetComponent<Animator>(), animPrefix + "_die", callback);
        if(sound) PlayAccordingSound(BaseCreatureAnimState.Die.ClipName);
    }

    public void PlayAttackAnim()
    {
        _AnimStateMachine.PlayAnimOnce(BaseCreatureAnimState.Attack, animPrefix);
        PlayAccordingSound(BaseCreatureAnimState.Attack.ClipName);
    }

    // Should be called by a skill
    public void PlayCustomAnim(string animName)
    {
        _AnimStateMachine.PlayAnimOnceCustom(animName, animPrefix);
    }

    private void PlayAccordingSound(string suffix)
    {
        EventManager.Dispatch("playSound", new PlaySoundData(animPrefix + "_" + suffix));
    }

}
