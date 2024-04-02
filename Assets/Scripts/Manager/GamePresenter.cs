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
        player.gameOverEvent.AddListener(() => uiManager.GameOver());
        player.experienceEvent.AddListener((currentExp, needExp) => uiManager.UpdateExperienceGauge(currentExp, needExp));
    }
}
