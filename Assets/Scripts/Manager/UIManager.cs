using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //���C���Q�[���p�l��
    [SerializeField]
    private GameObject mainPanel;

    //�Q�[���I�[�o�[�p�l��
    [SerializeField]
    private GameObject gameOverPanel;

    public GameObject GetGameOverPanel => gameOverPanel;

    //���x���A�b�v�p�l��
    [SerializeField]
    private GameObject levelUpPanel;

    public GameObject GetLevelUpPanel => levelUpPanel;

    //���x���A�b�v�p�l��(���e�ǉ��p)
    [SerializeField]
    private GameObject levelUpBombPanel;

    public GameObject GetLevelUpBombPanel => levelUpBombPanel;

    //�o�ߎ��ԃe�L�X�g�R���|�[�l���g
    [SerializeField]
    private Timer timer;
    
    //���݃��x���e�L�X�g�R���|�[�l���g
    [SerializeField]
    private PlayerLevel level;

    //�̗̓Q�[�W�R���|�[�l���g
    [SerializeField]
    private LifeGauge lifeGauge;

    public LifeGauge GetLifeGauge => lifeGauge;

    //�o���l�Q�[�W�R���|�[�l���g
    [SerializeField]
    private ExperienceValueGauge experienceValueGauge;

    public ExperienceValueGauge GetExperienceValueGauge => experienceValueGauge;

    //�N�[���^�C���Q�[�W�R���|�[�l���g
    [SerializeField]
    private CoolTimeGauge coolTimeGauge;

    public CoolTimeGauge GetCoolTimeGauge => coolTimeGauge;

    //�{�^���Ǘ��R���|�[�l���g
    [SerializeField]
    private ButtonManager buttonManager;

    public ButtonManager GetButtonManager => buttonManager;

    //BombIcon�R���|�[�l���g
    [SerializeField]
    private GameObject[] bombIcons;

    private int bombIconCounter;

    
    private void Start()
    {
        //�{�^���̏�����
        buttonManager.Initialize(this);

        //�{���A�C�R���J�E���^�[�̏�����
        bombIconCounter = 0;
    }


    private void Update()
    {
        //�o�ߎ��ԃe�L�X�g�̍X�V
        timer.CountTimer(GameManager.Instance.GetDeltaTimeInMain);

        //���݂̃��x���e�L�X�g�̍X�V
        level.CountLevel(GameManager.Instance.playerLevel);

        //�N�[���^�C���Q�[�W�̍X�V�𑝂������e���s��
        for (int i = 0; i < Enum.GetNames(typeof(Utilities.AddedBombType)).Length; i++)
        {
            coolTimeGauge.FillGauge(i);
        }
    }

    /// <summary>
    /// �p�l����\�������邩
    /// </summary>
    /// <param name="panel">�\��������p�l��</param>
    /// <param name="shoudShow">true:�\������ / false:�\�����Ȃ�</param>
    public void ShoulShowPanel(GameObject panel, bool shoudShow)
    {
        panel.SetActive(shoudShow);
    }

    /// <summary>
    /// �摜(���e�A�C�R��)��\�������邩
    /// </summary>
    public void ShouwBombIcon()
    {
        bombIcons[bombIconCounter].SetActive(true);
        bombIconCounter++;
    }

}
