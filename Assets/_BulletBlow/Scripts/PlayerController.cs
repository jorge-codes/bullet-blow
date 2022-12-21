using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnActionReload;
    public event Action OnActionTriggerBegin;
    public event Action OnActionTriggerEnd;
    public LayerMask hitlayer;
    
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private Transform head = null;
    
    [Space(30)]
    [SerializeField, Range(.1f, 20f)] private float characterSpeed = 3f;
    [SerializeField, Range(10f, 300f)] private float mouseSensitivity = 60f;
    [SerializeField, Range(10f, 90f)] private float maxVerticalAngle = 90f;
    [SerializeField, Range(1f, 30f)] private float jumpSpeed = 10f;
    [SerializeField, Range(1f, 10f)] private float mass = 2f;
    [SerializeField] private float groundControl = -0.5f;
    
    private Vector3 direction;
    private Quaternion baseRotation;
    private float vertSpeed;
    private int bulletsNow;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        baseRotation = head.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // ComputeSimpleMovement();
        ComputeMovementWithJump();
        
        ComputeRotation();
        
        ComputeActions();
    }

    private void ComputeSimpleMovement()
    {
        // calculate translation
        direction = GetMovementVector();
        // apply and SimpleMove applies gravity directly
        controller.SimpleMove(direction * characterSpeed);
    }

    private void ComputeMovementWithJump()
    {
        // calculate translation
        direction = GetMovementVector();
        var velocity = direction * characterSpeed;
        
        // calculate gravity
        vertSpeed += Physics.gravity.y * mass * Time.deltaTime;
        if (controller.isGrounded)
        {
            vertSpeed = groundControl;
        }


        // validate jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }

        velocity.y = vertSpeed;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ComputeRotation()
    {
        // calculate rotation
        (var hotizontal, var vertical) = GetMouseMovement(mouseSensitivity);
        // horizontal
        transform.Rotate(Vector3.up, hotizontal * Time.deltaTime, Space.Self);
        
        // vertical
        var rotation = Quaternion.AngleAxis(vertical * Time.deltaTime, -Vector3.right);
        var delta = head.localRotation * rotation;
        if (Quaternion.Angle(baseRotation, delta) < maxVerticalAngle)
        {
            head.localRotation = delta;
        }
    }

    private void ComputeActions()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnActionTriggerBegin?.Invoke();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            OnActionTriggerEnd?.Invoke();
        }

        
    }
    

    // private void ShootRay()
    // {
    //     var ray = new Ray(head.transform.position, head.forward);
    //     var hits = Physics.RaycastAll(ray, 2f);
    //     if (hits == null || hits.Length == 0) return;
    //     
    //     var hit = FindCloser(hits);
    // }

    private RaycastHit FindCloser(RaycastHit[] hits)
    {
        var min = Mathf.Infinity;
        var hit = hits[0];
        var p = transform.position;
        
        foreach (var h in  hits)
        {
            var distance = (h.transform.position - p).magnitude;
            if (distance < min)
            {
                min = distance;
                hit = h;
            }
        }

        return hit;
    }


    private Vector3 GetMovementVector()
    {
        var fore = Input.GetAxis("Vertical") * transform.forward;
        var side = Input.GetAxis("Horizontal") * transform.right;
        return (fore + side).normalized;
    }
    

    private (float, float) GetMouseMovement(float sensitivity = 1f)
    {
        var hori = Input.GetAxis("Mouse X") * sensitivity;
        var vert = Input.GetAxis("Mouse Y") * sensitivity;
        return (hori, vert);
    }
    
}
