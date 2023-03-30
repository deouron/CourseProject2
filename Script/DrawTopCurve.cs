using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTopCurve : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public static Vector3 p0 = new Vector3(-10f, 3f, 0f);
    private static Vector3 p1 = new Vector3(0f, 7f, 0f);
    private static Vector3 p2 = new Vector3(10f, 3f, 0f);

    public static Vector3 shift;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material.color = Color.red;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        shift = new Vector3(0f, (SettingsMenu.InitialDistance - 4) / 2, 0f);
    }

    void Update()
    {
        if (!PauseMenu.isPause) {
            DrawQuadraticBezierCurve(p0, p1, p2);
        } else {
            lineRenderer.positionCount = 0;
        }
    }

    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        point0 += shift;
        point1 += shift;
        point2 += shift;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2; // Bezier curve
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
}
