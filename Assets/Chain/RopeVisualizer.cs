using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeVisualizer : MonoBehaviour
{
    public RopeMass[] Masses; // Array of rope masses
    private LineRenderer lineRenderer;
    public Material material;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = Masses.Length;
    }

    void Update()
    {
        for (int i = 0; i < Masses.Length; i++)
        {
            lineRenderer.SetPosition(i, Masses[i].transform.position);
            lineRenderer.material = material;
        }
    }

}
