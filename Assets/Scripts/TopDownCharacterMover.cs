using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    private Rigidbody rb;
    private GroundCheck _ground;
    [Header("Rotate")]
    [SerializeField]
    private bool RotateTowardMouse;
    [Header("Movement")]
    [SerializeField]
    private float MovementSpeed;
    [Header("Jump")]
    [SerializeField]
    private float JumpSpeed;
    [SerializeField]
    private float FallSpeed;
    [SerializeField]
    private float MaxFallSpeed;

    [SerializeField]
    //I set to zero because the rotation will probably be made with the animation
    //But this is depending on the assets that we will use
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        _ground = GetComponentInChildren<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        Vector3 targetVector = new Vector3(_input.MovementInputVector.x, 0, _input.MovementInputVector.y);
        Vector3 movementVector = MoveTowardTarget(targetVector);
        //Jump
        if (_ground.isOnGround)
        {
            if (_input.JumpButton)
            {
                rb.velocity = new Vector3(rb.velocity.x, JumpSpeed, rb.velocity.z);
            }
        }
        else
        {
            if (rb.velocity.y > -MaxFallSpeed)
            {
                rb.velocity -= new Vector3(0, FallSpeed, 0);
            }
        }
        //Rotation
        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }

    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            Vector3 target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        float speed = MovementSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        Vector3 targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        Quaternion rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}