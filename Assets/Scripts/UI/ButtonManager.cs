using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�{�^�����܂Ƃ߂ĊǗ�����

public class ButtonManager : MonoBehaviour
{
    private const int powerUpButtonNum = 3;

    //�p���[�A�b�v�{�^��
    [SerializeField]
    private  Button[] powerUpButtons = new Button[powerUpButtonNum];

    //�����C�x���g
    public class PowerUpMaxLifeEvent : UnityEvent<int> { }
    public PowerUpMaxLifeEvent powerUpMaxLifeEvent = new PowerUpMaxLifeEvent();

    //�{�^���̏�����(�C�x���g�̓o�^)
    public void InitButton(UIManager uIManager)
    {
        //�p���[�A�b�v�{�^���Ƀ|�[�Y����߂�֐���
        //���x���A�b�v�p�l�����\���ɂ���֐���o�^����
        foreach (var button in powerUpButtons)
        {
            button.onClick.AddListener(() => Time.timeScale = 1);
            button.onClick.AddListener(() => uIManager.ShoulShowPanel(uIManager.GetLevelUpPanel, false));
        }
    }


    //�����{�^���ɋ����֐���o�^����
    public void RegisterPowerUpFunc()
    {
        powerUpButtons[0].onClick.AddListener(() => powerUpMaxLifeEvent.Invoke(2) );
    }

    //�����_���ŋ������ڂ����߂�

}
