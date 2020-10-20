using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Animator anim;
    public Transform cameraObject;

    [Header("Movement")]
    public bool canMove;
    public bool grounded;
    public float moveSpeed;
    public float sprintSpeed;
    private float normalSpeed;
    public float jumpForce = 8.0f;
    public float gravity = 2.5f;

    private bool canJump;

    [Space]

    public Vector3 movement;
    
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        normalSpeed = moveSpeed;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        canJump = true;
    }

    void Update()
    {
        MovementAndJump();
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            RotateTowardsCamera();
        }
    }

    float moveHorizontal;
    float moveVertical;
    void MovementAndJump()
    {
        float yMove = movement.y;
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        
        float sprintspeedlocal;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintspeedlocal = sprintSpeed;
            anim.SetBool("Running", true);
        }
        else
        {
            sprintspeedlocal = 0;
            anim.SetBool("Running", false);
        }

        movement = (transform.forward * moveVertical) + (transform.right * moveHorizontal);
        movement = movement.normalized * (moveSpeed + sprintspeedlocal);
        movement.y = yMove;

        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }

        anim.SetBool("IsGrounded", HitGround());
        grounded = HitGround();

        if (characterController.isGrounded || HitGround())
        {
            if (Input.GetButtonDown("Jump") && canJump)
            {
                anim.SetTrigger("Jump");
                canJump = false;
            }
        }
        
        // Apply gravity
        movement.y = movement.y + (Physics.gravity.y * gravity * Time.deltaTime);

        if (canMove)
            characterController.Move(movement * Time.deltaTime);
        
        return;
    }

    bool HitGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down * 0.6f, Color.red, 1);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 0.6f))
        {
            //Debug.Log(hit.collider.name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RotateTowardsCamera()
    {
        var CharacterRotation = cameraObject.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, CharacterRotation, Time.deltaTime * 8);
    }

    public void CanJumpAgain()
    {
        canJump = true;
    }
}