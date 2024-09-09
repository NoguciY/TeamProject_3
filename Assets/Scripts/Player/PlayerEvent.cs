using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    //ゲームオーバーの場合に実行するイベント
    [NonSerialized]
    //public UnityEvent gameOverEvent;
    public UnityEvent gameOverEvent = new UnityEvent();

    //経験値を得た場合に実行するイベント
    public class ExperienceValueEvent : UnityEvent<float, float> { }
    //public ExpEvent expEvent;
    public ExperienceValueEvent experienceValueEvent = new ExperienceValueEvent();

    //ダメージを受けた場合に実行するイベント
    public class AddLifeEvent : UnityEvent<float> { }
    //public AddLifeEvent addLifeEvent;
    public AddLifeEvent addLifeEvent = new AddLifeEvent();

    //体力の最大値を渡す場合に実行するイベント
    public class GetMaxLifeEvent : UnityEvent<float> { }
    //public GetMaxLifeEvent getMaxLifeEvent;
    public GetMaxLifeEvent getMaxLifeEvent = new GetMaxLifeEvent();

    //経験値の最大値を渡す場合に実行するイベント
    public class GetMaxExperienceValueEvent : UnityEvent<float, float> { }
    public GetMaxExperienceValueEvent getMaxExperienceValueEvent = new GetMaxExperienceValueEvent();

    //レベルアップの場合に実行するイベント
    [NonSerialized]
    //public UnityEvent levelUpEvent;
    public UnityEvent levelUpEvent = new UnityEvent();

    //レベルアップで新しい爆弾を追加する場合に実行するイベント
    [NonSerialized]
    //public UnityEvent addNewBombEvent;
    public UnityEvent addNewBombEvent = new UnityEvent();

    //クールダウンイベント
    public class CoolDownEvent : UnityEvent<int, float> { }
    public CoolDownEvent coolDownEvent = new CoolDownEvent();

    //private void Awake()
    //{
    //    gameOverEvent = new UnityEvent();
    //    expEvent = new ExpEvent();
    //    addLifeEvent = new AddLifeEvent();
    //    getMaxLifeEvent = new GetMaxLifeEvent();
    //    levelUpEvent = new UnityEvent();
    //    addNewBombEvent = new UnityEvent();
    //}
}
