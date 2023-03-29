using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{

    public GameObject clearMenuUI;

    public void Start() {
        clearMenuUI.SetActive(false);
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
