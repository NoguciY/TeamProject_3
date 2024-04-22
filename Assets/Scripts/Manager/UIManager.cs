using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //���x���A�b�v�p�l��
    [SerializeField]
    private GameObject levelUpPanel;

    //�o�ߎ��ԃe�L�X�g�R���|�[�l���g
    [SerializeField]
    private Timer timer;

    //�̗̓Q�[�W�R���|�[�l���g
    [SerializeField]
    private LifeGauge lifeGauge;

    //�o���l�Q�[�W�R���|�[�l���g
    [SerializeField]
    private ExpGauge experienceValueGauge;

    //�{�^���Ǘ��R���|�[�l���g
    [SerializeField]
    private ButtonManager buttonManager;

    //���ŏ�������ŏ���
    //private bool isClear = false;

    //�Q�b�^�[
    public GameObject GetGameOverPanel { get { return gameOverPanel; } }
    public GameObject GetLevelUpPanel { get { return levelUpPanel; } }
    public LifeGauge GetLifeGauge { get { return lifeGauge; } }
    public ExpGauge GetExperienceValueGauge { get {  return experienceValueGauge; } }
    public ButtonManager GetButtonManager { get {  return buttonManager; } }


    //�Q�[���N���A�p�l����\������
    private void GameClear()
    {
        Time.timeScale = 0;
        gameClearPanel.SetActive(true);
    }


    //�p�l����\�������邩
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    //�{�^����I������

    private void Start()
    {
        //Invoke("GameClear", endingTime);
        //�{�^���̏�����
        buttonManager.InitButton(this);
    }


    private void Update()
    {
        //�o�ߎ��ԃe�L�X�g�̍X�V
        timer.CountTimer(gameManager.GetDeltaTimeInMain);

        //�N���A�̕\��(���ŏ������̂Ō�ŏ�������)
        //if (isClear != gameManager.GetIsGameClear)
        //{
        //    isClear = true;
        //    GameClear();
        //}
    }
}
