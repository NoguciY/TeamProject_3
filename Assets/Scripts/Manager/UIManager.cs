using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float oldSeconds;

    public void GameOver()
    {
        //Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        Invoke("ChangeScene", 5.0f);
    }

    //ゲームクリアパネルを表示する
    private void GameClear()
    {
        //Time.timeScale = 0;
        gameClearPanel.SetActive(true);
        Invoke("ChangeScene", 5.0f);
    }

    //時間が来たらシーン遷移(仮)
    private void ChangeScene()
    {
        SceneManager.LoadScene("StartScene");
    }

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

    private void Start()
    {
        Invoke("GameClear", endingTime);
    }

    private void Update()
    {
        CountTimer(gameManager);
    }
}
