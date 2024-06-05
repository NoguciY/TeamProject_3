using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField, Header("�������ԃe�L�X�g")]
    private TextMeshProUGUI timerText;          

    [SerializeField, Header("���x���e�L�X�g")]
    private TextMeshProUGUI levelText;     
    
    [SerializeField, Header("�|�����G�̐��e�L�X�g")]
    private TextMeshProUGUI enemyCountText;   

    float survivalTime;

    void Start()
    {
        //�������Ԃ�\��
        survivalTime = GameManager.Instance.GetDeltaTimeInMain;
        timerText.text = ((int)survivalTime / 60).ToString("00") + ":" + ((int)survivalTime % 60).ToString("00");

        //���݂̃��x����\��
        levelText.text = GameManager.Instance.playerLevel.ToString() + "Lv";

        //�|�����G�̐���\��
        enemyCountText.text = GameManager.Instance.deadEnemyMun.ToString() + "��";
    }
}
