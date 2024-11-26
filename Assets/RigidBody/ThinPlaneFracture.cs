using UnityEngine;

public class FractureOnImpact : MonoBehaviour
{
    public Transform sphere; // Sphere representing the impact point
    public float gravity = -9.8f; // Gravity value
    public float forceMultiplier = 10.0f; // Force applied during fracture
    public float maxForce = 20.0f; // Maximum force to prevent instability
    public float sphereRadius = 0.5f; // Radius of the sphere
    public float angularDamping = 0.98f; // Damping to slow down rotation over time
    public float linearDamping = 0.98f; // Damping to slow down linear velocity
    private float cubeMomentOfInertia = 1.0f; // Simplified moment of inertia for a unit cube

    private Transform[] fragments; // Array to store fragments (cubes)
    private Vector3[] velocities; // Linear velocity of each fragment
    private Vector3[] angularVelocities; // Angular velocity of each fragment
    private Quaternion[] orientations; // Orientation of each fragment
    private bool[] fractured; // Whether each cube has fractured
    private Vector3 plateVelocity = Vector3.zero; // Velocity of the entire plate
    private float groundY = 0.0f; // Ground plane at y = 0

    void Start()
    {
        // Initialize fragments and their velocities
        int fragmentCount = transform.childCount;
        fragments = new Transform[fragmentCount];
        velocities = new Vector3[fragmentCount];
        angularVelocities = new Vector3[fragmentCount];
        orientations = new Quaternion[fragmentCount];
        fractured = new bool[fragmentCount];

        for (int i = 0; i < fragmentCount; i++)
        {
            fragments[i] = transform.GetChild(i);
            velocities[i] = Vector3.zero;
            angularVelocities[i] = Vector3.zero;
            orientations[i] = fragments[i].rotation; // Initialize with current rotation
            fractured[i] = false; // Initially not fractured
        }

        Debug.Log("Simulation Initialized. Plate will fall and fracture on impact.");
    }

    void Update()
    {
        // Simulate the plate falling under gravity
        plateVelocity += new Vector3(0, gravity, 0) * Time.deltaTime;
        transform.position += plateVelocity * Time.deltaTime;

        for (int i = 0; i < fragments.Length; i++)
        {
            if (!fractured[i])
            {
                // Check collision with sphere for each unfractured fragment
                if (IsCollidingWithSphere(fragments[i].position))
                {
                    Debug.Log($"Fragment {i} fractured!");
                    FractureCube(i);
                }
            }
            else
            {
                // Apply gravity and update fractured cube motion
                velocities[i] += new Vector3(0, gravity, 0) * Time.deltaTime;
                velocities[i] *= linearDamping; // Apply linear damping
                fragments[i].position += velocities[i] * Time.deltaTime;

                // Update rotation with damping
                angularVelocities[i] *= angularDamping;
                UpdateFragmentRotation(i);

                // Prevent going below ground
                if (fragments[i].position.y < groundY)
                {
                    fragments[i].position = new Vector3(fragments[i].position.x, groundY, fragments[i].position.z);
                    velocities[i] = Vector3.zero;
                    angularVelocities[i] *= 0.5f; // Reduce angular velocity on ground impact
                }
            }
        }

        // Prevent the plate itself from going below the ground
        if (transform.position.y < groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            plateVelocity = Vector3.zero; // Stop plate motion
        }
    }

    bool IsCollidingWithSphere(Vector3 position)
    {
        // Check if the fragment is within the sphere's radius
        float distanceToSphere = Vector3.Distance(position, sphere.position);
        return distanceToSphere <= sphereRadius;
    }

    void FractureCube(int index)
    {
        fractured[index] = true;

        // Calculate impact direction and force
        Vector3 impactDirection = fragments[index].position - sphere.position;
        float distance = impactDirection.magnitude;
        impactDirection.Normalize();

        // Apply force proportional to the distance, clamped to a maximum
        float force = Mathf.Min(forceMultiplier / (distance + 0.1f), maxForce);
        velocities[index] = impactDirection * force;

        // Add initial angular velocity based on impact torque
        Vector3 torque = Vector3.Cross(impactDirection, Vector3.up); // Simplified torque calculation
        angularVelocities[index] = (torque / cubeMomentOfInertia) * force;

        // Detach the cube to allow independent movement
        fragments[index].parent = null;
    }

    void UpdateFragmentRotation(int index)
    {
        // Compute angular displacement using angular velocity
        Quaternion deltaRotation = Quaternion.Euler(angularVelocities[index] * Mathf.Rad2Deg * Time.deltaTime);

        // Update the fragment's orientation
        orientations[index] = deltaRotation * orientations[index];

        // Apply the updated orientation to the fragment
        fragments[index].rotation = orientations[index];
    }
}
