using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    //強化項目
    //enum PowerUpItems
    //{
    //    MaxLife,                //最大体力
    //    Speed,                  //移動速度
    //    PlantedBombCoolDown,    //設置型爆弾の使用速度
    //    PlantedBombRange,       //設置型爆弾の攻撃範囲
    //}

    //強化項目コンポーネント
    [SerializeField]
    private PowerUpItems powerUpItems;

    //強化項目ボタン
    int maxlife;

    //強化項目画像
    [SerializeField]
    private List<Image> poweUpItemImages;


    //強化項目画像と強化項目クラスのディクショナリ
    private Dictionary<Image, PowerUpItems> powerUpItemsDictionary;

    //強化項目の初期化
    public void InitPowerUpItems()
    {
        //powerUpItemsDictionary = new Dictionary<Image, PowerUpItems>()
        //{
        //    { poweUpItemImages, powerUpItems. },
        //};
    }


    //レベルアップ項目をランダムに選択
    //3つの整数をランダムで選ぶ(追加と削除を行うのでリストがいいか)
    //
}
