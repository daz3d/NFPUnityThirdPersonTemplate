#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFPPlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public GameObject followTarget;

    public float rotationPower;

    public float normalizeDelay;
    public float normalizeSpeed;

    private Vector3 velocity;
    private bool isGrounded;
    private float timeBeforeNormalize;

    private Quaternion originalFollowRotation;

#if ENABLE_INPUT_SYSTEM
    InputAction movement;
    InputAction jump;
#endif
    void Start()
    {
        originalFollowRotation = followTarget.transform.localRotation;
#if ENABLE_INPUT_SYSTEM
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        
        jump = new InputAction("PlayerJump", binding: "<Gamepad>/a");
        jump.AddBinding("<Keyboard>/space");

        movement.Enable();
        jump.Enable();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        float x;
        float z;
        bool jumpPressed = false;

#if ENABLE_INPUT_SYSTEM
        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
        jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);
#else
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jumpPressed = Input.GetButtonDown("Jump");
#endif

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        var characterAnimator = gameObject.GetComponentInChildren<Animator>();
        if (characterAnimator != null)
        {
            var horzMove = move;
            horzMove.y = 0;

            var horzVelocityMag = horzMove.magnitude;
            characterAnimator.SetFloat("Speed", horzVelocityMag);

            //Debug.Log("Speed: " + horzVelocityMag + " Input: " + x + "," + z);
        }

        bool rightMousePressed = false;

#if ENABLE_INPUT_SYSTEM
        rightMousePressed = Mouse.current.rightButton.isPressed;
#else
        rightMousePressed = Input.GetMouseButton(1);
#endif

        if (rightMousePressed)
        {
            timeBeforeNormalize = normalizeDelay;

            var mouseSensitivity = 100f;
            float mouseX, mouseY;

#if ENABLE_INPUT_SYSTEM
            mouseSensitivity = 50f;
            mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
            mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;
#else
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
#endif

            followTarget.transform.rotation *= Quaternion.AngleAxis(mouseX * rotationPower, Vector3.up);

            followTarget.transform.rotation *= Quaternion.AngleAxis(mouseY * rotationPower, Vector3.right);

            var angles = followTarget.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTarget.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            followTarget.transform.localEulerAngles = angles;
        }
        else
        {
            timeBeforeNormalize -= Time.deltaTime;

            if (timeBeforeNormalize < 0)
            {
                followTarget.transform.localRotation = Quaternion.RotateTowards(followTarget.transform.localRotation, originalFollowRotation, normalizeSpeed * Time.deltaTime);
            }
        }
    }
}
