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

    //強化イベント
    public class PowerUpMaxLifeEvent : UnityEvent<int> { }
    public PowerUpMaxLifeEvent powerUpMaxLifeEvent = new PowerUpMaxLifeEvent();

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


    //強化ボタンに強化関数を登録する
    public void RegisterPowerUpFunc()
    {
        powerUpButtons[0].onClick.AddListener(() => powerUpMaxLifeEvent.Invoke(2) );
    }

    //ランダムで強化項目を決める

}
