using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Helpers
{
    public static Vector3 ExcludeAxis(Vector3 vector, string axis)
    {
        if(axis == "x")
        {
            return new Vector3(0, vector.y, vector.z);
        }

        if (axis == "y")
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        if (axis == "z")
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        return Vector3.zero;
    }
}
