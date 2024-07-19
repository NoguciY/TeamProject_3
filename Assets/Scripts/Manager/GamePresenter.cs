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

    private void Awake()
    {
        //プレイヤーのイベントに関数を登録する

        //最大体力取得イベント
        //体力ゲージの初期化する
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //経験値取得イベント
        //経験値ゲージを更新する
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));

        //被ダメージイベント
        //体力ゲージの更新する
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //ゲームオーバーイベント
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () =>{
                //体力ゲージを0にする
                uiManager.GetLifeGauge.GameOverLifeGauge();

                //ゲームオーバーパネルを表示する
                uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true);

                //ゲームをポーズする
                Time.timeScale = 0;

                //コンティニューボタンを選択する
                uiManager.GetButtonManager.GetGameOverContinueButton.Select();

                //プレイヤーの最終レベルを取得する
                GameManager.Instance.playerLevel = player.GetPlayerLevelUp.GetLevel;

                //シーンタイプを変更する
                GameManager.Instance.ChangeSceneType(SceneType.GameOver);

                //BGMを流す
                SoundManager.uniqueInstance.PlayBgm("ゲームオーバー");
            });

        //レベルアップイベント
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => {
                //レベルアップパネルを表示する
                uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true);

                //ゲームをポーズする
                Time.timeScale = 0;

                //強化ボタンに強化イベントを登録する
                uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents(player);

                //レベルアップ時に効果音を鳴らす
                SoundManager.uniqueInstance.Play("レベルアップ");

                //ゲームマネージャーに保存しているプレイヤーレベルをインクリメント
                GameManager.Instance.playerLevel++;
            });

        //爆弾追加用レベルアップイベント
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () =>{
                //レベルアップパネル(爆弾追加用)を表示する
                uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true);

                //ゲームをポーズする
                Time.timeScale = 0;

                //爆弾追加ボタンに爆弾追加イベントを登録する
                uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter);

                //爆弾アイコンの表示
                uiManager.ShouwBombIcon();

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("レベルアップ");

                //ゲームマネージャーに保存しているプレイヤーレベルをインクリメント
                GameManager.Instance.playerLevel++;
            });

        //クールダウンイベント
        player.GetPlayerEvent.coolDownEvent.AddListener(
            (index, coolTime) => {
                //使用した爆弾のクールタイムゲージを満たし始める
                uiManager.GetCoolTimeGauge.StartFillingCoolTimeGauge(index, coolTime);
            });

        //UIマネージャーのイベントに関数を登録する

        //最大体力強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener( 
            () => {
                //プレイヤーの最大体力強化
                player.GetPowerUpItems.PowerUpMaxLife(player);

                //体力ゲージの最大値を更新する
                uiManager.GetLifeGauge.SetMaxValue = player.maxLife;

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("最大体力アップ");
            });

        //移動速度強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () =>{
                //プレイヤーの移動速度強化
                player.GetPowerUpItems.PowerUpSpeed(player);

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("移動速度アップ");
            });

        //回収範囲強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => {
                //プレイヤーの回収範囲強化
                player.GetPowerUpItems.PowerUpCollectionRangeRate(player);

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("回収範囲アップ");
            });
        
        //防御力強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpDifenseEvent.AddListener(
            () =>{
                //プレイヤーの防御力強化
                player.GetPowerUpItems.PowerUpDifence(player);

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("防御力アップ");
            });

        //回復力強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () =>{
                //プレイヤーの回復力強化
                player.GetPowerUpItems.PowerUpResilience(player);

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("回復力アップ");
            });

        //爆弾の爆発範囲強化イベント
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => {
                //プレイヤーの爆弾の爆発範囲強化
                player.GetPowerUpItems.PowerUpBombRange(player);

                //効果音を鳴らす
                SoundManager.uniqueInstance.Play("爆発範囲アップ");
            });

        //爆弾追加イベント
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () =>{
                //新しい爆弾を使用可能にする
                player.EnableNewBomb();

                //効果音を鳴らす
            });
    }
}
