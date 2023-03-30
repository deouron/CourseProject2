using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    public GameObject clearMenuUI;

    public Slider sliderMinDistance; 
    [SerializeField] TMP_Text sliderMinDistanceText;
    public static float MinDistance = 1f;

    public Slider sliderMaxDistance; 
    [SerializeField] TMP_Text sliderMaxDistanceText;
    public static float MaxDistance = 4f;

    public Slider sliderInitialDistance; 
    [SerializeField] TMP_Text sliderInitialDistanceText;
    public static float InitialDistance = 3f;

    public Slider sliderPenaltyTime; 
    [SerializeField] TMP_Text sliderPenaltyTimeText;
    public static int penaltyTime = 30;

    public Slider sliderConvergenceSpeed; 
    [SerializeField] TMP_Text sliderConvergenceSpeedText;
    public static float convergenceSpeed = 0.05f;

    public Slider sliderRecedingSpeed; 
    [SerializeField] TMP_Text sliderRecedingSpeedText;
    public static float recedingSpeed = 0.15f;

    public Slider sliderPenaltySteppingOut; 
    [SerializeField] TMP_Text sliderPenaltySteppingOutText;
    public static int penaltySteppingOut = 10;

    public void Start() {
        clearMenuUI.SetActive(false);

        sliderMinDistance.value = MinDistance;
        sliderMaxDistance.value = MaxDistance;
        sliderInitialDistance.value = InitialDistance;
        sliderPenaltyTime.value = penaltyTime;
        sliderConvergenceSpeed.value = convergenceSpeed;
        sliderRecedingSpeed.value = recedingSpeed;
        sliderPenaltySteppingOut.value = penaltySteppingOut;
    }

    public void Update() {
        MinDistance = (float)System.Math.Round(sliderMinDistance.value, 2);
        sliderMinDistanceText.text = MinDistance.ToString();

        MaxDistance = (float)System.Math.Round(sliderMaxDistance.value, 2);
        sliderMaxDistanceText.text = MaxDistance.ToString();

        InitialDistance = (float)System.Math.Round(sliderInitialDistance.value, 2);
        sliderInitialDistanceText.text = InitialDistance.ToString();

        penaltyTime = (int)sliderPenaltyTime.value;
        sliderPenaltyTimeText.text = penaltyTime.ToString();

        convergenceSpeed = (float)System.Math.Round(sliderConvergenceSpeed.value, 2);
        sliderConvergenceSpeedText.text = convergenceSpeed.ToString();

        recedingSpeed = (float)System.Math.Round(sliderRecedingSpeed.value, 2);
        sliderRecedingSpeedText.text = recedingSpeed.ToString();

        penaltySteppingOut = (int)sliderPenaltySteppingOut.value;
        sliderPenaltySteppingOutText.text = penaltySteppingOut.ToString();
    }

    public void OpenConfirm() {
        clearMenuUI.SetActive(true);
    }

    public void NotConfirm() {
        clearMenuUI.SetActive(false);
    }

    public void ClearData() {
        DrawLine.logs.Clear();
        DrawLine.durations.Clear();
        DrawLine.times.Clear();
        PauseMenu.saveDataCnt = 0;
        PlayerPrefs.SetInt("saveDataCnt", PauseMenu.saveDataCnt);
        PlayerPrefs.Save();
        clearMenuUI.SetActive(false);
    }
}
