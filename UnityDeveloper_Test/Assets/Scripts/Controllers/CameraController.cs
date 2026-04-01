using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Camera Distance")]
    public float distance = 4f;
    public float height = 2f;
    
    [Header("Mouse Input")]
    public float mouseSensitivity = 100f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    
    [Header("Smoothing")]
    public float smoothTime = 0.3f;
    public float rotSmoothTime = 0.1f;
    
    [Header("Collision")]
    public LayerMask collisionMask;
    public float collisionBuffer = 0.2f;
    
    // Private
    private Vector3 posVelocity = Vector3.zero;
    private PlayerController playerController;
    
    private float currentYaw = 0f;   // Horizontal rotation
    private float currentPitch = 0f; // Vertical rotation
    private bool isSelectingGravity=false;

    public bool IsSelectingGravity
    {
        set{

            isSelectingGravity = value;
            
        }
    }
    
    void Start()
    {
        if (target != null)
        {
            playerController = target.GetComponentInParent<PlayerController>();
        }
        
        // Lock cursor for better control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialize angles based on current rotation
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;
    }
    
    void LateUpdate()
    {
        if (target == null || playerController == null) return;
        
        HandleMouseInput();
        UpdateCameraPosition();
    }
    
    private void HandleMouseInput()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Update angles
        currentYaw += mouseX;
        currentPitch -= mouseY; // Inverted for natural feel
        
        // Clamp vertical angle
        currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
    }
    
      void UpdateCameraPosition()
    {
        // Get gravity-based up direction
        Vector3 gravityUp = -playerController.GetCurrentGravityDirection();
        
        // Build rotation relative to gravity and player orientation
        Vector3 baseForward = target.forward;
        
        // Apply yaw rotation around gravity's up axis
        Quaternion yawRotation = Quaternion.AngleAxis(currentYaw, gravityUp);
        Vector3 yawedForward = yawRotation * baseForward;
        
        // Calculate right axis for pitch
        Vector3 rightAxis = Vector3.Cross(gravityUp, yawedForward).normalized;
        
        // Apply pitch rotation around right axis
        Quaternion pitchRotation = Quaternion.AngleAxis(currentPitch, rightAxis);
        Vector3 finalForward = pitchRotation * yawedForward;
            finalForward = isSelectingGravity?target.forward: finalForward;
        // Calculate desired camera position
        Vector3 offset = -finalForward * distance + gravityUp * height;
        Vector3 desiredPosition = target.position + offset;
        
        // Collision check
        Vector3 directionToCamera = desiredPosition - target.position;
        float maxDistance = directionToCamera.magnitude;
        
        if (Physics.Raycast(target.position, directionToCamera.normalized, out RaycastHit hit, maxDistance, collisionMask))
        {
            desiredPosition = hit.point - directionToCamera.normalized * collisionBuffer;
        }
        
        // Smooth position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref posVelocity,
            smoothTime
        );
        
        // Look at target with gravity-aware up
        Vector3 lookDirection = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, gravityUp);
        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            1f - Mathf.Exp(-rotSmoothTime * 10f * Time.deltaTime)
        );
    }
    
    
    // Call this to get camera's forward direction (useful for player movement)
    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward = Vector3.ProjectOnPlane(forward, -playerController.GetCurrentGravityDirection());
        return forward.normalized;
    }
    
    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right = Vector3.ProjectOnPlane(right, -playerController.GetCurrentGravityDirection());
        return right.normalized;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position);
        
        if (playerController != null)
        {
            Vector3 upDirection = -playerController.GetCurrentGravityDirection();
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            Vector3 offset = rotation * new Vector3(0f, height, -distance);
            Vector3 desiredPos = target.position + offset;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(desiredPos, 0.3f);
        }
    }
}