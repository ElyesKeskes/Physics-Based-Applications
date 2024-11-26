using UnityEngine;

public class RopeConstraint : MonoBehaviour
{
    public RopeMass MassA; // First mass
    public RopeMass MassB; // Second mass
    public float MaxLength = 1f; // Maximum length between the masses

    public void EnforceConstraint()
    {
        Vector3 delta = MassB.Position - MassA.Position;
        float distance = delta.magnitude;

        if (distance > MaxLength)
        {
            Vector3 correction = delta.normalized * (distance - MaxLength) / 2f;

            if (!MassA.IsFixed)
                MassA.Position += correction;

            if (!MassB.IsFixed)
                MassB.Position -= correction;
        }
    }
}

