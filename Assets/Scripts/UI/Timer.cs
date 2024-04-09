using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    //経過時間テキスト
    [SerializeField]
    private TextMeshProUGUI timerText;
    
    //分数
    private int minute;
    //秒数
    private int seconds;
    //前回の秒数
    private int oldSeconds;

    //テキストに経過時間を反映する
    public void CountTimer(float time)
    {
        //時間計測
        seconds = (int)time;

        int minute = seconds / 60;

        if (seconds != oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((seconds % 60)).ToString("00");
        }
        oldSeconds = seconds;
    }
}