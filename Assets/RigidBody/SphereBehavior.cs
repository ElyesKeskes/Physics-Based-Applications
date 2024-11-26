using UnityEngine;

public class SphereBehavior : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;

    public float radius = 0.5f; // Sphere radius

    public void Initialize(Vector3 startPosition, Vector3 startVelocity)
    {
        position = startPosition;
        velocity = startVelocity;
    }

    public void Simulate(float deltaTime)
    {
        // Update position based on velocity
        position += velocity * deltaTime;

        // Render the sphere
        RenderSphere();
    }

    private void RenderSphere()
    {
        // Draw the sphere manually
        transform.position = position;
    }

    public bool CheckCollision(Vector3 otherPosition, float otherSize)
    {
        // Check if the sphere overlaps with another object
        float distance = Vector3.Distance(position, otherPosition);
        return distance < radius + otherSize / 2;
    }
}
