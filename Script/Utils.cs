using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static int milisecForMove = 2;

    public static Vector3 pointsIncreaseRadiusShift = new Vector3(3.0f, 3.0f, 3.0f);
    public static Vector3 pointsDecreaseRadiusShift = new Vector3(1.3f, 1.3f, 1.3f);
    public static Vector3 initialRaiusScale = new Vector3(162.0f, 162.0f, 108.0f);

    public static float FindDistanceToEllipse(Vector2 point) {
        int steps = 3;

        double ellipse_a = 10, ellipse_b = 3;  // 11.3, 2
        double SQRT_TWO = 0.70710678118;
        // Ellipse: (x/a)^2 + (y/b)^2 = 1

        double px = System.Math.Abs(point.x), py = System.Math.Abs(point.y);  
        double tx = SQRT_TWO, ty = SQRT_TWO, a = ellipse_a, b = ellipse_b;
        double x, y, delta_x, delta_y, rx, ry, qx, qy, r, q, t = 0;

        for (int i = 0; i < steps; ++i)
        {
            x = a * SQRT_TWO;
            y = b * SQRT_TWO;

            delta_x = (Mathf.Pow((float)a, 2) - Mathf.Pow((float)b, 2)) * Mathf.Pow((float)tx, 3) / a;
            delta_y = (Mathf.Pow((float)b, 2) - Mathf.Pow((float)a, 2)) * Mathf.Pow((float)ty, 3) / b;

            rx = x - delta_x;
            ry = y - delta_y;

            qx = px - delta_x;
            qy = py - delta_y;

            r = System.Math.Sqrt(Mathf.Pow((float)rx, 2) + Mathf.Pow((float)ry, 2));
            q = System.Math.Sqrt(Mathf.Pow((float)qx, 2) + Mathf.Pow((float)qy, 2));

            tx = System.Math.Min(1, System.Math.Max(0, (qx * r / q + delta_x) / a));
            ty = System.Math.Min(1, System.Math.Max(0, (qy * r / q + delta_y) / b));

            t = System.Math.Sqrt(Mathf.Pow((float)tx, 2) + Mathf.Pow((float)ty, 2));

            tx /= t;
            ty /= t;
        }

        Vector2 dot = new Vector2
        {
            x = (float)(a * (point.x < 0 ? -tx : tx)),
            y = (float)(b * (point.y < 0 ? -ty : ty))
        };

        if (point.y < 0) { // the point lies closer in the lower arc of the ellipse
            double height = b / a * (a - System.Math.Abs(point.x));
            return (float)(height + System.Math.Abs(point.y));
        }

        return (float)Vector2.Distance(dot, point);
    }

    public static void SmoothCurves() {
        if (4 + DrawTopCurve.shift.y - DrawDownCurve.shift.y - SettingsMenu.convergenceSpeed * 2
            >= SettingsMenu.MinDistance) {
            DrawTopCurve.shift.y -= SettingsMenu.convergenceSpeed / (10 / milisecForMove);
            DrawDownCurve.shift.y += SettingsMenu.convergenceSpeed / (10 / milisecForMove);
        } 
    }

    public static void OffsetCurves() {
        if (4 + DrawTopCurve.shift.y + DrawTopCurve.shift.y - DrawDownCurve.shift.y + SettingsMenu.recedingSpeed * 2
            <= SettingsMenu.MaxDistance) {     
            DrawTopCurve.shift.y += SettingsMenu.recedingSpeed / (10 / milisecForMove);
            DrawDownCurve.shift.y -= SettingsMenu.recedingSpeed / (10 / milisecForMove);
        } 
    }

    public static Vector2 GetWorldCoordinate(Vector2 mousePosition) {
        Vector2 mousePoint = new Vector2(mousePosition.x, mousePosition.y);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
