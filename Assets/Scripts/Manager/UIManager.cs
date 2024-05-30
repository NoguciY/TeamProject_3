using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //メインゲームパネル
    [SerializeField]
    private GameObject mainPanel;

    //ゲームオーバーパネル
    [SerializeField]
    private GameObject gameOverPanel;

    //レベルアップパネル
    [SerializeField]
    private GameObject levelUpPanel;

    //レベルアップパネル(爆弾追加用)
    [SerializeField]
    private GameObject levelUpBombPanel;

    //経過時間テキストコンポーネント
    [SerializeField]
    private Timer timer;
    
    //現在レベルテキストコンポーネント
    [SerializeField]
    private PlayerLevel level;

    //体力ゲージコンポーネント
    [SerializeField]
    private LifeGauge lifeGauge;

    //経験値ゲージコンポーネント
    [SerializeField]
    private ExpGauge experienceValueGauge;

    //クールタイムゲージコンポーネント
    [SerializeField]
    private CoolTimeGauge coolTimeGauge;

    //ボタン管理コンポーネント
    [SerializeField]
    private ButtonManager buttonManager;

    //ゲッター
    public GameObject GetGameOverPanel => gameOverPanel;
    public GameObject GetLevelUpPanel => levelUpPanel;
    public GameObject GetLevelUpBombPanel => levelUpBombPanel;
    public LifeGauge GetLifeGauge => lifeGauge;
    public ExpGauge GetExperienceValueGauge => experienceValueGauge;
    public CoolTimeGauge GetCoolTimeGauge => coolTimeGauge;
    public ButtonManager GetButtonManager => buttonManager;


    //パネルを表示させるか
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    private void Start()
    {
        //ボタンの初期化
        buttonManager.Initialize(this);
    }


    private void Update()
    {
        //経過時間テキストの更新
        timer.CountTimer(GameManager.Instance.GetDeltaTimeInMain);

        //現在のレベルテキストの更新
        level.CountLevel(GameManager.Instance.playerLevel);

        //クールタイムゲージの更新を増えた爆弾分行う
        for(int i = 0; i < Enum.GetNames(typeof(Utilities.AddedBombType)).Length; i++)
        coolTimeGauge.FillGauge(i);
    }
}
