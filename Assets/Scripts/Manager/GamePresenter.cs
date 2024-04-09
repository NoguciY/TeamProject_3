using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MVP�p�^�[�����g���ăQ�[���I�[�o�[�����
//���ꂾ�����ƁA���̃X�N���v�g���������Ƀv���C���[��Player�R���|�[�l���g�̃C���X�y�N�^�[��
//UI�}�l�[�W���[�̊֐���o�^����΂����Ǝv��
//�C�x���g�̈��������I�Ȉ����Ȃ�g�����l�����邾�낤
//�v���C���[��UI�}�l�[�W���[�����݂���m��Ȃ��Ă��o�^�ł���̂́A�ύX��C�����ȒP�����炢���Ǝv��

public class GamePresenter : MonoBehaviour
{
    //�v���C���[
    [SerializeField]
    private Player player;

    //UI�}�l�[�W���[
    [SerializeField]
    private UIManager uiManager;


    private void Awake()
    {
        //�v���C���[�̃C�x���g��UI�}�l�[�W���[�̊֐���o�^����
        //�C�x���g�ɃQ�[���I�[�o�[�p�l����\������֐���o�^
        player.playerEvent.gameOverEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true));
        //�Q�[�����|�[�Y����֐���o�^
        player.playerEvent.gameOverEvent.AddListener(
            () => Time.timeScale = 0);

        //�C�x���g�Ɍo���l�Q�[�W���X�V����֐���o�^
        player.playerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //�C�x���g�ɑ̗̓Q�[�W�̍X�V���s���֐���o�^
        player.playerEvent.damageEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(-damage));

        //�C�x���g�ɑ̗̓Q�[�W�̏��������s���֐���o�^
        player.playerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //�C�x���g�Ƀ��x���A�b�v�p�l����\������֐���o�^
        player.playerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //�Q�[�����|�[�Y����֐���o�^
        player.playerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);


        //UI�}�l�W���[�̃C�x���g�Ƀv���C���[�̋����֐���o�^����
        
    }
}
