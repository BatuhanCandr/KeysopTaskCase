using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    internal void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }

    internal void GroundCheckAnim(bool isGround)
    {
        animator.SetBool("IsGrounded", isGround);
    }

    internal void RunAnim(float speed)
    {
        animator.SetFloat("Run", speed);
    }

    public void DigSideAnim()
    {
        
        if (!GameManager.Instance.playerController.isDigging)
        {
            animator.SetTrigger("DigSide");
            GameManager.Instance.digController.SetTrueScythe(0);
        }
        
        
    }

    public void DigDownAnim()
    {
        if (!GameManager.Instance.playerController.isDigging)
        {
            animator.SetTrigger("DigDown");
            GameManager.Instance.digController.SetTrueScythe(1);
        }
        
    }
    
}