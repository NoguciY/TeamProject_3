using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//ボタンをまとめて管理する

public class ButtonManager : MonoBehaviour
{
    private const int powerUpButtonNum = 3;

    //パワーアップボタン
    [SerializeField]
    private  Button[] powerUpButtons = new Button[powerUpButtonNum];


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
    public UnityEvent[] powerUpEvents = new UnityEvent[powerUpButtonNum];
    //各強化関数のリスナーを保持する配列
    private UnityAction[] powerUpListeners = { null, null, null};

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

    //リストに強化可能な強化項目を追加する
    public void SetPowerUpItems()
    {
        //強化項目イベントリストに強化可能な強化項目を格納
        List<UnityEvent> events = new List<UnityEvent>();
        //初期の強化項目を追加する
        events.Add(powerUpMaxLifeEvent);
        events.Add(powerUpSpeedEvent);
        events.Add(powerUpCollectionRangeRateEvent);
        events.Add(powerUpDifenceEvent);
        events.Add(powerUpResilienceEvent);
        events.Add(powerUpBombRangeEvent);

        //削除する

        
        
    }

    //リストからランダムに強化項目ボタンの数分、配列に格納

}
