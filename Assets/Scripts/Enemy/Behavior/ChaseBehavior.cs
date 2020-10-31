using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehavior : StateMachineBehaviour
{
    [SerializeField] float searchTime = 5;

    EnemyAI ai;
    float stopSearchTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        ai = animator.GetComponent<EnemyAI>();
        stopSearchTime = Time.time + searchTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stopSearchTime < Time.time) {
            ai.Stop();
            ai.target = null;
            animator.SetBool("enemySpotted", false);
        }
        else {
            if (ai.IsTargetVisible()) {
                if (ai.IsTargetInRange()) {
                    ai.Stop();
                }
                else {
                    ai.MoveTo(ai.target.position);
                }
                stopSearchTime = Time.time + searchTime;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
