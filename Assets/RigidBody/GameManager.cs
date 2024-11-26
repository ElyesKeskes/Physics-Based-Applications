using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SphereBehavior projectile;
    public CubeBehavior[] cubes;
    public float launchSpeed = 10f;

    void Start()
    {
        ResetSimulation();
    }

    public void ResetSimulation()
    {
        // Initialize projectile position
        projectile.Initialize(new Vector3(0, 0.5f, 0), Vector3.right * launchSpeed);

        // Initialize cube positions
        //foreach (var cube in cubes)
        //{
        //    cube.Initialize(cube.transform.position);
        //}
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetSimulation();
        }

        // Update simulation
        projectile.Simulate(Time.deltaTime);
        foreach (var cube in cubes)
        {
            cube.Simulate(Time.deltaTime, projectile);
        }
    }
}
