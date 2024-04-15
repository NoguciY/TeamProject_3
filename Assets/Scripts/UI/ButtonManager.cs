using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//ボタンをまとめて管理する

public class ButtonManager : MonoBehaviour
{
    //強化項目ボタンの数
    private const int powerUpButtonNum = 3;

    //パワーアップボタン
    [SerializeField]
    private  List<Button> powerUpButtons = new List<Button>();


    //最大HP強化イベント
    [NonSerialized]
    public UnityEvent powerUpMaxLifeEvent = new UnityEvent();
    //移動速度強化イベント
    [NonSerialized]
    public UnityEvent powerUpSpeedEvent = new UnityEvent();
    //回収範囲強化イベント
    [NonSerialized]
    public UnityEvent powerUpCollectionRangeRateEvent = new UnityEvent();
    //防御力強化イベント
    [NonSerialized]
    public UnityEvent powerUpDifenceEvent = new UnityEvent();
    //回復力強化イベント
    [NonSerialized]
    public UnityEvent powerUpResilienceEvent = new UnityEvent();
    //爆弾攻撃力強化イベント
    [NonSerialized]
    public UnityEvent powerUpBombAttackEvent = new UnityEvent();
    //爆弾の範囲強化イベント
    [NonSerialized]
    public UnityEvent powerUpBombRangeEvent = new UnityEvent();

    //各強化関数のリスナーを保持する変数(UnityActionのインスタンス)
    //リスナーを保持しておかないと削除できないため
    private UnityAction powerUpMaxLifeListener = null;
    private UnityAction powerUpSpeedListener = null;
    private UnityAction powerUpCollectionRangeRateListener = null;


    //強化イベント配列
    [NonSerialized]
    public List<UnityEvent> powerUpEvents = new List<UnityEvent>();
    //各強化関数のリスナーを保持する配列
    private List<UnityAction> powerUpListeners = new List<UnityAction>();
    //private UnityAction[] powerUpListeners = new UnityAction[powerUpButtonNum];

    //強化項目イベントリスト
    private List<UnityEvent> events = new List<UnityEvent>();

    //ボタンの初期化(イベントの登録)
    public void InitButton(UIManager uIManager)
    {
        //パワーアップボタンにポーズをやめる関数と
        //レベルアップパネルを非表示にする関数を登録する
        foreach (var button in powerUpButtons)
        {
            button.onClick.AddListener(() => Time.timeScale = 1);
            button.onClick.AddListener(() => uIManager.ShoulShowPanel(uIManager.GetLevelUpPanel, false));
        }
    }

    //レベルアップイベントで呼ばれる
    //強化ボタンに強化関数を登録する
    public void RegisterPowerUpFunc()
    {
        //前回ボタンに登録したリスナー(強化関数のみ)を削除する
        if (powerUpMaxLifeListener != null)
        {
            powerUpButtons[0].onClick.RemoveListener(powerUpMaxLifeListener);
            powerUpButtons[1].onClick.RemoveListener(powerUpSpeedListener);
            powerUpButtons[2].onClick.RemoveListener(powerUpCollectionRangeRateListener);
        }

        //ボタンに登録するリスナーを保持する
        powerUpMaxLifeListener = () => powerUpMaxLifeEvent.Invoke();
        powerUpSpeedListener = () => powerUpSpeedEvent.Invoke();
        powerUpCollectionRangeRateListener = () => powerUpCollectionRangeRateEvent.Invoke();

        //ボタンにリスナーを登録する
        powerUpButtons[0].onClick.AddListener(powerUpMaxLifeListener);
        powerUpButtons[1].onClick.AddListener(powerUpSpeedListener);
        powerUpButtons[2].onClick.AddListener(powerUpCollectionRangeRateListener);
    }

    //レベルアップイベントで呼ばれる
    //強化ボタンに強化関数を登録する
    public void RegisterPowerUpItemEvents()
    {
        SetPowerUpItemsList();
        GetRandomPowerUpItemEvents();

        for(int i = 0; i < powerUpButtonNum; i++)
        {
            //前回ボタンに登録したリスナー(強化関数のみ)を削除する
            if (powerUpListeners[i] != null)
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);

            //ボタンに登録するリスナーを保持する
            powerUpListeners[i] = () => powerUpEvents[i].Invoke();

            //ボタンにリスナーを登録する
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);
        }
    }

    //リストに強化可能な強化項目を追加する
    private void SetPowerUpItemsList()
    {
        events.Clear();

        //強化項目イベントリストに強化可能な強化項目を格納
        //初期の強化項目を追加する
        events.Add(powerUpMaxLifeEvent);
        events.Add(powerUpSpeedEvent);
        events.Add(powerUpCollectionRangeRateEvent);
        events.Add(powerUpDifenceEvent);
        events.Add(powerUpResilienceEvent);
        events.Add(powerUpBombRangeEvent);

        //追加される強化項目や最終強化して削除される強化項目があるため
        //ここでリストに追加や削除をする処理も記述する
    }

    //リストからランダムに強化項目ボタンの数分、配列に格納
    private void GetRandomPowerUpItemEvents()
    {
        //リストの数
        int listNum = events.Count;

        //1回のランダムな値(初期値には、ランダムでは選ばれない値であるリストの数を格納)
        int beforeRndFirst = listNum;
        //2回のランダムな値
        int beforeRndSecond = listNum;

        int i = 0;

        //配列の数分リストに格納する
        while(i < powerUpButtonNum)
        {
            //0からリストの数-1の整数からランダムな値を格納
            int rnd = UnityEngine.Random.Range(0, listNum);

            //同じイベントが配列に格納されないようにする
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //Debug.Log($"rnd:{rnd}");
                //配列に追加する
                powerUpEvents[i] = events[rnd];
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
}
