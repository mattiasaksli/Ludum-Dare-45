using UnityEngine;

public class AnimatorResetBoolAtEnd : StateMachineBehaviour
{
    [SerializeField]
    private string[] booleanVariableName;


    // OnStateExit is called when a transition ends and the state 
    //machine finishes evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (string s in booleanVariableName)
        {
            animator.SetBool(s, false);
        }
    }


}
