using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //���C���Q�[���p�l��
    [SerializeField]
    private GameObject mainPanel;

    //�Q�[���I�[�o�[�p�l��
    [SerializeField]
    private GameObject gameOverPanel;

    //���x���A�b�v�p�l��
    [SerializeField]
    private GameObject levelUpPanel;

    //���x���A�b�v�p�l��(���e�ǉ��p)
    [SerializeField]
    private GameObject levelUpBombPanel;

    //�o�ߎ��ԃe�L�X�g�R���|�[�l���g
    [SerializeField]
    private Timer timer;
    
    //���݃��x���e�L�X�g�R���|�[�l���g
    [SerializeField]
    private PlayerLevel level;

    //�̗̓Q�[�W�R���|�[�l���g
    [SerializeField]
    private LifeGauge lifeGauge;

    //�o���l�Q�[�W�R���|�[�l���g
    [SerializeField]
    private ExpGauge experienceValueGauge;

    //�N�[���^�C���Q�[�W�R���|�[�l���g
    [SerializeField]
    private CoolTimeGauge coolTimeGauge;

    //�{�^���Ǘ��R���|�[�l���g
    [SerializeField]
    private ButtonManager buttonManager;

    //�Q�b�^�[
    public GameObject GetGameOverPanel => gameOverPanel;
    public GameObject GetLevelUpPanel => levelUpPanel;
    public GameObject GetLevelUpBombPanel => levelUpBombPanel;
    public LifeGauge GetLifeGauge => lifeGauge;
    public ExpGauge GetExperienceValueGauge => experienceValueGauge;
    public CoolTimeGauge GetCoolTimeGauge => coolTimeGauge;
    public ButtonManager GetButtonManager => buttonManager;


    //�p�l����\�������邩
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    private void Start()
    {
        //�{�^���̏�����
        buttonManager.Initialize(this);
    }


    private void Update()
    {
        //�o�ߎ��ԃe�L�X�g�̍X�V
        timer.CountTimer(GameManager.Instance.GetDeltaTimeInMain);

        //���݂̃��x���e�L�X�g�̍X�V
        level.CountLevel(GameManager.Instance.playerLevel);

        //�N�[���^�C���Q�[�W�̍X�V�𑝂������e���s��
        for(int i = 0; i < Enum.GetNames(typeof(Utilities.AddedBombType)).Length; i++)
        coolTimeGauge.FillGauge(i);
    }
}
