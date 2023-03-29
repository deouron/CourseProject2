using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineDraw;
    public float score;
    [SerializeField] TMP_Text scoreText;

    private Transform curveStart;
    private Transform curveMid;
    private Transform curveFinish;

    private double ellipse_a, ellipse_b;
    private double SQRT_TWO = 0.70710678118;

    public float time;
    public float sec;
    [SerializeField] TMP_Text timeText;
    private float timeStart;

    public static List<float> logs = new List<float>();
    public static List<string> durations = new List<string>(); 
    public static List<string> times = new List<string>();

    void LoadData() {
        if (PlayerPrefs.HasKey("saveDataCnt")) {
            PauseMenu.saveDataCnt = PlayerPrefs.GetInt("saveDataCnt");
            for (int i = 0; i < PauseMenu.saveDataCnt; ++i) {
                logs.Add(PlayerPrefs.GetFloat("logs_" + i.ToString()));
                durations.Add(PlayerPrefs.GetString("durations_" + i.ToString()));
                times.Add(PlayerPrefs.GetString("times_" + i.ToString()));
            }
        } 
    }

    void Start()
    {
        lineDraw.material.color = Color.black;
        lineDraw.startWidth = 0.03f;
        lineDraw.endWidth = 0.03f;
        lineDraw.positionCount = 0;

        score = 0;
        sec = 0;
        timeStart = time;

        ellipse_a = 11.3; // 10.6 – TopCurve
        ellipse_b = 2; // 4 – TopCurve

        if (logs.Count == 0) {
            LoadData();
        }
    }

    float FindEllipseClosestPoint(Vector2 point) {
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
            if (System.Math.Abs(point.y) > height) { // the point lies under the ellipse
                return (float)(2 * height + Vector2.Distance(dot, point));
            }
            return (float)(height + System.Math.Abs(point.y));
        }

        return (float)Vector2.Distance(dot, point);
    }

    void ChangeTime() {
        if (sec % 10 == 0) {
            timeText.text = (sec / 10).ToString() + ".0s";
        } else {
            timeText.text = (sec / 10).ToString() + "s";
        }
        time -= Time.deltaTime;
        if (time <= 0) {
            time = timeStart;
            sec += 1;
        }
    }

    void Reset() {
        lineDraw.positionCount = 0;
        if (score > 0) {
            logs.Add((float)System.Math.Round(score, 2));

            if (sec % 10 == 0) {
                durations.Add((sec / 10).ToString() + ".0s");
            } else {
                durations.Add((sec / 10).ToString() + "s");
            }

            times.Add(DateTime.Now.ToString());
        }

        score = 0;
        sec = 0;
    }

    void Update() {
        if (Input.GetMouseButton(0) && !PauseMenu.isPause) {
            Vector2 currentPoint = GetWorldCoordinate(Input.mousePosition);
            if (!(currentPoint.x == 0 && currentPoint.y == 0)) {
                Debug.Log(currentPoint);
                ++lineDraw.positionCount;
                lineDraw.SetPosition(lineDraw.positionCount - 1, currentPoint);

                score += FindEllipseClosestPoint(currentPoint) / 100;
                scoreText.text = System.Math.Round(score, 2).ToString();

                ChangeTime();
            }
        } else if (Input.GetMouseButtonUp(0)) {
            Reset();
        }
    }

    private Vector2 GetWorldCoordinate(Vector2 mousePosition)
    {
        Vector2 mousePoint = new Vector2(mousePosition.x, mousePosition.y);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
