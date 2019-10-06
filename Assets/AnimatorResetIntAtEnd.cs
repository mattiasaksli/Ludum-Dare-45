using UnityEngine;

public class AnimatorResetIntAtEnd : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("count", 0);
    }
}
