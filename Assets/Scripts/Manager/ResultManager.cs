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
        Debug.Log($"deltaTime:{survivalTime}");
        timerText.text = (survivalTime / 60).ToString("00") + ":" + (survivalTime % 60).ToString("00");
    }
}
