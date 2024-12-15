//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

namespace FormulsLibrary
{
    public static class Helper
    {
        // Min 0, Max 10.0f
        public static Vector3 GetPointBetweenTwoPoints(Vector3 point1, Vector3 point2, float distance)
        {
            return Vector3.Lerp(point1, point2, distance / Vector3.Distance(point1, point2));
        }

        // speed = distance / time
        // distance = speed * time
        // time = distance / speed

        public static float AngleBetweenTwoVectors2D(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Atan2(point2.y - point1.y, point2.x - point1.x);
        }

        public static float AngleInRadiansBetweenTwoVectors(Vector3 v1, Vector3 v2)
        {
            float cosAngle = Vector3.Dot(v1, v2) / (v1.magnitude * v2.magnitude);
            return Mathf.Acos(cosAngle);
        }
    }
}


