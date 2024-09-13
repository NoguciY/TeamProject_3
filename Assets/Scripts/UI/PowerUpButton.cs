using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Utilities;

public class PowerUpButton : MonoBehaviour
{
    //強化項目ボタンの数
    private const int POWERUPBUTTONNUM = 3;

    //パワーアップボタン
    public Button[] powerUpButtons = new Button[POWERUPBUTTONNUM];

    //強化イベント
    public class PowerUpEvent : UnityEvent
    {
        //イベントに合った強化ボタン画像
        public Sprite powerUpButtonSprite; 
    }

    //最大HP強化イベント
    public PowerUpEvent powerUpMaxLifeEvent = new PowerUpEvent { };

    //移動速度強化イベント
    public PowerUpEvent powerUpSpeedEvent = new PowerUpEvent { };
    
    //防御力強化イベント
    public PowerUpEvent powerUpDifenseEvent = new PowerUpEvent { };
    
    //回収範囲強化イベント
    public PowerUpEvent powerUpCollectionRangeRateEvent = new PowerUpEvent { };
    
    //回復力強化イベント
    public PowerUpEvent powerUpResilienceEvent = new PowerUpEvent { };
    
    //爆弾の範囲強化イベント
    public PowerUpEvent powerUpBombRangeEvent = new PowerUpEvent { };
    
    //強化イベントクラスを格納するリスト
    //private List<PowerUpEvent> powerUpMEventList = new List<PowerUpEvent>(powerUpButtonNum);
    
    //強化ボタン画像リスト
    [SerializeField]
    private List<Sprite> powerUpButtonSpriteList;
    
    //強化ボタンに登録する強化ボタン画像を格納する配列
    private Sprite[] powerUpButtonSprites = new Sprite[POWERUPBUTTONNUM];

    //強化ボタンに登録する強化イベントを格納する配列
    [NonSerialized]
    public UnityEvent[] powerUpEvents = new UnityEvent[POWERUPBUTTONNUM];

    //強化ボタンに登録する強化イベントを保持する配列(リスナー)
    private UnityAction[] powerUpListeners = new UnityAction[POWERUPBUTTONNUM];

    //強化ボタンに登録する強化イベントの候補リスト
    private List<PowerUpEvent> powerUpEventList = new List<PowerUpEvent>(POWERUPBUTTONNUM);


    //爆弾追加ボタン
    public Button addNewBombButton;

    //爆弾追加ボタン画像
    [SerializeField]
    private Sprite[] addNewBombButtonSprites;

    //爆弾追加ボタンに登録するイベント
    [NonSerialized]
    public UnityEvent addNewBombEvent = new UnityEvent();

    //爆弾追加ボタンに登録するイベントを保持する
    private UnityAction addNewBombListener = null;

    private void Awake()
    {
        //powerUpButtons = new Button[POWERUPBUTTONNUM];

        //powerUpMaxLifeEvent = new PowerUpEvent { };
        //powerUpSpeedEvent = new PowerUpEvent { };
        //powerUpDifenseEvent = new PowerUpEvent { };
        //powerUpCollectionRangeRateEvent = new PowerUpEvent { };
        //powerUpResilienceEvent = new PowerUpEvent { };
        //powerUpBombRangeEvent = new PowerUpEvent { };

        //powerUpButtonSprites = new Sprite[POWERUPBUTTONNUM];

        //powerUpEvents = new UnityEvent[POWERUPBUTTONNUM];

        //powerUpListeners = new UnityAction[POWERUPBUTTONNUM];

        //powerUpEventList = new List<PowerUpEvent>(POWERUPBUTTONNUM);


        //addNewBombEvent = new UnityEvent();
        //addNewBombListener = null;
    }

    /// <summary>
    /// 強化イベントクラスのspriteに強化ボタン画像を設定
    /// </summary>
    public void SetPowerUpButtoSprites()
    {
        powerUpMaxLifeEvent.powerUpButtonSprite = powerUpButtonSpriteList[0];
        powerUpSpeedEvent.powerUpButtonSprite = powerUpButtonSpriteList[1];
        powerUpDifenseEvent.powerUpButtonSprite = powerUpButtonSpriteList[2];
        powerUpCollectionRangeRateEvent.powerUpButtonSprite = powerUpButtonSpriteList[3];
        powerUpResilienceEvent.powerUpButtonSprite = powerUpButtonSpriteList[4];
        powerUpBombRangeEvent.powerUpButtonSprite = powerUpButtonSpriteList[5];
    }

