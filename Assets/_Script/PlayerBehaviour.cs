using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Animator anim;
    public Transform cameraObject;
    public float horizontal;

    public LayerMask hitGround;
    
    [Header("Movement")]
    public bool canMove;
    public bool grounded;
    public float moveSpeed;
    public float sprintSpeed;
    private float normalSpeed;
    public float jumpForce = 8.0f;
    public float gravity = 2.5f;
    public int fallDuration;

    [Space]

    public bool canJump;
    public Vector3 movement;
    public Vector3 lookoffset;

    [HideInInspector] public Transform chest;
    [HideInInspector] public CharacterController characterController;
    public Transform grabHand;
    public Vector3 hangOffset;
    public float grabHeight;

    private State currentState;
    [HideInInspector] public OrbitCamera oc;

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
        Hanging hg = new Hanging();
        Falling fa = new Falling();
        StateBetween sb = new StateBetween();

        StateMachine.allStates.Add(lm);
        StateMachine.allStates.Add(hg);
        StateMachine.allStates.Add(fa);
        StateMachine.allStates.Add(sb);
        StateMachine.GoToState(this, "Locomotion");
    }

    void Update()
    {
        HitGround();
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

    IEnumerator Falling()
    {
        yield return new WaitForSeconds(1f);
        if (StateMachine.IsInState("Locomotion"))
        {
            StateMachine.GoToState(this, "Falling");
        }
    }

    public void DelayTurnOnRoot(float delay)
    {
        Invoke("DelayedRoot", delay);
    }

    void DelayedRoot()
    {
        anim.applyRootMotion = true;
    }
    
    public void HitGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down * 0.75f, Color.red, 1);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 0.75f))
        {
            if (grounded != true)
            {
                StopCoroutine("Falling");
                grounded = true;
                if (StateMachine.IsInState("Locomotion"))
                {
                    fallDuration = 0;
                }
            }
        }
        else
        {
            if (grounded != false)
            {
                grounded = false;
                StartCoroutine("Falling");
            }
        }
        canJump = grounded;
    }

    public void NextState(string state)
    {
        StateMachine.GoToState(this, state);
    }

    private void FixedUpdate()
    {
        if (!grounded)
        {
            if (StateMachine.IsInState("Locomotion") || StateMachine.IsInState("Falling"))
            {
                fallDuration++;
            }
        }
    }

    public void LerpToPosition(Vector3 pos)
    {
        StartCoroutine(LerpToPos(pos));
    }

    IEnumerator LerpToPos(Vector3 pos)
    {
        float timeElapsed = 0;

        while (timeElapsed < 0.25f)
        {
            transform.position = Vector3.Lerp(transform.position, pos, timeElapsed / 0.25f);
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    float lerping = 0;
    private void OnAnimatorIK()
    {
        if (StateMachine.IsInState("Locomotion") && !grounded)
        {
            if (Input.GetKey(KeyCode.E))
            {
                lerping += 2 * Time.deltaTime;
                anim.SetIKPosition(AvatarIKGoal.RightHand, grabHand.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, grabHand.rotation);

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, lerping));
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, lerping));
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(1, 0, lerping));
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(1, 0, lerping));
            }
        }
        else
        {
            lerping = 0;
        }
    }
}