//using System;
using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//ボタンをまとめて管理する

public class ButtonManager : MonoBehaviour
{
    //ゲームオーバー画面のコンティニューボタン
    [SerializeField]
    private Button gameOverContinueButton;

    //強化ボタンコンポーネント
    public PowerUpButton powerUpButton;


    //ゲッター
    public Button GetGameOverContinueButton => gameOverContinueButton;

    //ボタンの初期化(イベントの登録)
    public void Initialize(UIManager uIManager)
    {
        //事前にパワーアップボタンにポーズをやめる関数と
        //レベルアップパネルを非表示にする関数を登録する
        foreach (var button in powerUpButton.powerUpButtons)
        {
            button.onClick.AddListener(() => Time.timeScale = 1);
            button.onClick.AddListener(() => uIManager.ShoulShowPanel(uIManager.GetLevelUpPanel, false));
        }

        //イベントに合った強化ボタンの画像を設定する
        powerUpButton.SetPowerUpButtoSprites();

        //事前に爆弾追加ボタンにポーズをやめる関数と
        //レベルアップパネルを非表示にする関数を登録する
        powerUpButton.addNewBombButton.onClick.AddListener(() => Time.timeScale = 1);
        powerUpButton.addNewBombButton.onClick.AddListener(
            () => uIManager.ShoulShowPanel(uIManager.GetLevelUpBombPanel, false));
    }
}
