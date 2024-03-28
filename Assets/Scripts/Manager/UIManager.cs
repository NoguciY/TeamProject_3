using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //�Q�[���}�l�[�W���[
    [SerializeField]
    private GameManager gameManager;

    //���C���Q�[���p�l��
    [SerializeField]
    private GameObject mainPanel;

    //�Q�[�g�N���A�p�l��
    [SerializeField]
    private GameObject gameClearPanel;

    //�Q�[���I�[�o�[�p�l��
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField, Header("�Q�[���I������")]
    private float endingTime;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float oldSeconds;

    public void GameOver()
    {
        timerText.transform.position = new Vector3(1000.0f, 600.0f, 0.0f);
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    //�Q�[���N���A�p�l����\������
    private void GameClear()
    {
        timerText.transform.position = new Vector3(1000.0f,600.0f, 0.0f);
        Time.timeScale = 0;
        gameClearPanel.SetActive(true);
    }

    //���Ԃ�������V�[���J��(��)


    //�e�L�X�g�Ɍo�ߎ��Ԃ𔽉f����
    private void CountTimer(GameManager gameManager)
    {
        //�@Time.time�ł̎��Ԍv��
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
