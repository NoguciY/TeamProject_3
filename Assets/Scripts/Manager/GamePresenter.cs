using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MVPパターンを使ってゲームオーバーを作る
//これだけだと、このスクリプトを書かずにプレイヤーのPlayerコンポーネントのインスペクターに
//UIマネージャーの関数を登録すればいいと思う
//イベントの引数が動的な引数なら使う価値があるだろう
//プレイヤーとUIマネージャーがお互いを知らなくても登録できるのは、変更や修正が簡単だからいいと思う

public class GamePresenter : MonoBehaviour
{
    //プレイヤー
    [SerializeField]
    private Player player;

    //UIマネージャー
    [SerializeField]
    private UIManager uiManager;


    private void Awake()
    {
        //プレイヤーのイベントにUIマネージャーの関数を登録する

        //ゲームオーバー時に体力ゲージを0にする関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => uiManager.GetLifeGauge.GameOverLifeGauge());
        //ゲームオーバー時にゲームオーバーパネルを表示する関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true));
        //ゲームオーバー時にゲームをポーズする関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => Time.timeScale = 0);
        //ゲームオーバー時にコンティニューボタンを選択する関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => uiManager.GetButtonManager.GetGameOverContinueButton.Select());

        //経験値取得時に経験値ゲージを更新する関数を登録
        player.playerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //ダメージを受けた際に体力ゲージの更新を行う関数を登録
        player.playerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //レベルアップ時に体力ゲージの初期化を行う関数を登録
        player.playerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));
        //レベルアップ時にレベルアップパネルを表示する関数を登録
        player.playerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //レベルアップ時にゲームをポーズする関数を登録
        player.playerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);
        //レベルアップ時に強化ボタンに強化イベントを登録する関数を登録
        player.playerEvent.levelUpEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents());


        //UIマネージャーのイベントにプレイヤーの強化関数を登録

        //強化イベントにプレイヤーの最大体力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpMaxLife(player));

        //強化イベントにプレイヤーの移動速度強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpSpeed(player));

        //強化イベントにプレイヤーの回収範囲強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpCollectionRangeRate(player));

        //強化イベントにプレイヤーの防御力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpDifence(player));

        //強化イベントにプレイヤーの回復力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpResilience(player));

        //強化イベントにプレイヤーの爆弾の爆発範囲強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpBombRange(player));
    }
}
