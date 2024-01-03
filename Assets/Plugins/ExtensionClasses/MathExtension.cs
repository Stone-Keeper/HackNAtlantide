using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    public static float TAU = 6.2831855f;

    public static Vector3 RotateVectorOnXZPlane(Vector3 vectorToRotate, float angleInRad)
    {
        return new Vector3(vectorToRotate.x * Mathf.Cos(angleInRad) - vectorToRotate.z * Mathf.Sin(angleInRad), 0f, vectorToRotate.z * Mathf.Cos(angleInRad) + vectorToRotate.x * Mathf.Sin(angleInRad)).normalized * vectorToRotate.magnitude;
    }

    public static Vector3 RotateVectorOnXYPlane(Vector3 vectorToRotate, float angle)
    {
        return new Vector3(vectorToRotate.x * Mathf.Cos(angle) - vectorToRotate.y * Mathf.Sin(angle), vectorToRotate.y * Mathf.Cos(angle) + vectorToRotate.x * Mathf.Sin(angle),0f).normalized * vectorToRotate.magnitude;
    }

    public static float AngleBetweenVectorsInDeg(Vector3 vector1, Vector3 vector2)
    {
        //The angle between two vectors a and b is found with the formula angle = cos-1 [ (a. b) / (|a| |b|) ]
        float angle = Mathf.Acos(Vector3.Dot(vector1,vector2)/(vector1.magnitude*vector2.magnitude));
        return angle*Mathf.Rad2Deg;
    }

    public static float AngleBetweenVectors(Vector3 vector1, Vector3 vector2)
    {
        //The angle between two vectors a and b is found with the formula angle = cos-1 [ (a. b) / (|a| |b|) ]
        float angle = Mathf.Acos(Vector3.Dot(vector1, vector2) / (vector1.magnitude * vector2.magnitude));
        return angle;
    }

    public static Vector3 FlatVectorXZ(Vector3 vector)
    {
        return new Vector3(vector.x, 0,vector.z);
    }

    public static Vector3 FlatVectorXY(Vector3 vector)
    {
        return new Vector3(vector.x,vector.y, 0f);
    }

    public static Vector3 GetPositionAtDistanceOfPoint(Vector3 point, Vector3 destinationPoint, float distance) 
    {
        Vector3 direction = (destinationPoint - point).normalized;
        return destinationPoint - direction * distance;
    }

    public static float HyperbolicTangent(float x)
    {
        return (Mathf.Exp(x)-Mathf.Exp(-x))/(Mathf.Exp(x) + Mathf.Exp(-x));
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(d, e, t);
    }

    public static Quaternion GetBezierOrientation(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 tangent = (e - d).normalized;


        return Quaternion.LookRotation(tangent);
    }

    /// <summary>
    /// Tangent of the bezier curve
    /// </summary>
    /// <param name="t">interpolatio value between 0 and 1</param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <returns></returns>
    public static Vector3 GetBezierOrientationVector(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 tangent = (e - d).normalized;


        return tangent;
    }
}
