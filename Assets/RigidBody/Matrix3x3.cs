using UnityEngine;

public class Matrix3x3
{
    private float[,] elements;

    public static Matrix3x3 Identity
    {
        get
        {
            return new Matrix3x3(new float[3, 3] {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            });
        }
    }

    public Matrix3x3(float[,] elements)
    {
        this.elements = elements;
    }

    public Vector3 InverseMultiply(Vector3 vector)
    {
        // Simplified for diagonal inertia tensor
        return new Vector3(
            vector.x / elements[0, 0],
            vector.y / elements[1, 1],
            vector.z / elements[2, 2]
        );
    }

    public static Matrix3x3 operator *(Matrix3x3 matrix, float scalar)
    {
        float[,] result = new float[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                result[i, j] = matrix.elements[i, j] * scalar;
            }
        }
        return new Matrix3x3(result);
    }
}
