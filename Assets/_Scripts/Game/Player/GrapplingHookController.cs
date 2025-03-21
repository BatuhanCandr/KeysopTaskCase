using UnityEngine;

public class GrapplingHookController : MonoBehaviour
{
    public SpriteRenderer arrowSprite;
    public GameObject hook;
    public Joystick joystick;
    public float radius = 1.3f;
    public float smoothSpeed = 10f;
    public float raycastDistance = 5f;
    public Transform target;

    private Vector3 targetPosition;
    public LineRenderer lineRenderer;
    public bool isGrappling;
    private bool isLineExtended;
    public float grapplingSpeed = 5f;
    public float lineExtendSpeed = 10f;

    void Update()
    {
        if (joystick == null || GameManager.Instance.playerController.transform == null) return;

        Vector2 direction = GetJoystickDirection();
        
        if (direction.sqrMagnitude > 0.1f)
        {
            arrowSprite.enabled = true;
            Vector3 desiredPosition = CalculateDesiredPosition(direction);
            MoveArrowToTarget(desiredPosition);
            RotateArrowToDirection(direction);
        }
        else
        {
            arrowSprite.enabled = false;
        }

        if (isGrappling)
        {
            if (!isLineExtended)
            {
                ExtendLineRenderer();
            }
            else
            {
                MovePlayerToTarget();
            }
        }
    }

    private Vector2 GetJoystickDirection()
    {
        return new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private Vector3 CalculateDesiredPosition(Vector2 direction)
    {
        direction.Normalize();
        float xOffset = Mathf.Clamp(direction.x * radius, -1f, 1f);
        float yOffset = Mathf.Clamp(direction.y * radius, -1.3f, 1f);
        return GameManager.Instance.playerController.transform.position + new Vector3(xOffset, yOffset, 0);
    }

    private void MoveArrowToTarget(Vector3 desiredPosition)
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }

    private void RotateArrowToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle + 270), Time.deltaTime * smoothSpeed);
    }

    public void GetGrapplingHook()
    {
        ResetLineRenderer();
        isGrappling = true;
        isLineExtended = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, raycastDistance);
        if (hit.collider != null)
        {
            targetPosition = hit.point;
        }
        else
        {
            hook.SetActive(false);
            isGrappling = false;
            return;
        }
    }

    private void ExtendLineRenderer()
    {
        lineRenderer.enabled = true;
        hook.SetActive(true);
        lineRenderer.SetPosition(0, GameManager.Instance.playerController.transform.position);
        lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), targetPosition, lineExtendSpeed * Time.deltaTime));
        hook.transform.position = Vector3.Lerp(hook.transform.position, targetPosition, lineExtendSpeed * Time.deltaTime);
        
        if (Vector2.Distance(lineRenderer.GetPosition(1), targetPosition) < 0.1f)
        {
            isLineExtended = true;
        }
      
    }

    private void MovePlayerToTarget()
    {
        hook.transform.SetParent(null);
        lineRenderer.SetPosition(0, GameManager.Instance.playerController.transform.position);
        Vector2 newPosition = Vector2.MoveTowards(GameManager.Instance.playerController.m_Rigidbody2D.position, targetPosition, grapplingSpeed * Time.deltaTime);
        GameManager.Instance.playerController.m_Rigidbody2D.MovePosition(newPosition);
        
        if (Vector2.Distance(GameManager.Instance.playerController.m_Rigidbody2D.position, targetPosition) < 1.5f)
        {
            ResetLineRenderer();
            isGrappling = false;
        }
    }

    private void ResetLineRenderer()
    {
        lineRenderer.SetPosition(0, GameManager.Instance.playerController.transform.position);
        lineRenderer.SetPosition(1, transform.position);
        hook.transform.SetParent(transform);
        hook.SetActive(false);
        hook.transform.position = Vector3.zero;
        lineRenderer.enabled = false;
    }
}