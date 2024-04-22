using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //ゲームマネージャー
    [SerializeField]
    private GameManager gameManager;

    //メインゲームパネル
    [SerializeField]
    private GameObject mainPanel;

    //ゲートクリアパネル
    [SerializeField]
    private GameObject gameClearPanel;

    //ゲームオーバーパネル
    [SerializeField]
    private GameObject gameOverPanel;

    //レベルアップパネル
    [SerializeField]
    private GameObject levelUpPanel;

    //経過時間テキストコンポーネント
    [SerializeField]
    private Timer timer;

    //体力ゲージコンポーネント
    [SerializeField]
    private LifeGauge lifeGauge;

    //経験値ゲージコンポーネント
    [SerializeField]
    private ExpGauge experienceValueGauge;

    //ボタン管理コンポーネント
    [SerializeField]
    private ButtonManager buttonManager;

    //仮で書いた後で消す
    //private bool isClear = false;

    //ゲッター
    public GameObject GetGameOverPanel { get { return gameOverPanel; } }
    public GameObject GetLevelUpPanel { get { return levelUpPanel; } }
    public LifeGauge GetLifeGauge { get { return lifeGauge; } }
    public ExpGauge GetExperienceValueGauge { get {  return experienceValueGauge; } }
    public ButtonManager GetButtonManager { get {  return buttonManager; } }


    //ゲームクリアパネルを表示する
    private void GameClear()
    {
        Time.timeScale = 0;
        gameClearPanel.SetActive(true);
    }


    //パネルを表示させるか
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    //ボタンを選択する

    private void Start()
    {
        //Invoke("GameClear", endingTime);
        //ボタンの初期化
        buttonManager.InitButton(this);
    }


    private void Update()
    {
        //経過時間テキストの更新
        timer.CountTimer(gameManager.GetDeltaTimeInMain);

        //クリアの表示(仮で書いたので後で書き直す)
        //if (isClear != gameManager.GetIsGameClear)
        //{
        //    isClear = true;
        //    GameClear();
        //}
    }
}
