using UnityEngine;

public class GrapplingHookController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer arrowSprite;
    public GameObject hook;
    public Joystick joystick;
    public LineRenderer lineRenderer;
    public Transform target;

    [Header("Settings")]
    public float radius = 1.3f;
    public float smoothSpeed = 10f;
    public float raycastDistance = 5f;
    public float grapplingSpeed = 5f;
    public float lineExtendSpeed = 10f;

    [Header("State")]
    private Vector3 targetPosition;
    public bool isGrappling;
    private bool isLineExtended;
    void Update()
    {
        if (joystick == null || GameManager.Instance.playerController.transform == null) return;

        // Get joystick direction
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

    // Get the direction of the joystick
    private Vector2 GetJoystickDirection()
    {
        return new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    // Calculate the target position for the arrow based on joystick direction
    private Vector3 CalculateDesiredPosition(Vector2 direction)
    {
        direction.Normalize();
        float xOffset = Mathf.Clamp(direction.x * radius, -1f, 1f);
        float yOffset = Mathf.Clamp(direction.y * radius, -1.3f, 1f);
        return GameManager.Instance.playerController.transform.position + new Vector3(xOffset, yOffset, 0);
    }

    // Move the arrow to the desired position smoothly
    private void MoveArrowToTarget(Vector3 desiredPosition)
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }

    // Rotate the arrow to match the joystick direction
    private void RotateArrowToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle + 270), Time.deltaTime * smoothSpeed);
    }

    // Trigger the grappling hook mechanism
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

    // Extend the line renderer towards the target position
    private void ExtendLineRenderer()
    {
        lineRenderer.enabled = true;
        hook.transform.position = lineRenderer.GetPosition(1);

        hook.SetActive(true);
        Vector3 direction = targetPosition - hook.transform.position;
        hook.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        lineRenderer.SetPosition(0, GameManager.Instance.playerController.transform.position);
        lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), targetPosition, lineExtendSpeed * Time.deltaTime));
        hook.transform.position = Vector3.Lerp(hook.transform.position, lineRenderer.GetPosition(1), lineExtendSpeed * 2.5f * Time.deltaTime);

        if (Vector2.Distance(lineRenderer.GetPosition(1), targetPosition) < 0.1f)
        {
            isLineExtended = true;
        }
    }

    // Move the player towards the target position after line extension
    private void MovePlayerToTarget()
    {
        hook.transform.SetParent(null);
        lineRenderer.SetPosition(0, GameManager.Instance.playerController.transform.position);
        Vector2 newPosition = Vector2.MoveTowards(GameManager.Instance.playerController.rb.position, targetPosition, grapplingSpeed * Time.deltaTime);
        GameManager.Instance.playerController.rb.MovePosition(newPosition);

        if (Vector2.Distance(GameManager.Instance.playerController.rb.position, targetPosition) < 1.5f)
        {
            ResetLineRenderer();
            isGrappling = false;
        }
    }

    // Reset the line renderer and hook position
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
