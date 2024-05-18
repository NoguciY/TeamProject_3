using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MVP�p�^�[���̃v���[���^�[
//UI�ƃv���C���[�̃C�x���g�ɓo�^����
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


    private void Awake()
    {
        //�v���C���[�̃C�x���g��UI�}�l�[�W���[�ƃT�E���h�}�l�[�W���[�̊֐���o�^����

        //�Q�[���I�[�o�[�C�x���g
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

        //�o���l�擾���Ɍo���l�Q�[�W���X�V����
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));
        
        //�_���[�W���󂯂��ۂɑ̗̓Q�[�W�̍X�V����
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //�Q�[���J�n���ɑ̗̓Q�[�W�̏���������
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //���x���A�b�v�C�x���g
        //���x���A�b�v�p�l����\������
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true));
        //�Q�[�����|�[�Y����
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => Time.timeScale = 0);
        //�����{�^���ɋ����C�x���g��o�^����
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents());
        //���x���A�b�v���Ɍ��ʉ���炷
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => soundManager.Play("���x���A�b�v"));

        //���e�ǉ��p���x���A�b�v�C�x���g
        //���x���A�b�v�p�l��(���e�ǉ��p)��\������
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true));
        //�Q�[�����|�[�Y����
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => Time.timeScale = 0);
        //���e�ǉ��{�^���ɔ��e�ǉ��C�x���g��o�^����
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter));
        //���ʉ���炷
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () => soundManager.Play("���x���A�b�v"));


        //UI�}�l�[�W���[�̃C�x���g�Ƀv���C���[�ƃT�E���h�}�l�[�W���[�̊֐���o�^����

        //�����C�x���g
        //�v���C���[�̍ő�̗͋���
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpMaxLife(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener(
            () => soundManager.Play("�ő�̗̓A�b�v"));

        //�v���C���[�̈ړ����x����
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpSpeed(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () => soundManager.Play("�ړ����x�A�b�v"));

        //�v���C���[�̉���͈͋���
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpCollectionRangeRate(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => soundManager.Play("����͈̓A�b�v"));

        //�v���C���[�̖h��͋���
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpDifence(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpDifenceEvent.AddListener(
            () => soundManager.Play("�h��̓A�b�v"));

        //�v���C���[�̉񕜗͋���
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpResilience(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () => soundManager.Play("�񕜗̓A�b�v"));

        //�v���C���[�̔��e�̔����͈͋���
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => player.GetPowerUpItems.PowerUpBombRange(player));
        //���ʉ���炷
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => soundManager.Play("�����͈̓A�b�v"));

        //���e�ǉ��C�x���g
        //�V�������e���g�p�\�ɂ���
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () => player.EnableNewBomb());
    }
}
