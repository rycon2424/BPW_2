using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Animator anim;
    public Transform cameraObject;
    public Transform rangedView;
    public GameObject crosshair;

    [Header("Movement")]
    public bool canMove;
    public bool grounded;
    public float moveSpeed;
    public float sprintSpeed;
    private float normalSpeed;
    public float jumpForce = 8.0f;
    public float gravity = 2.5f;
    
    [Space]

    public bool canJump;
    public Vector3 movement;
    public Vector3 lookoffset;

    [HideInInspector] public Transform chest;
    [HideInInspector] public CharacterController characterController;

    private State currentState;
    private OrbitCamera oc;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        oc = FindObjectOfType<OrbitCamera>();
        normalSpeed = moveSpeed;
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        canJump = true;

        SetupStateMachine();
    }

    void SetupStateMachine()
    {
        Locomotion lm = new Locomotion();
        Aiming am = new Aiming();

        StateMachine.allStates.Add(lm);
        StateMachine.allStates.Add(am);
        StateMachine.GoToState(this, "Locomotion");
    }

    void Update()
    {
        currentState.StateUpdate(this);
    }

    void LateUpdate()
    {
        currentState.StateLateUpdate(this);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void SetCamDistanceAndTarget(float newDistance, Transform target)
    {
        oc.distance = newDistance;
        if (target != null)
        {
            oc.viewTarget = target;
        }
    }

    public void CanJumpAgain(PlayerBehaviour pb)
    {
        canJump = true;
    }

}