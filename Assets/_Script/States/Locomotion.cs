using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.characterController.enabled = true;
        pb.anim.applyRootMotion = true;
        pb.canJump = true;
        pb.fallDuration = 0;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.crossHairUI.SetActive(false);
        pb.Aiming = false;
    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        MovementAndJump(pb);
        if (moveHorizontal != 0 || moveVertical != 0 || pb.aiming)
        {
            RotateTowardsCamera(pb);
        }
        if (Input.GetKey(KeyCode.E) && pb.grounded == false)
        {
            FindLedge(pb);
        }
    }
    
    void FindLedge(PlayerBehaviour pb)
    {
        RaycastHit hit;
        Ray ray = new Ray(pb.transform.position + Vector3.up * 1.8f + pb.transform.forward, -pb.transform.up);
        Debug.DrawRay(pb.transform.position + Vector3.up * 1.8f + pb.transform.forward, pb.transform.up * -0.6f, Color.red, 1);
        if (Physics.Raycast(ray, out hit, 0.6f))
        {
            pb.grabHeight = hit.point.y - pb.transform.position.y;
            StateMachine.GoToState(pb, "Hanging");
            Debug.Log(hit.collider.name);
        }
    }

    float moveHorizontal;
    float moveVertical;
    void MovementAndJump(PlayerBehaviour pb)
    {
        float yMove = pb.movement.y;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        pb.anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        pb.anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        
        float hasInput = Input.GetAxis("Vertical") + Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && hasInput != 0 && pb.pc.inCombo == false)
        {
            pb.anim.SetBool("Sprinting", true);
        }
        else
        {
            pb.anim.SetBool("Sprinting", false);
        }
        pb.sprinting = pb.anim.GetBool("Sprinting");

        pb.movement = (pb.transform.forward * moveVertical) + (pb.transform.right * moveHorizontal);
        pb.movement = pb.movement.normalized;
        pb.movement.y = yMove;

        if (pb.characterController.isGrounded)
        {
            pb.movement.y = 0f;
        }

        pb.anim.SetBool("IsGrounded", pb.grounded);

        if (pb.characterController.isGrounded || pb.grounded)
        {
            pb.pc.CombatUpdate();
            if (Input.GetButtonDown("Jump") && pb.canJump)
            {
                pb.anim.SetTrigger("Jump");
                pb.canJump = false;
                pb.pc.InteruptAttack();
            }
        }

        // Apply gravity
        pb.movement.y = pb.movement.y + (Physics.gravity.y * pb.gravity * Time.deltaTime);

        if (pb.canMove)
            pb.characterController.Move(pb.movement * Time.deltaTime);

        return;
    }

    public void RotateTowardsCamera(PlayerBehaviour pb)
    {
        var CharacterRotation = pb.cameraObject.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        pb.transform.rotation = Quaternion.Slerp(pb.transform.rotation, CharacterRotation, Time.deltaTime * 8);
    }
}
