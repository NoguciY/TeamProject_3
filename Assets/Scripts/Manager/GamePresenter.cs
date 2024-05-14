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

    //�T�E���h�}�l�[�W���[
    [SerializeField]
    private SoundManager soundManager;


    private void Start()
    {
        //�v���C���[�̃C�x���g��UI�}�l�[�W���[�̊֐���o�^����

        //�Q�[���I�[�o�[�C�x���g�Ɋ֐���o�^����
        //�̗̓Q�[�W��0�ɂ���
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.GetLifeGauge.GameOverLifeGauge());
        //�Q�[���I�[�o�[�p�l����\������
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true));
        //�Q�[�����|�[�Y����
        player.GetPlayerEvent .gameOverEvent.AddListener(
            () => Time.timeScale = 0);
        //�R���e�B�j���[�{�^����I������
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () => uiManager.GetButtonManager.GetGameOverContinueButton.Select());

        //�o���l�擾���Ɍo���l�Q�[�W���X�V����֐���o�^
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //�_���[�W���󂯂��ۂɑ̗̓Q�[�W�̍X�V���s���֐���o�^
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //�Q�[���J�n���ɑ̗̓Q�[�W�̏��������s���֐���o�^
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //���x���A�b�v�C�x���g�Ɋ֐���o�^����
        //���x���A�b�v�p�l����\������
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //�Q�[�����|�[�Y����֐���o�^
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);
        //���x���A�b�v���ɋ����{�^���ɋ����C�x���g��o�^����֐���o�^
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents());
        //���x���A�b�v���Ɍ��ʉ���炷�֐���o�^
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => soundManager.Play("���x���A�b�v"));

        //�V�������e��ǉ����鎞�Ƀ��x���A�b�v�p�l��(���e�ǉ��p)��\������֐���o�^
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true));
        //�V�������e��ǉ����鎞�ɃQ�[�����|�[�Y����֐���o�^
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => Time.timeScale = 0);
        //�V�������e��ǉ����鎞�ɔ��e�ǉ��{�^���ɔ��e�ǉ��C�x���g��o�^����֐���o�^
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter));
        //�V�������e��ǉ����鎞�Ɍ��ʉ���炷�֐���o�^
        player.GetPlayerEvent.AddNewBombEvent.AddListener(
            () => soundManager.Play("���x���A�b�v"));


        //UI�}�l�[�W���[�̃C�x���g�Ƀv���C���[�̋����֐���o�^

        //�����C�x���g�Ƀv���C���[�̍ő�̗͋����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpMaxLife(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => soundManager.Play("�ő�̗̓A�b�v"));

        //�����C�x���g�Ƀv���C���[�̈ړ����x�����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpSpeed(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => soundManager.Play("�ړ����x�A�b�v"));

        //�����C�x���g�Ƀv���C���[�̉���͈͋����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpCollectionRangeRate(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => soundManager.Play("����͈̓A�b�v"));

        //�����C�x���g�Ƀv���C���[�̖h��͋����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpDifence(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => soundManager.Play("�h��̓A�b�v"));

        //�����C�x���g�Ƀv���C���[�̉񕜗͋����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpResilience(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => soundManager.Play("�񕜗̓A�b�v"));

        //�����C�x���g�Ƀv���C���[�̔��e�̔����͈͋����֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpBombRange(player));
        //�����C�x���g�Ɍ��ʉ���炷�֐���o�^
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => soundManager.Play("�����͈̓A�b�v"));

        //���e�ǉ��C�x���g�ɐV�������e���g�p�\�ɂ���֐���o�^
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () => player.NewBombEnabled());
    }
}
