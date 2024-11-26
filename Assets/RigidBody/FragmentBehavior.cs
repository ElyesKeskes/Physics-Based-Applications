using UnityEngine;

public class FragmentBehavior : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;

    public void Initialize(Vector2 startPosition, Vector2 startVelocity)
    {
        position = startPosition;
        velocity = startVelocity;
    }

    void Update()
    {
        // Update position
        float deltaTime = Time.deltaTime;
        position += velocity * deltaTime;

        // Render fragment
        transform.position = new Vector3(position.x, position.y, 0);
    }
}
