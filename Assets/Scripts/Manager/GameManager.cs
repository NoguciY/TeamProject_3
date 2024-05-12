using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private static GameManager instance;

    private void Update()
    {
        //時間を計測
        deltaTimeInMain += Time.deltaTime;

        //isGameClear = enemyBossSpawn.CheckClearFlag();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<GameManager>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gameObj);
        }
    }
}
