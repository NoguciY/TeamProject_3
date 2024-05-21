using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;          //生存時間テキスト
    [SerializeField]
    private TextMeshProUGUI levelText;          //レベルテキスト
    [SerializeField]
    private TextMeshProUGUI enemyCountText;     //倒した敵の数テキスト

    float survivalTime;

    void Start()
    {
        //生存時間を表示
        survivalTime = GameManager.Instance.GetDeltaTimeInMain;
        timerText.text = ((int)survivalTime / 60).ToString("00") + ":" + ((int)survivalTime % 60).ToString("00");

        //現在のレベルを表示
        levelText.text = GameManager.Instance.lastPlayerLevel.ToString() + "Lv";

        //倒した敵の数を表示
        enemyCountText.text = GameManager.Instance.deadEnemyMun.ToString() + "体";
    }
}
