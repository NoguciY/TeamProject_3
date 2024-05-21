using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;          //�������ԃe�L�X�g
    [SerializeField]
    private TextMeshProUGUI levelText;          //���x���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI enemyCountText;     //�|�����G�̐��e�L�X�g

    float survivalTime;

    void Start()
    {
        //�������Ԃ�\��
        survivalTime = GameManager.Instance.GetDeltaTimeInMain;
        timerText.text = ((int)survivalTime / 60).ToString("00") + ":" + ((int)survivalTime % 60).ToString("00");

        //���݂̃��x����\��
        levelText.text = GameManager.Instance.lastPlayerLevel.ToString() + "Lv";

        //�|�����G�̐���\��
        enemyCountText.text = GameManager.Instance.deadEnemyMun.ToString() + "��";
    }
}
