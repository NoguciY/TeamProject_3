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
    //シングルトンインスタンス
    private static GameManager instance;

    //現在のシーン
    private SceneType currentSceneType;

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
            Destroy(gameObject);
        
        //開始時のシーン
        currentSceneType = SceneType.MainGame;
    }

    private void Update()
    {
        //メインゲーム画面の場合
        if(currentSceneType == SceneType.MainGame)
        //時間を計測
        deltaTimeInMain += Time.deltaTime;
    }

    //GameManagerインスタンスにアクセスする
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                //インスタンスをセット
                SetupInstance();
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

    /// <summary>
    /// 現在のシーンを変更する
    /// </summary>
    /// <param name="nextSceneType">次のシーン</param>
    public void ChangeSceneType(SceneType nextSceneType)
    {
        currentSceneType = nextSceneType;
        Debug.Log($"現在のシーン：{currentSceneType}");
    }

    /// <summary>
    /// 現在のシーンを変更する
    /// </summary>
    /// <param name="loadSceneName">次のシーン名</param>
    public void ChangeSceneType(string nextSceneName)
    {
        //シーン名からSceneTypeに変換
        if (nextSceneName == "TitleScene")
            currentSceneType = SceneType.Title;
        else if (nextSceneName == "MainScene")
            currentSceneType = SceneType.MainGame;
        else if(nextSceneName == "ResultScene")
            currentSceneType = SceneType.Result;
        else
            Debug.LogWarning($"{nextSceneName}を現在のシーンに変更できません");

        Debug.Log($"現在のシーン：{currentSceneType}");
    }
}
