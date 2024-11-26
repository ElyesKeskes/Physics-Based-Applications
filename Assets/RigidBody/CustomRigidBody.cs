using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    public Transform groundPlane; // Assign your ground plane
    public List<Transform> cubes; // Add all cube transforms manually
    public float cubeMass = 1f;   // Mass of each cube
    public float gravity = -9.81f; // Gravity force
    public float cubeSize = 1f;   // Size of each cube for collision detection
    public float fractureThreshold = -10f; // Velocity threshold to trigger fracture
    private Vector3[] velocities;
    private Vector3[] previousPositions;
    private bool fractured = false;

    void Start()
    {
        velocities = new Vector3[cubes.Count];
        previousPositions = new Vector3[cubes.Count];

        // Initialize previous positions for verlet integration
        for (int i = 0; i < cubes.Count; i++)
        {
            previousPositions[i] = cubes[i].position;
        }
    }

    void Update()
    {
        if (fractured) return;

        ApplyGravity();
        ResolveCollisions();
        ResolveConstraints();
        DetectGroundCollision();
    }

    void ApplyGravity()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            Vector3 currentPosition = cubes[i].position;
            Vector3 displacement = currentPosition - previousPositions[i];
            velocities[i] = displacement + (Vector3.up * gravity * Time.deltaTime);
            previousPositions[i] = currentPosition;
            cubes[i].position += velocities[i] * Time.deltaTime;
        }
    }

    void ResolveCollisions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            for (int j = i + 1; j < cubes.Count; j++)
            {
                if (CheckOverlap(cubes[i].position, cubes[j].position))
                {
                    // Resolve overlap by moving cubes apart
                    Vector3 direction = cubes[j].position - cubes[i].position;
                    float overlapDistance = cubeSize - direction.magnitude;
                    Vector3 correction = direction.normalized * overlapDistance / 2f;

                    cubes[i].position -= correction;
                    cubes[j].position += correction;

                    // Apply simple velocity adjustment for collision response
                    velocities[i] = Vector3.zero;
                    velocities[j] = Vector3.zero;
                }
            }
        }
    }

    bool CheckOverlap(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position1, position2) < cubeSize;
    }

    void ResolveConstraints()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            for (int j = i + 1; j < cubes.Count; j++)
            {
                float maxDistance = 1.0f; // Example constraint length (adjust as needed)
                Vector3 direction = cubes[j].position - cubes[i].position;
                float distance = direction.magnitude;

                if (distance > maxDistance)
                {
                    Vector3 correction = direction.normalized * (distance - maxDistance) / 2f;
                    cubes[i].position += correction;
                    cubes[j].position -= correction;
                }
            }
        }
    }

    void DetectGroundCollision()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i].position.y <= groundPlane.position.y)
            {
                if (velocities[i].y <= fractureThreshold)
                {
                    Fracture();
                    return;
                }
                else
                {
                    // Simple collision response: bounce back with reduced velocity
                    velocities[i].y *= -0.5f;
                    cubes[i].position = new Vector3(
                        cubes[i].position.x,
                        groundPlane.position.y + cubeSize / 2f,
                        cubes[i].position.z);
                }
            }
        }
    }

    void Fracture()
    {
        fractured = true;
        Debug.Log("Fractured!");
        // Add optional VFX here for a better fracture effect
    }
}
