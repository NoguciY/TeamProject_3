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

    private void Awake()
    {
        //�v���C���[�̃C�x���g�Ɋ֐���o�^����

        //�ő�̗͎擾�C�x���g
        //�̗̓Q�[�W�̏���������
        player.GetPlayerEvent.getMaxLifeEvent.AddListener(
            (maxLife) => uiManager.GetLifeGauge.InitializeGauge(maxLife));

        //�o���l�擾�C�x���g
        //�o���l�Q�[�W���X�V����
        player.GetPlayerEvent.expEvent.AddListener(
            (currentExp, needExp) => uiManager.GetExperienceValueGauge.UpdateExpGauge(currentExp, needExp));

        //��_���[�W�C�x���g
        //�̗̓Q�[�W�̍X�V����
        player.GetPlayerEvent.addLifeEvent.AddListener(
            (damage) => uiManager.GetLifeGauge.UpdateGauge(damage));

        //�Q�[���I�[�o�[�C�x���g
        player.GetPlayerEvent.gameOverEvent.AddListener(
            () =>{
                //�̗̓Q�[�W��0�ɂ���
                uiManager.GetLifeGauge.GameOverLifeGauge();

                //�Q�[���I�[�o�[�p�l����\������
                uiManager.ShoulShowPanel(uiManager.GetGameOverPanel, true);

                //�Q�[�����|�[�Y����
                Time.timeScale = 0;

                //�R���e�B�j���[�{�^����I������
                uiManager.GetButtonManager.GetGameOverContinueButton.Select();

                //�v���C���[�̍ŏI���x�����擾����
                GameManager.Instance.playerLevel = player.GetPlayerLevelUp.GetLevel;

                //�V�[���^�C�v��ύX����
                GameManager.Instance.ChangeSceneType(SceneType.GameOver);

                //BGM�𗬂�
                SoundManager.uniqueInstance.PlayBgm("�Q�[���I�[�o�[");
            });

        //���x���A�b�v�C�x���g
        player.GetPlayerEvent.levelUpEvent.AddListener(
            () => {
                //���x���A�b�v�p�l����\������
                uiManager.ShoulShowPanel(uiManager.GetLevelUpPanel, true);

                //�Q�[�����|�[�Y����
                Time.timeScale = 0;

                //�����{�^���ɋ����C�x���g��o�^����
                uiManager.GetButtonManager.powerUpButton.RegisterPowerUpItemEvents(player);

                //���x���A�b�v���Ɍ��ʉ���炷
                SoundManager.uniqueInstance.Play("���x���A�b�v");

                //�Q�[���}�l�[�W���[�ɕۑ����Ă���v���C���[���x�����C���N�������g
                GameManager.Instance.playerLevel++;
            });

        //���e�ǉ��p���x���A�b�v�C�x���g
        player.GetPlayerEvent.addNewBombEvent.AddListener(
            () =>{
                //���x���A�b�v�p�l��(���e�ǉ��p)��\������
                uiManager.ShoulShowPanel(uiManager.GetLevelUpBombPanel, true);

                //�Q�[�����|�[�Y����
                Time.timeScale = 0;

                //���e�ǉ��{�^���ɔ��e�ǉ��C�x���g��o�^����
                uiManager.GetButtonManager.powerUpButton.RegisterAddNewBombEvent(player.GetNewBombCounter);

                //���e�A�C�R���̕\��
                uiManager.ShouwBombIcon();

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("���x���A�b�v");

                //�Q�[���}�l�[�W���[�ɕۑ����Ă���v���C���[���x�����C���N�������g
                GameManager.Instance.playerLevel++;
            });

        //�N�[���_�E���C�x���g
        player.GetPlayerEvent.coolDownEvent.AddListener(
            (index, coolTime) => {
                //�g�p�������e�̃N�[���^�C���Q�[�W�𖞂����n�߂�
                uiManager.GetCoolTimeGauge.StartFillingCoolTimeGauge(index, coolTime);
            });

        //UI�}�l�[�W���[�̃C�x���g�Ɋ֐���o�^����

        //�ő�̗͋����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpMaxLifeEvent.AddListener( 
            () => {
                //�v���C���[�̍ő�̗͋���
                player.GetPowerUpItems.PowerUpMaxLife(player);

                //�̗̓Q�[�W�̍ő�l���X�V����
                uiManager.GetLifeGauge.SetMaxValue = player.maxLife;

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("�ő�̗̓A�b�v");
            });

        //�ړ����x�����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpSpeedEvent.AddListener(
            () =>{
                //�v���C���[�̈ړ����x����
                player.GetPowerUpItems.PowerUpSpeed(player);

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("�ړ����x�A�b�v");
            });

        //����͈͋����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpCollectionRangeRateEvent.AddListener(
            () => {
                //�v���C���[�̉���͈͋���
                player.GetPowerUpItems.PowerUpCollectionRangeRate(player);

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("����͈̓A�b�v");
            });
        
        //�h��͋����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpDifenseEvent.AddListener(
            () =>{
                //�v���C���[�̖h��͋���
                player.GetPowerUpItems.PowerUpDifence(player);

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("�h��̓A�b�v");
            });

        //�񕜗͋����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpResilienceEvent.AddListener(
            () =>{
                //�v���C���[�̉񕜗͋���
                player.GetPowerUpItems.PowerUpResilience(player);

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("�񕜗̓A�b�v");
            });

        //���e�̔����͈͋����C�x���g
        uiManager.GetButtonManager.powerUpButton.powerUpBombRangeEvent.AddListener(
            () => {
                //�v���C���[�̔��e�̔����͈͋���
                player.GetPowerUpItems.PowerUpBombRange(player);

                //���ʉ���炷
                SoundManager.uniqueInstance.Play("�����͈̓A�b�v");
            });

        //���e�ǉ��C�x���g
        uiManager.GetButtonManager.powerUpButton.addNewBombEvent.AddListener(
            () =>{
                //�V�������e���g�p�\�ɂ���
                player.EnableNewBomb();

                //���ʉ���炷
            });
    }
}
