using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 angularVelocity; // Angular velocity (radians per second)
    public Quaternion orientation; // Orientation of the cube

    public float size = 1f;  // Cube size (assume cubic shape)
    public float mass = 1f; // Cube mass
    private Matrix3x3 inertiaTensor; // Moment of inertia tensor

    public void Initialize(Vector3 startPosition)
    {
        position = this.transform.position;
        velocity = Vector3.zero;
        angularVelocity = Vector3.zero;
        orientation = orientation;

        // Calculate the moment of inertia tensor for a cube
        float side = size;
        float inertia = (1f / 6f) * mass * side * side; // Moment of inertia for a cube
        inertiaTensor = Matrix3x3.Identity * inertia;
    }

    public void Simulate(float deltaTime, SphereBehavior projectile)
    {
        // Check for collision with the sphere
        if (projectile.CheckCollision(position, size))
        {
            Vector3 impactPoint = projectile.position;
            Vector3 impactForce = projectile.velocity * mass;

            ApplyForce(impactForce, impactPoint, projectile.radius < size);
            projectile.velocity = Vector3.zero; // Stop the sphere on collision
        }

        // Update linear motion
        position += velocity * deltaTime;
        velocity *= 0.98f; // Apply damping

        // Update angular motion
        Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Mathf.Rad2Deg * deltaTime);
        orientation = deltaRotation * orientation;

        // Render the cube
        RenderCube();
    }

    private void ApplyForce(Vector3 force, Vector3 pointOfImpact, bool addUpwardForce)
    {
        // Calculate the lever arm
        Vector3 leverArm = pointOfImpact - position;

        // Apply linear force
        velocity += force / mass;

        // Apply upward force if the sphere is smaller
        if (addUpwardForce)
        {
            velocity += Vector3.up * force.magnitude * 0.1f;
        }

        // Apply torque
        Vector3 torque = Vector3.Cross(leverArm, force);
        Vector3 angularAcceleration = inertiaTensor.InverseMultiply(torque);
        angularVelocity += angularAcceleration;
    }

    private void RenderCube()
    {
        // Manually render position and orientation
        transform.position = position;

        // Convert orientation to a rotation matrix for display
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(orientation);
        transform.rotation = Quaternion.LookRotation(rotationMatrix.GetColumn(2), rotationMatrix.GetColumn(1));
    }
}
