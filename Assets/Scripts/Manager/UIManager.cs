using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField, Header("ゲーム終了時間")]
    private float endingTime;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Slider experienceGauge;

    private float oldSeconds;

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    //ゲームクリアパネルを表示する
    private void GameClear()
    {
        Time.timeScale = 0;
        gameClearPanel.SetActive(true);
    }

    //時間が来たらシーン遷移(仮)


    //テキストに経過時間を反映する
    private void CountTimer(GameManager gameManager)
    {
        //　Time.timeでの時間計測
        float seconds = gameManager.GetDeltaTimeInMain;

        int minute = (int)seconds / 60;

        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)(seconds % 60)).ToString("00");
        }
        oldSeconds = seconds;
    }

    /// <summary>
    /// 経験値ゲージを更新する
    /// </summary>
    /// <param name="currentExp">現在の経験値</param>
    /// <param name="needExp">レベルアップに必要な経験値</param>
    public void UpdateExperienceGauge(int currentExp, int needExp)
    {
        float value = 0;
        if (currentExp != 0)
            //int型同士の除算では、結果が整数になるため、flaot型にキャストする
            value = (float)currentExp / (float)needExp;
        
        experienceGauge.value = value;
    }

    private void Start()
    {
        Invoke("GameClear", endingTime);
    }

    private void Update()
    {
        CountTimer(gameManager);
    }
}
