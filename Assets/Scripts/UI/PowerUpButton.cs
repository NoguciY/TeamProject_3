using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    //強化項目ボタンの数
    private const int powerUpButtonNum = 3;

    //パワーアップボタン
    public Button[] powerUpButtons = new Button[powerUpButtonNum];

    //爆弾追加ボタン
    public Button addNewBombButton;

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
    public PowerUpEvent powerUpDifenceEvent = new PowerUpEvent { };
    
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
    private Sprite[] powerUpButtonsSprites = new Sprite[powerUpButtonNum];

    //強化ボタンに登録する強化イベントを格納する配列
    [NonSerialized]
    public UnityEvent[] powerUpEvents = new UnityEvent[powerUpButtonNum];

    //強化ボタンに登録する強化イベントを保持する配列(リスナー)
    private UnityAction[] powerUpListeners = new UnityAction[powerUpButtonNum];

    //強化ボタンに登録する強化イベントの候補リスト
    private List<PowerUpEvent> powerUpEventList = new List<PowerUpEvent>(powerUpButtonNum);

    //爆弾追加ボタン画像
    [SerializeField]
    private Sprite[] addNewBombButtons;

    //爆弾追加ボタンに登録するイベント
    [NonSerialized]
    public UnityEvent addNewBombEvent = new UnityEvent();

    //爆弾追加ボタンに登録するイベントを保持する
    private UnityAction addNewBombListener = null;

    //強化イベントクラスのspriteに強化ボタン画像を設定
    public void SetPowerUpButtoSprites()
    {
        powerUpMaxLifeEvent.powerUpButtonSprite = powerUpButtonSpriteList[0];
        powerUpSpeedEvent.powerUpButtonSprite = powerUpButtonSpriteList[1];
        powerUpDifenceEvent.powerUpButtonSprite = powerUpButtonSpriteList[2];
        powerUpCollectionRangeRateEvent.powerUpButtonSprite = powerUpButtonSpriteList[3];
        powerUpResilienceEvent.powerUpButtonSprite = powerUpButtonSpriteList[4];
        powerUpBombRangeEvent.powerUpButtonSprite = powerUpButtonSpriteList[5];
    }

    //レベルアップイベントで呼ばれる
    //強化ボタンに強化関数を登録する
    public void RegisterPowerUpItemEvents()
    {
        //リストに強化可能な強化イベントを追加する
        SetPowerUpItemsList();
        //リストからランダムに強化項目ボタンの数分、配列に格納
        GetRandomPowerUpItemEvents();

        for (int i = 0; i < powerUpButtonNum; i++)
        {
            //前回ボタンに登録したリスナー(強化関数のみ)を削除する
            if (powerUpListeners[i] != null)
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);

            //ラムダ式で引数ありのリスナーを登録するために必要
            int index = i;

            //ボタンに登録するリスナーを保持する
            powerUpListeners[index] = () => powerUpEvents[index].Invoke();

            //ボタンにリスナーを登録する
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);

            //ボタンの画像を変更
            powerUpButtons[i].image.sprite = powerUpButtonsSprites[i];
        }
    }

    //リストに強化可能な強化イベントを追加する
    private void SetPowerUpItemsList()
    {
        powerUpEventList.Clear();

        //強化項目イベントリストに強化可能な強化項目を格納
        //初期の強化項目を追加する
        powerUpEventList.Add(powerUpMaxLifeEvent);
        powerUpEventList.Add(powerUpSpeedEvent);
        powerUpEventList.Add(powerUpCollectionRangeRateEvent);
        powerUpEventList.Add(powerUpDifenceEvent);
        powerUpEventList.Add(powerUpResilienceEvent);
        powerUpEventList.Add(powerUpBombRangeEvent);

        //追加される強化項目や最終強化して削除される強化項目があるため
        //ここでリストに追加や削除をする処理も記述する
    }

    //強化イベントリストからランダムに強化項目ボタンの数だけ、配列に格納
    private void GetRandomPowerUpItemEvents()
    {
        //リストの数
        int listNum = powerUpEventList.Count;

        //1回のランダムな値(初期値には、ランダムでは選ばれない値であるリストの数を格納)
        int beforeRndFirst = listNum;
        //2回のランダムな値
        int beforeRndSecond = listNum;

        int i = 0;

        while(i < powerUpButtonNum)
        {
            //0からリストの数-1の整数からランダムな値を格納
            int rnd = UnityEngine.Random.Range(0, listNum);

            //同じイベントが配列に格納されないようにする
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //Debug.Log($"rnd:{rnd}");
                //配列に追加する
                powerUpEvents[i] = powerUpEventList[rnd];
                powerUpButtonsSprites[i] = powerUpEventList[rnd].powerUpButtonSprite;
                Debug.Log($"powerUpEvents[{i}]にevents[{rnd}]を追加");

                //1回目のランダムな値を格納
                if (i == 0)
                    beforeRndFirst = rnd;
                //2回目のランダムな値を格納
                else if(i == 1)
                    beforeRndSecond = rnd;
                i++;
            }
        }
    }

    //爆弾追加イベントで呼ばれる
    //爆弾追加ボタンに爆弾追加関数を登録する
    public void RegisterAddNewBombEvent()
    {
        //前回ボタンに登録したリスナー(爆弾追加関数のみ)を削除する
        //addNewBombButton.onClick.RemoveListener(addNewBombListener);

        //ラムダ式で引数ありのリスナーを登録するために必要


        //ボタンに登録するリスナーを保持する


        //ボタンに登録するリスナーを登録する
        //addNewBombButton.onClick.AddListener(powerUpListeners);

        //ボタンの画像を変更
        //addNewBombButton.image.sprite = 
    }
}
