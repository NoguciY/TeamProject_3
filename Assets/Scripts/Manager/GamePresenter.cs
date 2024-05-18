using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MVPパターンのプレゼンター
//UIとプレイヤーのイベントに登録する
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


    private void Awake()
    {
        //プレイヤーのイベントにUIマネージャーとサウンドマネージャーの関数を登録する

        //ゲームオーバーイベント
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

        //経験値取得時に経験値ゲージを更新する
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //ダメージを受けた際に体力ゲージの更新する
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //ゲーム開始時に体力ゲージの初期化する
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //レベルアップイベント
        //レベルアップパネルを表示する
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //ゲームをポーズする
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);
        //強化ボタンに強化イベントを登録する
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents());
        //レベルアップ時に効果音を鳴らす
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => soundManager.Play("レベルアップ"));

        //爆弾追加用レベルアップイベント
        //レベルアップパネル(爆弾追加用)を表示する
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true));
        //ゲームをポーズする
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => Time.timeScale = 0);
        //爆弾追加ボタンに爆弾追加イベントを登録する
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter));
        //効果音を鳴らす
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => soundManager.Play("レベルアップ"));


        //UIマネージャーのイベントにプレイヤーとサウンドマネージャーの関数を登録する

        //強化イベント
        //プレイヤーの最大体力強化
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpMaxLife(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => soundManager.Play("最大体力アップ"));

        //プレイヤーの移動速度強化
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpSpeed(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => soundManager.Play("移動速度アップ"));

        //プレイヤーの回収範囲強化
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpCollectionRangeRate(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => soundManager.Play("回収範囲アップ"));

        //プレイヤーの防御力強化
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpDifence(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => soundManager.Play("防御力アップ"));

        //プレイヤーの回復力強化
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpResilience(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => soundManager.Play("回復力アップ"));

        //プレイヤーの爆弾の爆発範囲強化
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpBombRange(player));
        //効果音を鳴らす
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => soundManager.Play("爆発範囲アップ"));

        //爆弾追加イベント
        //新しい爆弾を使用可能にする
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () => player.EnableNewBomb());
    }
}
