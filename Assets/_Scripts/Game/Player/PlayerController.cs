using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private FixedJoystick _fixedJoystick;
    [SerializeField] private float run;
    [SerializeField] private float runSpeed;
    [SerializeField] private float deceleration = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Components")]
    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] public PlayerAnimation playerAnimation;

    // States
    internal bool isMoving;
    private bool isGrounded;
    private bool canDoubleJump;
    internal bool isDigging;

    private void Update()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        UpdateRunAnimation();
    }

    // Updates run animation and movement behavior
    private void UpdateRunAnimation()
    {
        if (!isGrounded)
        {
            playerAnimation.RunAnim(0);
            return;
        }

        float horizontalAxis = _fixedJoystick.Horizontal;

        if (Mathf.Abs(horizontalAxis) > 0.1f)
        {
            isMoving = true;
            run = Mathf.Abs(horizontalAxis);
            playerAnimation.RunAnim(run);
            GameManager.Instance.digController.SetFalseScythe();

            transform.localScale = new Vector3(horizontalAxis > 0 ? -1 : 1, transform.localScale.y, transform.localScale.z);
            rb.linearVelocity = new Vector2(horizontalAxis * runSpeed, rb.linearVelocity.y);
        }
        else
        {
            playerAnimation.RunAnim(0);
            isMoving = false;
            
            if (rb.linearVelocity.x != 0)
            {
                float decelerationSpeed = Mathf.MoveTowards(rb.linearVelocity.x, 0, deceleration * Time.deltaTime);
                rb.linearVelocity = new Vector2(decelerationSpeed, rb.linearVelocity.y);
            }
        }
    }

    // Handles player jump input
    public void HandleJump()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }
    }

    // Executes the jump action
    private void Jump()
    {
        isGrounded = false;
        playerAnimation.GroundCheckAnim(isGrounded);
        GameManager.Instance.digController.SetFalseScythe();
        playerAnimation.JumpAnim();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Handles collision detection
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            playerAnimation.GroundCheckAnim(isGrounded);
        }

        if (other.gameObject.CompareTag("Deathzone"))
        {
            GameManager.Instance.RestartGame();
        }
    }
}
