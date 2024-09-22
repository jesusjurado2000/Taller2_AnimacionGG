using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float fastRunSpeed = 8f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public Transform cameraTransform; 
    private CharacterController controller;
    private Animator animator;

    private bool isJumping = false;
    private float verticalVelocity = 0f;
    public float gravity = -9.81f; 
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir.normalized * fastRunSpeed * Time.deltaTime);
                animator.SetBool("FastRun", true);
                animator.SetBool("SlowRun", false);
                animator.SetBool("Walking", false);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                controller.Move(moveDir.normalized * walkSpeed * Time.deltaTime);
                animator.SetBool("Walking", true);
                animator.SetBool("FastRun", false);
                animator.SetBool("SlowRun", false);
            }
            else
            {
                controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                animator.SetBool("SlowRun", true);
                animator.SetBool("FastRun", false);
                animator.SetBool("Walking", false);
            }
        }
        else
        {
            animator.SetBool("SlowRun", false);
            animator.SetBool("FastRun", false);
            animator.SetBool("Walking", false);
        }
    }

    void HandleJump()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            animator.SetBool("isGrounded", true);
            animator.SetBool("Jump", false);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
            isJumping = true;
            animator.SetBool("Jump", true);
            animator.SetBool("isGrounded", false);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 verticalMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(verticalMove * Time.deltaTime);
    }
}
