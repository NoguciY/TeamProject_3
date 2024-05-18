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
    private static GameManager instance;

    //メイン画面の経過時間(秒)
    private float deltaTimeInMain;

    //ゲッター
    public float GetDeltaTimeInMain => deltaTimeInMain;


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

    private void Update()
    {
        //メインゲーム画面の場合

        //時間を計測
        deltaTimeInMain += Time.deltaTime;
    }

    //GameManagerインスタンスにアクセスする
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                //インスタンスをセット
                SetupInstance();
            }
            return instance;
        }
    }

    //インスタンスをセットする
    private static void SetupInstance()
    {
        //シーン内に存在するGameManagerを検索
        instance = FindObjectOfType<GameManager>();

        //シーン内にない場合、インスタンスを作成する
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gameObj);
        }
    }
}
