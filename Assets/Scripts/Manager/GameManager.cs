using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���C����ʂ̌o�ߎ���(�b)
    private float deltaTimeInMain;

    //�Q�[���N���A��������邽��
    //[SerializeField]
    //private EnemyBossSpawn enemyBossSpawn;

    //�Q�[���N���A���ǂ���
    //private bool isGameClear = false;

    //�Q�b�^�[
    public float GetDeltaTimeInMain => deltaTimeInMain;
    //public bool GetIsGameClear => isGameClear;

    private void Update()
    {
        //���Ԃ��v��
        deltaTimeInMain += Time.deltaTime;

        //isGameClear = enemyBossSpawn.CheckClearFlag();
    }
}
