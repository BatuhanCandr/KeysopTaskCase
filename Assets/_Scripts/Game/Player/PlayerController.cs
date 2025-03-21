using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private FixedJoystick _fixedJoystick;
    [SerializeField] private float m_Run;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] private float deceleration = 10f; // Yavaşlama oranı

   
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

   
    [Header("Components")]
  
    [SerializeField] internal Rigidbody2D m_Rigidbody2D;
    [SerializeField] public PlayerAnimation _playerAnimation;
    
    
    //States
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
        HandleJump();
    }

    void UpdateRunAnimation()
    {
        if (!isGrounded)
        {
            _playerAnimation.RunAnim(0);
            return;
        }

        float horizontalAxis = _fixedJoystick.Horizontal;

        if (Mathf.Abs(horizontalAxis) > 0.1f)
        {
            isMoving = true;
            m_Run = Mathf.Abs(horizontalAxis);
            _playerAnimation.RunAnim(m_Run);
            GameManager.Instance.digController.SetFalseScythe();
            if (horizontalAxis > 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalAxis < 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }

            m_Rigidbody2D.linearVelocity = new Vector2(horizontalAxis * m_RunSpeed, m_Rigidbody2D.linearVelocity.y);
        }
        else
        {
            _playerAnimation.RunAnim(0);
            isMoving = false;
            if (m_Rigidbody2D.linearVelocity.x != 0)
            {
                float decelerationSpeed = Mathf.MoveTowards(m_Rigidbody2D.linearVelocity.x, 0, deceleration * Time.deltaTime);
                m_Rigidbody2D.linearVelocity = new Vector2(decelerationSpeed, m_Rigidbody2D.linearVelocity.y);
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
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
    }

    void Jump()
    {
        isGrounded = false;
        _playerAnimation.GroundCheckAnim(isGrounded);
        GameManager.Instance.digController.SetFalseScythe();
        _playerAnimation.JumpAnim();
        m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, jumpForce);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
          _playerAnimation.GroundCheckAnim(isGrounded);
        }
    }
    
}
