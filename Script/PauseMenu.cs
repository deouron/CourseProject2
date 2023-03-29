using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    public GameObject pauseMenuUI;
    public static int saveDataCnt = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPause) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        WriteLog.ClearArrays();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }

    public void LoadLog() {
        WriteLog.ClearArrays();
        Debug.Log("Load log");
        Time.timeScale = 1f;
        SceneManager.LoadScene(1); // name of scene
    }

    public void Settings() {
        WriteLog.ClearArrays();
        Debug.Log("Load settings");
        Time.timeScale = 1f;
        SceneManager.LoadScene(2); // name of scene
    }

    public void SaveData() {
        // from DrawLine.cs
        // List<float> logs, List<string> durations, List<string> times
        for (int i = 0; i < DrawLine.logs.Count; ++i) {
            PlayerPrefs.SetFloat("logs_" + i.ToString(), DrawLine.logs[i]);
        }
        for (int i = 0; i < DrawLine.durations.Count; ++i) {
            PlayerPrefs.SetString("durations_" + i.ToString(), DrawLine.durations[i]);
        }
        for (int i = 0; i < DrawLine.times.Count; ++i) {
            PlayerPrefs.SetString("times_" + i.ToString(), DrawLine.times[i]);
        }
        saveDataCnt = DrawLine.logs.Count;
        PlayerPrefs.SetInt("saveDataCnt", saveDataCnt);
        PlayerPrefs.Save();
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame() {
        WriteLog.ClearArrays();
        Application.Quit();
    }
}
