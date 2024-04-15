using System;
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


    //�ő�HP�����C�x���g
    [NonSerialized]
    public UnityEvent powerUpMaxLifeEvent = new UnityEvent();
    //�ړ����x�����C�x���g
    [NonSerialized]
    public UnityEvent powerUpSpeedEvent = new UnityEvent();
    //����͈͋����C�x���g
    [NonSerialized]
    public UnityEvent powerUpCollectionRangeRateEvent = new UnityEvent();
    //�h��͋����C�x���g
    [NonSerialized]
    public UnityEvent powerUpDifenceEvent = new UnityEvent();
    //�񕜗͋����C�x���g
    [NonSerialized]
    public UnityEvent powerUpResilienceEvent = new UnityEvent();
    //���e�U���͋����C�x���g
    [NonSerialized]
    public UnityEvent powerUpBombAttackEvent = new UnityEvent();
    //���e�͈̔͋����C�x���g
    [NonSerialized]
    public UnityEvent powerUpBombRangeEvent = new UnityEvent();

    //�e�����֐��̃��X�i�[��ێ�����ϐ�(UnityAction�̃C���X�^���X)
    //���X�i�[��ێ����Ă����Ȃ��ƍ폜�ł��Ȃ�����
    private UnityAction powerUpMaxLifeListener = null;
    private UnityAction powerUpSpeedListener = null;
    private UnityAction powerUpCollectionRangeRateListener = null;


    //�����C�x���g�z��
    [NonSerialized]
    public UnityEvent[] powerUpEvents = new UnityEvent[powerUpButtonNum];
    //�e�����֐��̃��X�i�[��ێ�����z��
    private UnityAction[] powerUpListeners = { null, null, null};

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

    //���x���A�b�v�C�x���g�ŌĂ΂��
    //�����{�^���ɋ����֐���o�^����
    public void RegisterPowerUpFunc()
    {
        //�O��{�^���ɓo�^�������X�i�[(�����֐��̂�)���폜����
        if (powerUpMaxLifeListener != null)
        {
            powerUpButtons[0].onClick.RemoveListener(powerUpMaxLifeListener);
            powerUpButtons[1].onClick.RemoveListener(powerUpSpeedListener);
            powerUpButtons[2].onClick.RemoveListener(powerUpCollectionRangeRateListener);
        }

        //�{�^���ɓo�^���郊�X�i�[��ێ�����
        powerUpMaxLifeListener = () => powerUpMaxLifeEvent.Invoke();
        powerUpSpeedListener = () => powerUpSpeedEvent.Invoke();
        powerUpCollectionRangeRateListener = () => powerUpCollectionRangeRateEvent.Invoke();

        //�{�^���Ƀ��X�i�[��o�^����
        powerUpButtons[0].onClick.AddListener(powerUpMaxLifeListener);
        powerUpButtons[1].onClick.AddListener(powerUpSpeedListener);
        powerUpButtons[2].onClick.AddListener(powerUpCollectionRangeRateListener);
    }

    //���X�g�ɋ����\�ȋ������ڂ�ǉ�����
    public void SetPowerUpItems()
    {
        //�������ڃC�x���g���X�g�ɋ����\�ȋ������ڂ��i�[
        List<UnityEvent> events = new List<UnityEvent>();
        //�����̋������ڂ�ǉ�����
        events.Add(powerUpMaxLifeEvent);
        events.Add(powerUpSpeedEvent);
        events.Add(powerUpCollectionRangeRateEvent);
        events.Add(powerUpDifenceEvent);
        events.Add(powerUpResilienceEvent);
        events.Add(powerUpBombRangeEvent);

        //�폜����

        
        
    }

    //���X�g���烉���_���ɋ������ڃ{�^���̐����A�z��Ɋi�[

}
