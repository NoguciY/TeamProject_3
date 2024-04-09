using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    //�o�ߎ��ԃe�L�X�g
    [SerializeField]
    private TextMeshProUGUI timerText;
    
    //����
    private int minute;
    //�b��
    private int seconds;
    //�O��̕b��
    private int oldSeconds;

    //�e�L�X�g�Ɍo�ߎ��Ԃ𔽉f����
    public void CountTimer(float time)
    {
        //���Ԍv��
        seconds = (int)time;

        int minute = seconds / 60;

        if (seconds != oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((seconds % 60)).ToString("00");
        }
        oldSeconds = seconds;
    }
}