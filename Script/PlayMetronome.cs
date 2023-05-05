using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMetronome : MonoBehaviour
{
    public static bool isMetronomeClick = false;

    public void MetronomeClick() {
        Debug.Log("Metronome Click");
        isMetronomeClick = !isMetronomeClick;
        isMetronomeClick = !isMetronomeClick;
    }
}
