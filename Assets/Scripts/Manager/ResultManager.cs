using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    //[SerializeField]
    //private GameManager gameManager;

    float survivalTime;

    void Start()
    {
        survivalTime = GameManager.Instance.GetDeltaTimeInMain;
        timerText.text = (survivalTime / 60).ToString("00") + ":" + (survivalTime % 60).ToString("00");
    }

    void Update()
    {
        
    }
}
