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

    public GameObject leftPoint;
    public GameObject rightPoint;

    public float time;
    public float secDuration;
    [SerializeField] TMP_Text timeText;
    private float timeStart;

    private float timeMistake;
    private float timeStartMistake;
    public float milisecDurationMistake;

    public static List<float> logs = new List<float>();
    public static List<string> durations = new List<string>(); 
    public static List<string> times = new List<string>();

    private bool isNeedShift = false;
    private float distanceBetweenCurves;
    private float distanceToEllipse;

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

    void resetPoints() {
        leftPoint.transform.localScale = Utils.initialRaiusScale;
        rightPoint.transform.localScale = Utils.initialRaiusScale;
    }

    void Start() {
        // lineDraw.material.color = Color.black;
        lineDraw.startWidth = 0.03f;
        lineDraw.endWidth = 0.03f;
        lineDraw.positionCount = 0;

        score = 0;
        secDuration = 0;
        timeStart = time;

        timeMistake = time / 5;
        timeStartMistake = timeMistake;
        milisecDurationMistake = 0;

        distanceBetweenCurves = (float)Vector3.Distance(DrawTopCurve.p0 + DrawTopCurve.shift,
                                                        DrawDownCurve.p0 + DrawDownCurve.shift);
        distanceToEllipse = 0;

        if (logs.Count == 0) {
            LoadData();
        }

        resetPoints();
    }

    void increaseRaduises() {
        if (leftPoint.transform.localScale.x < 200) {
            leftPoint.transform.localScale += Utils.pointsIncreaseRadiusShift;
            rightPoint.transform.localScale += Utils.pointsIncreaseRadiusShift;
        }
        // Debug.Log("leftPoint: " + leftPoint.transform.localScale);
    }

    void decreaseRaduises() {
        if (leftPoint.transform.localScale.x > 50) {
            leftPoint.transform.localScale -= Utils.pointsDecreaseRadiusShift;
            rightPoint.transform.localScale -= Utils.pointsDecreaseRadiusShift;
        }
    }

    void ShiftCurves() {
        if (milisecDurationMistake > SettingsMenu.penaltyTime / (10 / Utils.milisecForMove)) {
            Utils.OffsetCurves();
            milisecDurationMistake -= SettingsMenu.penaltyTime / (10 / Utils.milisecForMove);
            score += SettingsMenu.penaltySteppingOut;
            increaseRaduises();
        } else {
            Utils.SmoothCurves();
            decreaseRaduises();
        }
        
    }

    void ChangeTime() {
        if (isNeedShift) {
            ShiftCurves();
            isNeedShift = false;
        }
        if (secDuration % 10 == 0) {
            timeText.text = (secDuration / 10).ToString() + ".0s";
        } else {
            timeText.text = (secDuration / 10).ToString() + "s";
        }
        time -= Time.deltaTime;
        if (time <= 0) {
            time = timeStart;
            secDuration += 1;
            if (secDuration % Utils.milisecForMove == 0) {
                isNeedShift = true;
            }
        }
    }

    void ChangeTimeMistake() {
        timeMistake -= Time.deltaTime;
        if (timeMistake <= 0) {
            timeMistake = timeStartMistake;
            milisecDurationMistake += 1;
        }
    }

    void Reset() {
        lineDraw.positionCount = 0;
        if (score > 0) {
            logs.Add((float)System.Math.Round(score, 2));

            if (secDuration % 10 == 0) {
                durations.Add((secDuration / 10).ToString() + ".0s");
            } else {
                durations.Add((secDuration / 10).ToString() + "s");
            }

            times.Add(DateTime.Now.ToString());
        }

        Debug.Log("Mistake time: " + milisecDurationMistake.ToString());
        Debug.Log("All time: " + secDuration.ToString());

        score = 0;
        secDuration = 0;
        milisecDurationMistake = 0;
        DrawTopCurve.shift.y = (SettingsMenu.InitialDistance - 4) / 2;
        DrawDownCurve.shift.y = -(SettingsMenu.InitialDistance - 4) / 2;

        resetPoints();
    }

    void Update() {
        distanceBetweenCurves = (float)Vector3.Distance(DrawTopCurve.p0 + DrawTopCurve.shift,
                                                        DrawDownCurve.p0 + DrawDownCurve.shift);
        if (Input.GetMouseButton(0) && !PauseMenu.isPause) {
            Vector2 currentPoint = Utils.GetWorldCoordinate(Input.mousePosition);
            if (!(currentPoint.x == 0 && currentPoint.y == 0)) {
                // Debug.Log(currentPoint);
                ++lineDraw.positionCount;
                lineDraw.SetPosition(lineDraw.positionCount - 1, currentPoint);

                distanceToEllipse = Utils.FindDistanceToEllipse(currentPoint);
                score += distanceToEllipse / 100;
                scoreText.text = System.Math.Round(score, 2).ToString();

                 Debug.Log("distanceToEllipse: " + distanceToEllipse.ToString() + 
                    "\ndistanceBetweenCurves: " + (distanceBetweenCurves / 2).ToString());

                if (distanceToEllipse > (distanceBetweenCurves / 2)) {
                    ChangeTimeMistake();
                }

                ChangeTime();
            }
        } else if (Input.GetMouseButtonUp(0)) {
            Reset();
        }
    }
}
