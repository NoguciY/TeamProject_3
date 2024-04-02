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
        player.gameOverEvent.AddListener(() => uiManager.GameOver());
        player.experienceEvent.AddListener((currentExp, needExp) => uiManager.UpdateExperienceGauge(currentExp, needExp));
    }
}
