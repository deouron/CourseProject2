using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WriteLog : MonoBehaviour
{
    [SerializeField] TMP_Text FirstLogText;
    [SerializeField] TMP_Text SecondLogText;
    [SerializeField] TMP_Text NumberText;

    public GameObject nextButton;
    public GameObject previousButton;

    private string output;
    private int pageNumber;

    private const int COLUMN_ROWS = 5;
    private const int PAGE_ROWS = COLUMN_ROWS * 2;

    public static void ClearArrays() {
        DrawLine.durations.RemoveAt(DrawLine.durations.Count - 1);
        DrawLine.logs.RemoveAt(DrawLine.logs.Count - 1);
        DrawLine.times.RemoveAt(DrawLine.times.Count - 1);
    }

    void SetNextButtonActivity() {
        if (DrawLine.logs.Count <= 2 * COLUMN_ROWS * pageNumber) {
            nextButton.SetActive(false);
        } else {
            nextButton.SetActive(true);
        }
    }

    void SetPreviousButtonActivity() {
        if (pageNumber > 1) {
            previousButton.SetActive(true);
        } else {
            previousButton.SetActive(false);
        }
    }

    void WritePage() {
        output = "";
        for (int i = DrawLine.logs.Count - 1 - PAGE_ROWS * (pageNumber - 1); 
            i >= System.Math.Max(0, DrawLine.logs.Count - COLUMN_ROWS - PAGE_ROWS * (pageNumber - 1)); --i) {
            output += DrawLine.times[i] + "\nduration: " + DrawLine.durations[i] + 
                    ", score: " + DrawLine.logs[i].ToString() + "\n\n";
        }
        FirstLogText.text = output;
        if (DrawLine.logs.Count > COLUMN_ROWS) {
            output = "";
            for (int i = DrawLine.logs.Count - 1 - PAGE_ROWS * (pageNumber - 1) - COLUMN_ROWS;
            i >= System.Math.Max(0, DrawLine.logs.Count - PAGE_ROWS - PAGE_ROWS * (pageNumber - 1)); --i) {
                output += DrawLine.times[i] + "\nduration: " + DrawLine.durations[i] + 
                        ", score: " + DrawLine.logs[i].ToString() + "\n\n";
            }
            SecondLogText.text = output;
        }
    }

    void Start() {
        pageNumber = 1;
        NumberText.text = pageNumber.ToString();
        previousButton.SetActive(false);
        SetNextButtonActivity();

        Debug.Log("Size: " + DrawLine.logs.Count.ToString());

        WritePage();
    }

    public void next() {
        pageNumber += 1;
        WritePage();
        SetNextButtonActivity();
        SetPreviousButtonActivity();
        NumberText.text = pageNumber.ToString();
    }

    public void previous() {
        pageNumber -= 1;
        WritePage();
        SetNextButtonActivity();
        SetPreviousButtonActivity();
        NumberText.text = pageNumber.ToString();
    }
}
