using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    float survivalTime;

    void Start()
    {
        survivalTime = GameManager.Instance.GetDeltaTimeInMain;
        timerText.text = ((int)survivalTime / 60).ToString("00") + ":" + ((int)survivalTime % 60).ToString("00");
    }
}
