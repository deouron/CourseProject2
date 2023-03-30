using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float FindDistanceToEllipse(Vector2 point) {
        double ellipse_a = 10, ellipse_b = 3;  // 11.3, 2
        double SQRT_TWO = 0.70710678118;
        // Ellipse: (x/a)^2 + (y/b)^2 = 1

        double px = System.Math.Abs(point.x);
        double py = System.Math.Abs(point.y);      

        double tx = SQRT_TWO;
        double ty = SQRT_TWO;

        double a = ellipse_a;
        double b = ellipse_b;

        double x, y, ex, ey, rx, ry, qx, qy, r, q, t = 0;

        for (int i = 0; i < 3; ++i)
        {
            x = a * SQRT_TWO;
            y = b * SQRT_TWO;

            ex = (a * a - b * b) * Mathf.Pow((float)tx, 3) / a;
            ey = (b * b - a * a) * Mathf.Pow((float)ty, 3) / b;

            rx = x - ex;
            ry = y - ey;

            qx = px - ex;
            qy = py - ey;

            r = System.Math.Sqrt(rx * rx + ry * ry);
            q = System.Math.Sqrt(qy * qy + qx * qx);

            tx = System.Math.Min(1, System.Math.Max(0, (qx * r / q + ex) / a));
            ty = System.Math.Min(1, System.Math.Max(0, (qy * r / q + ey) / b));

            t = System.Math.Sqrt(tx * tx + ty * ty);

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
            DrawTopCurve.shift.y -= SettingsMenu.convergenceSpeed;
            DrawDownCurve.shift.y += SettingsMenu.convergenceSpeed;
        } 
    }

    public static void OffsetCurves() {
        if (4 + DrawTopCurve.shift.y + DrawTopCurve.shift.y - DrawDownCurve.shift.y + SettingsMenu.recedingSpeed * 2
            <= SettingsMenu.MaxDistance) {     
            DrawTopCurve.shift.y += SettingsMenu.recedingSpeed;
            DrawDownCurve.shift.y -= SettingsMenu.recedingSpeed;
        } 
    }

    public static Vector2 GetWorldCoordinate(Vector2 mousePosition) {
        Vector2 mousePoint = new Vector2(mousePosition.x, mousePosition.y);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
