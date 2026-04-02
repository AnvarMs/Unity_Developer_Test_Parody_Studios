using UnityEngine;
public class GravitySelector : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public PlayerController playerController;
    public GameObject p_Hologram;
    
    [Header("Settings")]
    public float selectionSensitivity = 1f;
    
    // The 6 possible gravity directions
    private Vector3[] gravityDirections = new Vector3[]
    {
        Vector3.down,    // 0
        Vector3.up,      // 1
        Vector3.left,    // 2
        Vector3.right,   // 3
        Vector3.forward, // 4
        Vector3.back     // 5
    };
    
    private CameraController controller;
    private bool isSelecting = false;
    private int selectedDirectionIndex = 0;
    
    // NEW: Store angles instead of vector
    private float accumulatedYaw = 0f;
    private float accumulatedPitch = 0f;

    void Start()
    {
        if(controller == null) 
            controller = playerCamera.GetComponent<CameraController>();
    }
    
    void Update()
    {
        HandleGravitySelection();
    }
    
    void HandleGravitySelection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartSelection();
        }
        
        if (Input.GetMouseButton(1) && isSelecting)
        {
            UpdateSelection();
        }
        
        if (Input.GetMouseButtonUp(1) && isSelecting)
        {
            ApplySelection();
        }
    }
    
    void StartSelection()
    {
        isSelecting = true;
        
        // Reset angles
        accumulatedYaw = 0f;
        accumulatedPitch = 0f;
        
        selectedDirectionIndex = 0;
        p_Hologram.SetActive(true);
        controller.IsSelectingGravity = true;
        
        Debug.Log("Started gravity selection");
    }
    
    void UpdateSelection()
    {
        // Get mouse input this frame
        float mouseX = Input.GetAxis("Mouse X") * selectionSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * selectionSensitivity;
        
        // Accumulate rotation angles
        accumulatedYaw += mouseX;
        accumulatedPitch += mouseY;
        
        // Build direction by rotating camera forward
        Vector3 selectionDirection = CalculateSelectionDirection();
        
        // Find closest gravity direction
        selectedDirectionIndex = FindClosestDirection(selectionDirection);
        
        Debug.Log($"Yaw: {accumulatedYaw:F1}, Pitch: {accumulatedPitch:F1}, Selected: {gravityDirections[selectedDirectionIndex]}");
        
        UpdateHologram();
    }
    
    Vector3 CalculateSelectionDirection()
    {
        // Start with camera's forward direction
        Vector3 baseDirection = playerCamera.transform.forward;
        Vector3 cameraUp = playerCamera.transform.up;
        Vector3 cameraRight = playerCamera.transform.right;
        
        // Apply yaw rotation (horizontal mouse movement)
        Quaternion yawRotation = Quaternion.AngleAxis(accumulatedYaw * 50f, cameraUp);
        Vector3 direction = yawRotation * baseDirection;
        
        // Apply pitch rotation (vertical mouse movement)
        Quaternion pitchRotation = Quaternion.AngleAxis(accumulatedPitch * 50f, cameraRight);
        direction = pitchRotation * direction;
        
        return direction.normalized;
    }
    
    void UpdateHologram()
    {
        Vector3 hologramUp = -gravityDirections[selectedDirectionIndex];
        Vector3 hologramForward = transform.forward;
        
        hologramForward = Vector3.ProjectOnPlane(hologramForward, hologramUp);
        
        if (hologramForward.magnitude < 0.1f)
        {
            hologramForward = Vector3.ProjectOnPlane(transform.right, hologramUp);
        }
        
        hologramForward.Normalize();
        
        Quaternion targetRotation = Quaternion.LookRotation(hologramForward, hologramUp);
        
        p_Hologram.transform.rotation = Quaternion.Slerp(
            p_Hologram.transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
        
        // Position hologram
        p_Hologram.transform.position = transform.position + transform.up * 2f ;
    }
    
    void ApplySelection()
    {
        isSelecting = false;
        p_Hologram.SetActive(false);
        controller.IsSelectingGravity = false;
        
        Vector3 newGravity = gravityDirections[selectedDirectionIndex];
        playerController.SetGravityDirection(newGravity);
        
        Debug.Log($"Applied gravity: {newGravity}");
    }
    
    int FindClosestDirection(Vector3 targetDirection)
    {
        float maxDot = -1f;
        int bestIndex = 0;
        
        for (int i = 0; i < gravityDirections.Length; i++)
        {
            float dot = Vector3.Dot(targetDirection, gravityDirections[i]);
            
            if (dot > maxDot)
            {
                maxDot = dot;
                bestIndex = i;
            }
        }
        
        return bestIndex;
    }
    
    // NEW: Visual debugging
    void OnDrawGizmos()
    {
        if (!isSelecting || playerCamera == null) return;
        
        // Draw all 6 possible directions
        for (int i = 0; i < gravityDirections.Length; i++)
        {
            Gizmos.color = (i == selectedDirectionIndex) ? Color.green : Color.gray;
            Gizmos.DrawRay(transform.position, gravityDirections[i] * 2f);
        }
        
        // Draw the selection direction
        Vector3 selectionDir = CalculateSelectionDirection();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, selectionDir * 3f);
        
        // Draw camera axes for reference
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, playerCamera.transform.right);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, playerCamera.transform.up);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, playerCamera.transform.forward);
    }
}