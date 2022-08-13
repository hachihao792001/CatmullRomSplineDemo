using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullDemo : MonoBehaviour
{
    public float alpha;
    void OnDrawGizmos()
    {
        List<Vector3> allChildPos = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
            allChildPos.Add(transform.GetChild(i).position);

        CatmullRomSpline spline = new CatmullRomSpline(allChildPos, alpha);
        List<CatmullRomCurve> curves = spline.GetAllCurves();
        for (int i = 0; i < curves.Count; i++)
            DrawCurveSegment(curves[i]);
    }

    void DrawCurveSegment(CatmullRomCurve curve)
    {
        const int detail = 32;
        Vector3 prev = curve.p1;
        for (int i = 1; i < detail; i++)
        {
            float t = i / (detail - 1f);
            Vector3 pt = curve.GetPoint(t);
            Gizmos.DrawLine(prev, pt);
            prev = pt;
        }
    }
}
