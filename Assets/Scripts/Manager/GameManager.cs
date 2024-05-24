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

    //前回のシーン
    private SceneType preSceneType;

    //メイン画面の経過時間(秒)
    private float deltaTimeInMain = 0;

    //最終レベル
    public int lastPlayerLevel = 0;

    //倒した敵の数
    public int deadEnemyMun = 0;

    //ゲッター
    public float GetDeltaTimeInMain => deltaTimeInMain;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        //開始時のシーン
        currentSceneType = SceneType.MainGame;
        preSceneType = currentSceneType;
    }

    private void Update()
    {
        //メインゲーム画面の場合
        if (currentSceneType == SceneType.MainGame)
            //時間を計測
            deltaTimeInMain += Time.deltaTime;

        Debug.Log($"現在のシーン:{currentSceneType}");
        Debug.Log($"前のシーン:{preSceneType}");
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

    //リザルトでの値をリセットする
    private void ResetResult()
    {
        deltaTimeInMain = 0;
        lastPlayerLevel = 0;
        deadEnemyMun = 0;
    }

    /// <summary>
    /// 現在のシーンを変更する
    /// </summary>
    /// <param name="nextSceneType">次のシーン</param>
    public void ChangeSceneType(SceneType nextSceneType)
    {
        //前回と同じシーンに変更させない
        if (preSceneType != currentSceneType)
        {
            preSceneType = currentSceneType;
            currentSceneType = nextSceneType;
            //Debug.Log($"現在のシーン：{currentSceneType}");
        }
    }

    /// <summary>
    /// 現在のシーンを変更する
    /// </summary>
    /// <param name="loadSceneName">次のシーン名</param>
    public void ChangeSceneType(string nextSceneName)
    {
        preSceneType = currentSceneType;

        //シーン名から現在のSceneTypeを変更

        if (nextSceneName == "TitleScene")
        {
            currentSceneType = SceneType.Title;
            ResetResult();
            SoundManager.uniqueInstance.PlayBgm("タイトルBGM");
        }
        else if (nextSceneName == "MainScene")
        {
            currentSceneType = SceneType.MainGame;
            SoundManager.uniqueInstance.PlayBgm("メインゲームBGM");
        }
        else if (nextSceneName == "ResultScene")
        {
            currentSceneType = SceneType.Result;
            SoundManager.uniqueInstance.PlayBgm("リザルトBGM");
        }
        else
            Debug.LogWarning($"{nextSceneName}を現在のシーンに変更できません");

        //Debug.Log($"現在のシーン：{currentSceneType}");
    }
}
