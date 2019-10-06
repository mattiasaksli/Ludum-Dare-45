using UnityEngine;

public class ElderCountAtIdle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("count", animator.GetInteger("count") + 1);
    }
}
