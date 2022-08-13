using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline
{

    public float alpha = 0.5f;
    int pointCount;
    int segmentCount;
    List<Vector3> controlPoints;

    public CatmullRomCurve GetCurve(int i)
    {
        return new CatmullRomCurve(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], alpha);
    }

    public List<CatmullRomCurve> GetAllCurves()
    {
        List<CatmullRomCurve> result = new List<CatmullRomCurve>();
        for (int i = 0; i < segmentCount; i++)
        {
            result.Add(GetCurve(i));
        }
        return result;
    }

    public CatmullRomSpline(List<Vector3> controlPoints, float alpha = 1)
    {
        this.controlPoints = controlPoints;
        pointCount = controlPoints.Count;
        segmentCount = pointCount - 3;

        this.alpha = alpha;
    }
}

public struct CatmullRomCurve
{

    public Vector3 p0, p1, p2, p3;
    public float alpha;

    public CatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float alpha)
    {
        (this.p0, this.p1, this.p2, this.p3) = (p0, p1, p2, p3);
        this.alpha = alpha;
    }

    // Evaluates a point at the given t-value from 0 to 1
    public Vector3 GetPoint(float t)
    {
        // calculate knots
        const float k0 = 0;
        float k1 = GetKnotInterval(p0, p1);
        float k2 = GetKnotInterval(p1, p2) + k1;
        float k3 = GetKnotInterval(p2, p3) + k2;

        // evaluate the point
        float u = Mathf.LerpUnclamped(k1, k2, t);
        Vector3 A1 = Remap(k0, k1, p0, p1, u);
        Vector3 A2 = Remap(k1, k2, p1, p2, u);
        Vector3 A3 = Remap(k2, k3, p2, p3, u);
        Vector3 B1 = Remap(k0, k2, A1, A2, u);
        Vector3 B2 = Remap(k1, k3, A2, A3, u);
        return Remap(k1, k2, B1, B2, u);
    }

    static Vector3 Remap(float a, float b, Vector3 c, Vector3 d, float u)
    {
        return Vector3.LerpUnclamped(c, d, (u - a) / (b - a));
    }

    float GetKnotInterval(Vector3 a, Vector3 b)
    {
        return Mathf.Pow(Vector3.SqrMagnitude(a - b), 0.5f * alpha);
    }

}