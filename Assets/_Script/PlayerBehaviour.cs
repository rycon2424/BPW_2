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
    public bool aiming;
    private float normalSpeed;
    public float jumpForce = 8.0f;
    public float gravity = 2.5f;
    public int fallDuration;

    [Header("Gun")]
    public GameObject gunInHand;
    public GameObject gunHoldster;
    public GameObject crossHairUI;

    [Space]

    public bool jumped;
    public bool canJump;
    public bool sprinting;
    public Vector3 movement;
    public Vector3 lookoffset;
    
    public Transform grabHand;
    public Transform gunHand;
    public Vector3 hangOffset;
    public float grabHeight;
    
    private State currentState;

    //Public hidden
    [HideInInspector] public OrbitCamera oc;
    [HideInInspector] public Transform chest;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public PlayerCombat pc;
    [HideInInspector] public PlayerStats st;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        oc = FindObjectOfType<OrbitCamera>();
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        pc = GetComponent<PlayerCombat>();
        st = GetComponent<PlayerStats>();

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
                    if (jumped)
                    {
                        anim.SetTrigger("Rol");
                        jumped = false;
                    }
                    fallDuration = 0;
                }
                else
                {
                    jumped = false;
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
        if (StateMachine.IsInState("Locomotion") && grounded && !anim.GetBool("Sprinting"))
        {
            if (Input.GetMouseButton(1) && !pc.inCombo)
            {
                Aiming = true;
                if (lerping < 0.7f)
                {
                    lerping += 2 * Time.deltaTime;
                }
                anim.SetIKPosition(AvatarIKGoal.LeftHand, gunHand.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, gunHand.rotation);

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, Mathf.Lerp(0, 0.7f, lerping));
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, Mathf.Lerp(0, 0.7f, lerping));
            }
            else
            {
                Aiming = false;
                if (lerping > 0)
                {
                    lerping -= 2 * Time.deltaTime;
                }
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, lerping);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, lerping);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, gunHand.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, gunHand.rotation);
            }
        }
        else if (StateMachine.IsInState("Locomotion") && !grounded)
        {
            Aiming = false;
            if (Input.GetKey(KeyCode.E))
            {
                if (lerping < 0.7f)
                {
                    lerping += 2 * Time.deltaTime;
                }
                anim.SetIKPosition(AvatarIKGoal.RightHand, grabHand.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, grabHand.rotation);

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 0.7f, lerping));
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 0.7f, lerping));
            }
            else
            {
                if (lerping > 0)
                {
                    lerping -= 2 * Time.deltaTime;
                }
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, lerping);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, lerping);
                anim.SetIKPosition(AvatarIKGoal.RightHand, grabHand.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, grabHand.rotation);
            }
        }
        else
        {
            Aiming = false;
        }
    }

    public bool Aiming
    {
        get
        {
            return aiming;
        }
        set
        {
            aiming = value;
            gunInHand.SetActive(aiming);
            gunHoldster.SetActive(!aiming);
        }
    }

}