using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Animator animator;

    //instanceÇ≈éQè∆Ç∑ÇÈÇΩÇﬂ
    public static AnimationEvent instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //AnimatorÇÃèâä˙âª
        animator = PlayerMotion.instance.Animator;
    }
    public void Run1FinishEvent()
    {
        animator.SetBool("run1", false);
        Debug.Log("run1");
    }
    public void Run2FinishEvent()
    {
        animator.SetBool("run2", false);
        Debug.Log("run2");
    }
    public void Walk1FinishEvent()
    {
        animator.SetBool("walk1", false);
        Debug.Log("walk1");
    }
    public void Walk2FinishEvent()
    {
        animator.SetBool("walk2", false);
        Debug.Log("walk2");
    }
    public void Strafe1FinishEvent()
    {
        animator.SetBool("strafe1", false);
        Debug.Log("strafe1");
    }
    public void Strafe2FinishEvent()
    {
        animator.SetBool("strafe2", false);
        Debug.Log("strafe2");
    }
    public void JogForwardDiagonal1FinishEvent()
    {
        animator.SetBool("Jog Forward Diagonal", false);
        Debug.Log("Jog Forward Diagonal");
    }
    public void JogForwardDiagonal2FinishEvent()
    {
        animator.SetBool("Jog Forward Diagonal (1)", false);
        Debug.Log("Jog Forward Diagonal (1)");
    }
    public void JogBackwardDiagonal1FinishEvent()
    {
        animator.SetBool("Jog Backward Diagonal", false);
        Debug.Log("Jog Backward Diagonal");
    }
    public void JogBackwardDiagonal2FinishEvent()
    {
        animator.SetBool("Jog Backward Diagonal (1)", false);
        Debug.Log("Jog Backward Diagonal (1)");
    }

    public void AllFalseEvent()
    {
        animator.SetBool("run1", false);
        animator.SetBool("run2", false);
        animator.SetBool("walk1", false);
        animator.SetBool("walk2", false);
        animator.SetBool("strafe1", false);
        animator.SetBool("strafe2", false);
        animator.SetBool("Jog Forward Diagonal", false);
        animator.SetBool("Jog Forward Diagonal (1)", false);
        animator.SetBool("Jog Backward Diagonal", false);
        animator.SetBool("Jog Backward Diagonal (1)", false);
        Debug.Log("AllFalseEvent");
    }

    public void InitializeAnimationEvent()
    {
        animator.SetBool("run1", false);
        animator.SetBool("run2", false);
        animator.SetBool("walk1", false);
        animator.SetBool("walk2", false);
        animator.SetBool("strafe1", false);
        animator.SetBool("strafe2", false);
        animator.SetBool("Jog Forward Diagonal", false);
        animator.SetBool("Jog Forward Diagonal (1)", false);
        animator.SetBool("Jog Backward Diagonal", false);
        animator.SetBool("Jog Backward Diagonal (1)", false);
        animator.SetBool("death1", false);
    }
}
