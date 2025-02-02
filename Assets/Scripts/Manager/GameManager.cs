using System;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Title,      //タイトル
    MainGame,   //メインゲーム
    Option,     //オプション
    GameOver,   //ゲームオーバー
    Result,     //リザルト
}

public class GameManager : MonoBehaviour
{
    //シングルトンインスタンス
    private static GameManager instance;

    //現在のシーン
    [SerializeField]
    private SceneType currentSceneType;

    public SceneType CurrentSceneType
    {
        get { return currentSceneType; }
        set
        {
            currentSceneType = value;
            Debug.Log($"シーン変更{currentSceneType}");
        }
    }

    //前回のシーン
    private SceneType preSceneType;

    public SceneType PreSceneType
    {
        get { return preSceneType; }
        set { preSceneType = value; }
    }

    //メイン画面の経過時間(秒)
    private float deltaTimeInMain = 0;

    public float GetDeltaTimeInMain => deltaTimeInMain;

    //レベル
    [NonSerialized]
    public int playerLevel = 1;

    //倒した敵の数
    [NonSerialized]
    public int deadEnemyMun = 0;

    //フィールドの経験値のリスト
    public List<ItemExperienceValue> items;

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

        //フレームレートを設定
        Application.targetFrameRate = 60;

        //開始時のシーン
        preSceneType = currentSceneType;

        items = new List<ItemExperienceValue>();
    }

    private void Update()
    {
        //メインゲーム画面の場合
        if (currentSceneType == SceneType.MainGame)
        {
            //時間を計測
            deltaTimeInMain += Time.deltaTime;
        }

        Debug.Log($"現在のシーン:{currentSceneType}");
        Debug.Log($"前のシーン:{preSceneType}");

    }

    /// <summary>
    /// GameManagerインスタンスにアクセスする
    /// </summary>
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

    /// <summary>
    /// インスタンスをセットする
    /// </summary>
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
        playerLevel = 1;
        deadEnemyMun = 0;
    }

    /// <summary>
    /// 現在のシーンを変更する
    /// </summary>
    /// <param name="nextSceneType">次のシーン</param>
    public void ChangeSceneType(SceneType nextSceneType)
    {
        preSceneType = currentSceneType;
        currentSceneType = nextSceneType;
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
            SoundManager.uniqueInstance.PlayBgm("タイトル");
        }
        else if (nextSceneName == "MainScene")
        {
            currentSceneType = SceneType.MainGame;
            SoundManager.uniqueInstance.PlayBgm("メインゲーム");
        }
        else if (nextSceneName == "ResultScene")
        {
            currentSceneType = SceneType.Result;
            SoundManager.uniqueInstance.PlayBgm("リザルト");
        }
        else
        {
            Debug.LogWarning($"{nextSceneName}を現在のシーンに変更できません");
        }
    }
}
