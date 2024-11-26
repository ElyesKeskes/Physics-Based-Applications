using UnityEngine;

public class RopePhysicsIntegrator : MonoBehaviour
{
    public RopeMass[] Masses;          // Array of masses in the rope
    public RopeConstraint[] Constraints; // Array of constraints between masses
    public Vector3 Gravity = new Vector3(0, -9.81f, 0); // Gravity vector
    public float Damping = 0.99f;      // Velocity damping to simulate friction
    public int ConstraintIterations = 10; // Number of iterations to enforce constraints

    public void StepSimulation(float deltaTime)
    {
        // Apply gravity to each mass
        foreach (var mass in Masses)
        {
            if (!mass.IsFixed)
                mass.ApplyForce(Gravity * mass.MassValue);
        }

        // Update mass positions
        foreach (var mass in Masses)
        {
            mass.UpdateState(deltaTime);
            mass.Velocity *= Damping; // Apply damping
        }

        // Enforce constraints multiple times for stability
        for (int i = 0; i < ConstraintIterations; i++)
        {
            foreach (var constraint in Constraints)
            {
                constraint.EnforceConstraint();
            }
        }

        // Check collisions for each mass
        foreach (var mass in Masses)
        {
            mass.CheckCollisions();
            mass.SyncTransform();
        }
    }

    void FixedUpdate()
    {
        StepSimulation(Time.fixedDeltaTime);
    }
}
