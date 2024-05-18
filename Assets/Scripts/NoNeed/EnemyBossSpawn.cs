using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSpawn : MonoBehaviour
{
    //敵プレハブ
    [SerializeField]
    private GameObject[] enemyPrefab = new GameObject[3];
    //敵生成時間間隔
    private float interval;
    //経過時間
    private float time = 0f;

    //最後のボスが出現したかどうか
    bool isLastBoss;

    void Start()
    {
        //時間間隔を決定する
        interval = 5f;

        isLastBoss = false;
    }

    void Update()
    {
        //時間計測
        time += Time.deltaTime;

        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            if (time > interval)
            {
                //enemyをインスタンス化する(生成する)
                GameObject enemy = Instantiate(enemyPrefab[i]);
                this.enemyPrefab[i].SetActive(true);
                //生成した敵の座標を決定する(現状X=0,Y=50,Z=20の位置に出力)
                enemy.transform.position = new Vector3(0, 50, 20);
                //経過時間を初期化して再度時間計測を始める
                time = 0f;
            }
        }
    }

    //ゲームクリア判定をする
    public bool CheckClearFlag()
    {
        //クリアしているか
        bool isClear = false;

        //最後のボスがやられた場合、クリア
        if (!isLastBoss && enemyPrefab[2] != null)
        {
            isLastBoss = true;
        }

        if (isLastBoss && enemyPrefab[2] == null)
        {
            isClear = true;
        }

        return isClear;
    }
}
