using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public CameraController cameraController;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rotationSpeed = 10f;
    
    [Header("Physics")]
    public float customGravity = 20f;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.3f;
    
    [Header("Ground Check")]
    public Transform groundCheck; // Empty GameObject at feet position
    
    // Components
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    
    // Input
    private PlayerInput p_input;
    private Vector2 input;
    
    // State
    private Vector3 currentGravityDirection = Vector3.down;
    private bool isGrounded;
    private Vector3 moveDirection;
    
    void Awake()
    {
        p_input = new PlayerInput();
        p_input.Enable();
        p_input.Player.Jump.started += Jump;
        
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        // Configure Rigidbody
        rb.useGravity = false; // We'll handle gravity manually
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent physics rotation
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
    void OnDestroy()
    {
        p_input.Disable();
    }
    
    void Update()
    {
        CheckGroundStatus();
        HandleInput();
        HandleRotation();
        UpdateAnimations();
    }
    
    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
    }
    
    private void CheckGroundStatus()
    {
        
        
        // Alternative: Use sphere cast for more reliable detection
        isGrounded = Physics.SphereCast(
            groundCheck.position,
            0.1f,
            currentGravityDirection,
            out RaycastHit hit,
            groundCheckDistance,
            groundMask
        );
    }
    
    private void HandleInput()
    {
        input = p_input.Player.Move.ReadValue<Vector2>();
        
        if (cameraController != null)
        {
            // Get camera directions (already projected on gravity plane)
            Vector3 cameraForward = cameraController.GetCameraForward();
            Vector3 cameraRight = cameraController.GetCameraRight();
            
            // Calculate movement direction relative to camera
            moveDirection = (cameraForward * input.y + cameraRight * input.x).normalized;
        }
        else
        {
            // Fallback if no camera reference
            moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        }
    }
    
    private void ApplyMovement()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            // Move in the desired direction
            Vector3 movement = moveDirection * moveSpeed;
            
            // Preserve vertical velocity (for jumping/falling)
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, currentGravityDirection);
            Vector3 horizontalVelocity = movement;
            
            rb.linearVelocity = horizontalVelocity + verticalVelocity;
        }
        else
        {
            // Stop horizontal movement, keep vertical
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, currentGravityDirection);
            rb.linearVelocity = verticalVelocity;
        }
    }
    
    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            // Apply custom gravity
            rb.linearVelocity += currentGravityDirection * customGravity * Time.fixedDeltaTime;
        }
        else
        {
            // Small force to keep grounded
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, currentGravityDirection);
            if (Vector3.Dot(verticalVelocity, currentGravityDirection) < 0)
            {
                rb.linearVelocity -= verticalVelocity;
                rb.linearVelocity += currentGravityDirection * 2f;
            }
        }
    }
    
    private void HandleRotation()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            // GTA 5 Style: Character faces movement direction
            Quaternion targetRotation = Quaternion.LookRotation(
                moveDirection,              // Face movement direction
                -currentGravityDirection    // Up direction based on gravity
            );
            
            // Smooth rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    
    private void UpdateAnimations()
    {
        if (animator == null) return;
        
        bool isMoving = moveDirection.magnitude > 0.1f;
        
        animator.SetBool("running", isMoving);
        animator.SetBool("idle", !isMoving);
        animator.SetBool("jumping", !isGrounded);
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            // Jump opposite to gravity direction
            Vector3 jumpVelocity = -currentGravityDirection * jumpForce;
            
            // Remove existing vertical velocity and add jump
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, currentGravityDirection);
            rb.linearVelocity -= verticalVelocity;
            rb.linearVelocity += jumpVelocity;
        }
    }
    
    public void SetGravityDirection(Vector3 newDirection)
    {
        currentGravityDirection = newDirection.normalized;
        
        // Rotate character to align with new gravity
        StartCoroutine(RotateToNewGravity());
    }
    
    private System.Collections.IEnumerator RotateToNewGravity()
    {
        Quaternion startRotation = transform.rotation;
        Vector3 currentForward = transform.forward;
        
        // Keep character facing the same direction, but change up vector
        Quaternion targetRotation = Quaternion.LookRotation(currentForward, -currentGravityDirection);
        
        float elapsed = 0f;
        float duration = 0.5f;
        
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = targetRotation;
    }
    
    public Vector3 GetCurrentGravityDirection()
    {
        return currentGravityDirection;
    }
    
    void OnDrawGizmos()
    {
        // Draw gravity direction
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, currentGravityDirection * 2f);
        
        // Draw character's up
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up * 2f);
        
        // Draw movement direction
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, moveDirection * 2f);
        
        // Draw ground check
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
            Gizmos.DrawRay(groundCheck.position, currentGravityDirection * groundCheckDistance);
        }
    }
}