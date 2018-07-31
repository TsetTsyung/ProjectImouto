using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {


    public static Vector3 GetSurfacePoint(Vector3 startPos)
    {
        startPos.y = 1000f;
        RaycastHit hitInfo;
        if (Physics.Raycast(startPos, -Vector3.up, out hitInfo, 5000f))
        {
            return hitInfo.point;
        }

        else return Vector3.zero;
    }

}
