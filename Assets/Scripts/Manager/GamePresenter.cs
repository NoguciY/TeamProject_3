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
        //イベントにゲームオーバーパネルを表示する関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true));
        //ゲームをポーズする関数を登録
        player.playerEvent.gameOverEvent.AddListener(
            () => Time.timeScale = 0);

        //イベントに経験値ゲージを更新する関数を登録
        player.playerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //イベントに体力ゲージの更新を行う関数を登録
        player.playerEvent.damageEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(-damage));

        //イベントに体力ゲージの初期化を行う関数を登録
        player.playerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //イベントにレベルアップパネルを表示する関数を登録
        player.playerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //ゲームをポーズする関数を登録
        player.playerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);


        //UIマネジャーのイベントにプレイヤーの強化関数を登録する
        
    }
}
