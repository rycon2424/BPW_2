using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform arrowRotator;
    public float arrowSpeed;
    public float rotationSpeed = 2;
    public Animator anim;
    public GameObject RopeAttached;
    [HideInInspector] public PlayerBehaviour pb;

    float vertical;
    float horizontal;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        arrowRotator.Rotate(0, 0, 200 * Time.deltaTime);

        if (enabledControl)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        transform.Rotate(-vertical * rotationSpeed, horizontal * rotationSpeed, 0);

        transform.Translate(-horizontal * Time.deltaTime, -vertical * Time.deltaTime, -arrowSpeed * Time.deltaTime);

    }

    private bool enabledControl = false;
    public void EnableControl()
    {
        enabledControl = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pole"))
        {
            return;
        }
        arrowSpeed = 0;
        rotationSpeed = 0;
        anim.Play("Break");
    }

    public void Succes(List<Vector3> hits)
    {
        GameObject go = Instantiate(RopeAttached, pb.transform.position, Quaternion.identity);
        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.positionCount = hits.Count + 1;
        lr.SetPosition(0, go.transform.position);

        for (int i = 0; i < hits.Count; i++)
        {
            lr.SetPosition(i + 1, hits[i]);
        }
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, hits[0]);

        DestroyThis();
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StateMachine.GoToState(pb, "Locomotion");
    }

}
