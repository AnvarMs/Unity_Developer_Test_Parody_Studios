using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator animator;
    public CharacterController controller;
    public Camera m_camera;
    
    public float moveSpeed =5;
    public float JumpForce = 10;
    public float gravity = 20f;

    // Input 
     PlayerInput p_input; 

    // private

    Vector3 currentGravityDirection = Vector3.down;
    private Vector2 input;
    private Vector3 moveDirection; 
    void Awake()
    {
        p_input = new PlayerInput();
        p_input.Enable();
    }
    void OnDestroy()
    {
        p_input.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        controller = GetComponent<CharacterController>();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
        HandleMovement();
    }


    private void HandleMovement()
    {

        input = p_input.Player.Move.ReadValue<Vector2>();


        moveDirection = (m_camera.transform.forward * input.y +m_camera.transform.right *input.x).normalized;
       
        controller.Move(moveDirection*moveSpeed*Time.deltaTime);

        if (moveDirection.magnitude > 0.1f)
        {
            if(!animator.GetBool("running")){
            animator.SetBool("running",true);
            animator.SetBool("idle",false);
            }
        }
        else
        {
             if(!animator.GetBool("idle")){
            animator.SetBool("running",false);
            animator.SetBool("idle",true);
            }
        }
    }

    void HandleRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, -currentGravityDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,0.05f);
    }
}
