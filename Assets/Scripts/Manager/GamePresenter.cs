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

    //サウンドマネージャー
    [SerializeField]
    private SoundManager soundManager;


    private void Start()
    {
        //プレイヤーのイベントにUIマネージャーの関数を登録する

        //ゲームオーバーイベントに関数を登録する
        //体力ゲージを0にする
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.GetLifeGauge.GameOverLifeGauge());
        //ゲームオーバーパネルを表示する
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true));
        //ゲームをポーズする
        player.GetPlayerEvent .gameOverEvent.AddListener(
            () => Time.timeScale = 0);
        //コンティニューボタンを選択する
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.GetButtonManager.GetGameOverContinueButton.Select());

        //経験値取得時に経験値ゲージを更新する関数を登録
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //ダメージを受けた際に体力ゲージの更新を行う関数を登録
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //ゲーム開始時に体力ゲージの初期化を行う関数を登録
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //レベルアップイベントに関数を登録する
        //レベルアップパネルを表示する
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //ゲームをポーズする関数を登録
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);
        //レベルアップ時に強化ボタンに強化イベントを登録する関数を登録
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents());
        //レベルアップ時に効果音を鳴らす関数を登録
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => soundManager.Play("レベルアップ"));

        //新しい爆弾を追加する時にレベルアップパネル(爆弾追加用)を表示する関数を登録
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true));
        //新しい爆弾を追加する時にゲームをポーズする関数を登録
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => Time.timeScale = 0);
        //新しい爆弾を追加する時に爆弾追加ボタンに爆弾追加イベントを登録する関数を登録
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter));
        //新しい爆弾を追加する時に効果音を鳴らす関数を登録
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => soundManager.Play("レベルアップ"));


        //UIマネージャーのイベントにプレイヤーの強化関数を登録

        //強化イベントにプレイヤーの最大体力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpMaxLife(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => soundManager.Play("最大体力アップ"));

        //強化イベントにプレイヤーの移動速度強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpSpeed(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => soundManager.Play("移動速度アップ"));

        //強化イベントにプレイヤーの回収範囲強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpCollectionRangeRate(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => soundManager.Play("回収範囲アップ"));

        //強化イベントにプレイヤーの防御力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpDifence(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => soundManager.Play("防御力アップ"));

        //強化イベントにプレイヤーの回復力強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpResilience(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => soundManager.Play("回復力アップ"));

        //強化イベントにプレイヤーの爆弾の爆発範囲強化関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpBombRange(player));
        //強化イベントに効果音を鳴らす関数を登録
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => soundManager.Play("爆発範囲アップ"));

        //爆弾追加イベントに新しい爆弾を使用可能にする関数を登録
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () => player.NewBombEnabled());
    }
}
