using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    //ゲームオーバーの場合に実行するイベント
    [NonSerialized]
    public UnityEvent gameOverEvent = new UnityEvent();

    //経験値を得た場合に実行するイベント
    public class ExpEvent : UnityEvent<int, int> { }
    public ExpEvent expEvent = new ExpEvent();

    //ダメージを受けた場合に実行するイベント
    public class AddLifeEvent : UnityEvent<float> { }
    public AddLifeEvent addLifeEvent = new AddLifeEvent();

    //体力の最大値を渡す場合に実行するイベント
    public class GetMaxLifeEvent : UnityEvent<float> { }
    public GetMaxLifeEvent getMaxLifeEvent = new GetMaxLifeEvent();

    //レベルアップの場合に実行するイベント
    [NonSerialized]
    public UnityEvent levelUpEvent = new UnityEvent();

    //レベルアップで新しい爆弾を追加する場合に実行するイベント
    [NonSerialized]
    public UnityEvent AddNewBombEvent = new UnityEvent();
}
