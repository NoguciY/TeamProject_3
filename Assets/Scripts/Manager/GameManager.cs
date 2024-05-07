using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //メイン画面の経過時間(秒)
    private float deltaTimeInMain;

    //ゲームクリア判定をするため
    //[SerializeField]
    //private EnemyBossSpawn enemyBossSpawn;

    //ゲームクリアかどうか
    //private bool isGameClear = false;

    //ゲッター
    public float GetDeltaTimeInMain => deltaTimeInMain;
    //public bool GetIsGameClear => isGameClear;

    private void Update()
    {
        //時間を計測
        deltaTimeInMain += Time.deltaTime;

        //isGameClear = enemyBossSpawn.CheckClearFlag();
    }
}
