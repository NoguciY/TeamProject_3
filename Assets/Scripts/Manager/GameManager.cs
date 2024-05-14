using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SceneType
{
    Title,      //タイトル
    MainGame,   //メインゲーム
    GameOver,   //ゲームオーバー
    Result,     //リザルト
}

public class GameManager : MonoBehaviour
{
    //メイン画面の経過時間(秒)
    private float deltaTimeInMain;

    //ゲッター
    public float GetDeltaTimeInMain => deltaTimeInMain;

    private static GameManager instance;

    private void Update()
    {
        //メインゲーム画面の場合

        //時間を計測
        deltaTimeInMain += Time.deltaTime;

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
