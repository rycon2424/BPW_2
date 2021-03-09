using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    public Transform viewTarget;
    public LayerMask collisionLayers;
    public float distance = 6.0f;
    public float distanceSpeed = 150.0f;
    public float collisionOffset = 0.3f;
    public float minDistance = 4.0f;
    public float maxDistance = 12.0f;
    public float height = 1.5f;
    public float horizontalRotationSpeed = 250.0f;
    public float verticalRotationSpeed = 150.0f;
    public float rotationDampening = 0.75f;
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;
    public bool useRMBToAim = false;
    public bool canChangeDistance = true;
    
    private float h, v, newDistance, smoothDistance;
    private Vector3 newPosition;
    private Quaternion newRotation, smoothRotation;
    private Transform cameraTransform;
    
    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            verticalRotationSpeed -= 10;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            verticalRotationSpeed += 10;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontalRotationSpeed -= 10;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontalRotationSpeed += 10;
        }
    }
    
    void Initialize()
    {
        h = this.transform.eulerAngles.x;
        v = this.transform.eulerAngles.y;

        cameraTransform = this.transform;
        smoothDistance = distance;

        NullErrorCheck();
    }
    
    void NullErrorCheck()
    {
        if (!viewTarget)
        {
            Debug.LogError("Please make sure to assign a view target!");
            Debug.Break();
        }
        if (collisionLayers == 0)
        {
            Debug.LogWarning("Make sure to set the collision layers to the layers the camera should collide with!");
        }
    }
    
    void LateUpdate()
    {
        if (!viewTarget)
            return;
        
        if (!useRMBToAim)
        {
            h += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
            v -= Input.GetAxis("Mouse Y") * verticalRotationSpeed * Time.deltaTime;

            h = ClampAngle(h, -360.0f, 360.0f);
            v = ClampAngle(v, minVerticalAngle, maxVerticalAngle);

            newRotation = Quaternion.Euler(v, h, 0.0f);
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                h += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
                v -= Input.GetAxis("Mouse Y") * verticalRotationSpeed * Time.deltaTime;

                h = ClampAngle(h, -360.0f, 360.0f);
                v = ClampAngle(v, minVerticalAngle, maxVerticalAngle);

                newRotation = Quaternion.Euler(v, h, 0.0f);
            }
        }
        
        if (canChangeDistance)
        {
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 10, minDistance, maxDistance);
        }
        smoothDistance = Mathf.Lerp(smoothDistance, distance, TimeSignature(distanceSpeed));
        
        smoothRotation = Quaternion.Slerp(smoothRotation, newRotation, TimeSignature((1 / rotationDampening) * 100.0f));

        newPosition = viewTarget.position;
        newPosition += smoothRotation * new Vector3(0.0f, height, -smoothDistance);
        
        CheckSphere();

        smoothRotation.eulerAngles = new Vector3(smoothRotation.eulerAngles.x, smoothRotation.eulerAngles.y, 0.0f);

        cameraTransform.position = newPosition;
        cameraTransform.rotation = smoothRotation;
    }
    
    void CheckSphere()
    {
        Vector3 tmpVect = viewTarget.position;
        tmpVect.y += height;

        RaycastHit hit;
        
        Vector3 dir = (newPosition - tmpVect).normalized;
        
        if (Physics.SphereCast(tmpVect, 0.3f, dir, out hit, distance, collisionLayers))
        {
            newPosition = hit.point + (hit.normal * collisionOffset);
        }
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
    
    private float TimeSignature(float speed)
    {
        return 1.0f / (1.0f + 80.0f * Mathf.Exp(-speed * 0.02f));
    }
}