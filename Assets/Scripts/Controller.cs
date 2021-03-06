﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Controller : MonoBehaviour
{
    [SerializeField]
    Transform playerInputSpace = default;

    [SerializeField, Range(0.0f, 100.0f)]
    float maxSpeed = 10.0f, maxAcceleration = 10.0f, maxAirAcceleration = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    float jumpHeight = 2.0f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    [SerializeField, Range(0.0f, 90.0f)]
    float maxGroundAngle = 40.0f;

    [SerializeField] private List<GameObject> orbitObjectsList = new List<GameObject>();
    private LinkedList<GameObject> orbitObjects;

    Rigidbody body;

    Vector3 velocity, desiredVelocity, contactNormal;

    bool desiredJump;

    int jumpPhase, groundContactCount;

    bool OnGround => groundContactCount > 0;

    float minGroundDotProduct;

    string jump = "Jump", horizontal = "Horizontal", vertical = "Vertical";

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);    
    }

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void Start()
    {
        orbitObjects = new LinkedList<GameObject>(orbitObjectsList);
    }

    void Update()
    {
        Vector2 playerInput = new Vector2(Input.GetAxis(horizontal), Input.GetAxis(vertical));
        playerInput = Vector2.ClampMagnitude(playerInput, 1.0f);
        if(playerInputSpace)
        {
            Vector3 forward = playerInputSpace.forward;
            forward.y = 0.0f;
            forward.Normalize();
            Vector3 right = playerInputSpace.right;
            right.y = 0.0f;
            right.Normalize();
            desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed;
        }
        else
        {
            desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed;
        }
        desiredJump |= Input.GetButtonDown(jump);
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }
        body.velocity = velocity;
        ClearState();
    }

    void UpdateState()
    {
        velocity = body.velocity;
        if(OnGround)
        {
            jumpPhase = 0;
            if(groundContactCount > 1)
            {
                contactNormal.Normalize();
            }        
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    void Jump()
    {
        if (OnGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if(alignedSpeed > 0.0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0.0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if (orbitObjects.Count > 0)
        {
            orbitObjects.First.Value.SetActive(true);
            orbitObjects.RemoveFirst();
        }
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if(normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.fixedDeltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }
}