    /// <summary>
    /// レベルアップイベントで呼ばれる
    /// 強化ボタンに強化関数を登録する
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void RegisterPowerUpItemEvents(PlayerManager player)
    {
        //リストに強化可能な強化イベントを追加する
        SetPowerUpItemsList(player);

        //リストからランダムに強化項目ボタンの数分、配列に格納
        GetRandomPowerUpItemEvents();

        for (int i = 0; i < POWERUPBUTTONNUM; i++)
        {
            //前回ボタンに登録したリスナー(強化関数のみ)を削除する
            if (powerUpListeners[i] != null)
            {
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);
            }

            //ラムダ式で引数ありのリスナーを登録するため必要
            int index = i;

            //ボタンに登録するリスナーを保持する
            powerUpListeners[index] = () => powerUpEvents[index].Invoke();

            //ボタンにリスナーを登録する
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);

            //ボタンの画像を変更
            powerUpButtons[i].image.sprite = powerUpButtonSprites[i];
        }
    }

    /// <summary>
    /// リストに強化可能な強化イベントを追加する
    /// </summary>
    /// <param name="player">PlayerManager</param>
    private void SetPowerUpItemsList(PlayerManager player)
    {
        powerUpEventList.Clear();

        //強化項目イベントリストに強化可能な強化項目を格納
        //初期の強化項目を追加する
        powerUpEventList.Add(powerUpMaxLifeEvent);

        //速さが上限値より下の場合、追加
        if (player.speed < Utilities.SPEEDUPPERLIMIT)
        {
            powerUpEventList.Add(powerUpSpeedEvent);
        }

        powerUpEventList.Add(powerUpCollectionRangeRateEvent);

        //防御力が上限値より下の場合、追加
        if (player.difense < Utilities.DEFENSEUPPERLIMIT)
        {
            powerUpEventList.Add(powerUpDifenseEvent);
        }

        powerUpEventList.Add(powerUpResilienceEvent);

        //設置型爆弾が使用可能の場合、追加
        if (player.GetBombManager.CanUseBumb((int)BombManager.BombType.Planted))
        {
            powerUpEventList.Add(powerUpBombRangeEvent);
        }
    }

    /// <summary>
    /// 強化イベントリストからランダムに強化項目ボタンの数だけ、配列に格納
    /// </summary>
    private void GetRandomPowerUpItemEvents()
    {
        //リストの数
        int listNum = powerUpEventList.Count;

        //1回のランダムな値(初期値には、ランダムでは選ばれない値であるリストの数を格納)
        int beforeRndFirst = listNum;
        //2回のランダムな値
        int beforeRndSecond = listNum;

        int i = 0;

        while(i < POWERUPBUTTONNUM)
        {
            //0からリストの数-1の整数からランダムな値を格納
            int rnd = UnityEngine.Random.Range(0, listNum);

            //同じイベントが配列に格納されないようにする
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //配列に追加する
                powerUpEvents[i] = powerUpEventList[rnd];
                powerUpButtonSprites[i] = powerUpEventList[rnd].powerUpButtonSprite;
                //Debug.Log($"powerUpEvents[{i}]にevents[{rnd}]を追加");

                //1回目のランダムな値を格納
                if (i == 0)
                {
                    beforeRndFirst = rnd;
                }
                //2回目のランダムな値を格納
                else if (i == 1)
                {
                    beforeRndSecond = rnd;
                }

                i++;
            }
        }
    }

    /// <summary>
    /// 爆弾追加イベントに登録する関数
    /// 爆弾追加ボタンに爆弾追加関数を登録する
    /// </summary>
    /// <param name="newBombID">新しく追加する爆弾の固有番号</param>
    public void RegisterAddNewBombEvent(int newBombID)
    {
        //前回ボタンに登録したリスナー(爆弾追加関数のみ)を削除する
        if(addNewBombListener != null)
        addNewBombButton.onClick.RemoveListener(addNewBombListener);

        //ボタンを押した時に実行するイベントを登録する
        addNewBombListener = () => addNewBombEvent.Invoke();

        //ボタンにリスナーを登録する
        addNewBombButton.onClick.AddListener(addNewBombListener);

        //ボタンの画像を変更 , newBombID - 1：newBombIDは1から始まるため -1 している
        addNewBombButton.image.sprite = addNewBombButtonSprites[newBombID - 1];
    }
}
