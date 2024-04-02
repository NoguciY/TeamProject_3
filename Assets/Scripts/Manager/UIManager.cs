using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    private Slider experienceGauge;

    private float oldSeconds;

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    //�Q�[���N���A�p�l����\������
    private void GameClear()
    {
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

    /// <summary>
    /// �o���l�Q�[�W���X�V����
    /// </summary>
    /// <param name="currentExp">���݂̌o���l</param>
    /// <param name="needExp">���x���A�b�v�ɕK�v�Ȍo���l</param>
    public void UpdateExperienceGauge(int currentExp, int needExp)
    {
        float value = 0;
        if (currentExp != 0)
            //int�^���m�̏��Z�ł́A���ʂ������ɂȂ邽�߁Aflaot�^�ɃL���X�g����
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
