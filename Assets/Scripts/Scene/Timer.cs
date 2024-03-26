using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{

    private TextMeshProUGUI timerText;
    private int minute;
    private float seconds;
    private float oldSeconds;
    //　最初の時間
    private float startTime;
    [SerializeField] GameObject GameClearObject;

    void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        oldSeconds = 0;
        startTime = Time.time;

        GameClearObject.SetActive(false);
        Invoke("GameClear", 180.0f);
    }

    void Update()
    {

        //　Time.timeでの時間計測
        seconds = Time.time - startTime;

        minute = (int)seconds / 60;

        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)(seconds % 60)).ToString("00");
        }
        oldSeconds = seconds;
    }

    void GameClear()
    {
        GameClearObject.SetActive(true);
        Invoke("ChangeScene", 10.0f);
    }

    //時間が来たらシーン遷移(仮)
     void ChangeScene()
    {
      SceneManager.LoadScene("StartScene");
        
    }

}