using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //メインゲームパネル
    [SerializeField]
    private GameObject mainPanel;

    //ゲームオーバーパネル
    [SerializeField]
    private GameObject gameOverPanel;

    public GameObject GetGameOverPanel => gameOverPanel;

    //レベルアップパネル
    [SerializeField]
    private GameObject levelUpPanel;

    public GameObject GetLevelUpPanel => levelUpPanel;

    //レベルアップパネル(爆弾追加用)
    [SerializeField]
    private GameObject levelUpBombPanel;

    public GameObject GetLevelUpBombPanel => levelUpBombPanel;

    //経過時間テキストコンポーネント
    [SerializeField]
    private Timer timer;
    
    //現在レベルテキストコンポーネント
    [SerializeField]
    private PlayerLevel level;

    //体力ゲージコンポーネント
    [SerializeField]
    private LifeGauge lifeGauge;

    public LifeGauge GetLifeGauge => lifeGauge;

    //経験値ゲージコンポーネント
    [SerializeField]
    private ExperienceValueGauge experienceValueGauge;

    public ExperienceValueGauge GetExperienceValueGauge => experienceValueGauge;

    //クールタイムゲージコンポーネント
    [SerializeField]
    private CoolTimeGauge coolTimeGauge;

    public CoolTimeGauge GetCoolTimeGauge => coolTimeGauge;

    //ボタン管理コンポーネント
    [SerializeField]
    private ButtonManager buttonManager;

    public ButtonManager GetButtonManager => buttonManager;

    //BombIconコンポーネント
    [SerializeField]
    private GameObject[] bombIcons;

    private int bombIconCounter;

    //オプションコンポーネント
    [SerializeField]
    private OptionManager optionManager;

    [SerializeField]
    private SoundVolumeSlider soundVolumeSlider;

    private void Start()
    {
        //ボタンの初期化
        buttonManager.Initialize(this);

        //ボムアイコンカウンターの初期化
        bombIconCounter = 0;

        optionManager.Initialize();

        soundVolumeSlider.Initialize();
    }


    private void Update()
    {
        //経過時間テキストの更新
        timer.CountTimer(GameManager.Instance.GetDeltaTimeInMain);

        //現在のレベルテキストの更新
        level.CountLevel(GameManager.Instance.playerLevel);

        //クールタイムゲージの更新を増えた爆弾分行う
        for (int i = 0; i < coolTimeGauge.GetGeugeNum; i++)
        {
            coolTimeGauge.FillGauge(i);
        }

        //オプションの操作
        optionManager.ControllOption();
    }

    /// <summary>
    /// パネルを表示させるか
    /// </summary>
    /// <param name="panel">表示させるパネル</param>
    /// <param name="shoudShow">true:表示する / false:表示しない</param>
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    /// <summary>
    /// 画像(爆弾アイコン)を表示させるか
    /// </summary>
    public void ShouwBombIcon()
    {
        bombIcons[bombIconCounter].SetActive(true);
        bombIconCounter++;
    }

}
