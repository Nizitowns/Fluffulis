using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourScript : StateMachineBehaviour
{
    [SerializeField]
    private float timeUntilIdle;

    [SerializeField]
    private int _numberOfIdleAnimations;
    private bool _isIdle;
    private float _idleTime;
    private int _IdleAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isIdle == false) 
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > timeUntilIdle && stateInfo.normalizedTime % 1 < 0.02f ) 
            {
                _isIdle = true;
                _IdleAnimation = Random.Range(1, _numberOfIdleAnimations + 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > .98)
        {
            ResetIdle();
        }

        animator.SetFloat("StretchAnimation", _IdleAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle() 
    {
        _isIdle = false;
        _idleTime = 0;
        _IdleAnimation = 0;

    }

}
