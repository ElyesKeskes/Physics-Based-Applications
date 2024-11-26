using UnityEngine;

public class RopeMass : MonoBehaviour
{
    public float MassValue = 1f;         // Mass of the segment
    public Vector3 Position;            // Current position
    public Vector3 Velocity;            // Current velocity
    private Vector3 netForce = Vector3.zero; // Net force applied to this mass
    public bool IsFixed = false;        // Whether this node is fixed in place
    public float ColliderRadius = 0.1f; // Radius of the custom collider

    void Start()
    {
        Position = transform.position; // Initialize position
    }

    public void ApplyForce(Vector3 force)
    {
        if (!IsFixed) netForce += force;
    }

    public void UpdateState(float deltaTime)
    {
        if (IsFixed)
        {
            Position = transform.position; // Keep the position synced for fixed nodes
            return;
        }

        Vector3 acceleration = netForce / MassValue; // F = ma
        Velocity += acceleration * deltaTime;
        Position += Velocity * deltaTime;
        netForce = Vector3.zero; // Reset net force after update
    }

    public void SyncTransform()
    {
        transform.position = Position;
    }

    public void CheckCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Position, ColliderRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.transform == this.transform) continue;

            Vector3 direction = Position - hit.ClosestPoint(Position);
            float overlap = ColliderRadius - direction.magnitude;

            if (overlap > 0)
            {
                direction = direction.normalized * overlap;
                Position += direction;
                Velocity *= 0.8f; // Dampen velocity on collision
            }
        }
    }

    // Draw custom collider in Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color for the collider
        Gizmos.DrawWireSphere(transform.position, ColliderRadius); // Draw a sphere representing the collider

    }
}
