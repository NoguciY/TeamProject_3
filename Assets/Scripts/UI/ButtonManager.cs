//using System;
using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�{�^�����܂Ƃ߂ĊǗ�����

public class ButtonManager : MonoBehaviour
{
    //�Q�[���I�[�o�[��ʂ̃R���e�B�j���[�{�^��
    [SerializeField]
    private Button gameOverContinueButton;

    //�����{�^���R���|�[�l���g
    public PowerUpButton powerUpButton;


    //�Q�b�^�[
    public Button GetGameOverContinueButton => gameOverContinueButton;

    //�{�^���̏�����(�C�x���g�̓o�^)
    public void Initialize(UIManager uIManager)
    {
        //���O�Ƀp���[�A�b�v�{�^���Ƀ|�[�Y����߂�֐���
        //���x���A�b�v�p�l�����\���ɂ���֐���o�^����
        foreach (var button in powerUpButton.powerUpButtons)
        {
            button.onClick.AddListener(() => Time.timeScale = 1);
            button.onClick.AddListener(() => uIManager.ShoulShowPanel(uIManager.GetLevelUpPanel, false));
        }

        //�C�x���g�ɍ����������{�^���̉摜��ݒ肷��
        powerUpButton.SetPowerUpButtoSprites();

        //���O�ɔ��e�ǉ��{�^���Ƀ|�[�Y����߂�֐���
        //���x���A�b�v�p�l�����\���ɂ���֐���o�^����
        powerUpButton.addNewBombButton.onClick.AddListener(() => Time.timeScale = 1);
        powerUpButton.addNewBombButton.onClick.AddListener(
            () => uIManager.ShoulShowPanel(uIManager.GetLevelUpBombPanel, false));
    }
}
