using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�{�^�����܂Ƃ߂ĊǗ�����

public class ButtonManager : MonoBehaviour
{
    //�������ڃ{�^���̐�
    private const int powerUpButtonNum = 3;

    //�p���[�A�b�v�{�^��
    [SerializeField]
    private  List<Button> powerUpButtons = new List<Button>();


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
    public List<UnityEvent> powerUpEvents = new List<UnityEvent>();
    //�e�����֐��̃��X�i�[��ێ�����z��
    private List<UnityAction> powerUpListeners = new List<UnityAction>();
    //private UnityAction[] powerUpListeners = new UnityAction[powerUpButtonNum];

    //�������ڃC�x���g���X�g
    private List<UnityEvent> events = new List<UnityEvent>();

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

    //���x���A�b�v�C�x���g�ŌĂ΂��
    //�����{�^���ɋ����֐���o�^����
    public void RegisterPowerUpItemEvents()
    {
        SetPowerUpItemsList();
        GetRandomPowerUpItemEvents();

        for(int i = 0; i < powerUpButtonNum; i++)
        {
            //�O��{�^���ɓo�^�������X�i�[(�����֐��̂�)���폜����
            if (powerUpListeners[i] != null)
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);

            //�{�^���ɓo�^���郊�X�i�[��ێ�����
            powerUpListeners[i] = () => powerUpEvents[i].Invoke();

            //�{�^���Ƀ��X�i�[��o�^����
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);
        }
    }

    //���X�g�ɋ����\�ȋ������ڂ�ǉ�����
    private void SetPowerUpItemsList()
    {
        events.Clear();

        //�������ڃC�x���g���X�g�ɋ����\�ȋ������ڂ��i�[
        //�����̋������ڂ�ǉ�����
        events.Add(powerUpMaxLifeEvent);
        events.Add(powerUpSpeedEvent);
        events.Add(powerUpCollectionRangeRateEvent);
        events.Add(powerUpDifenceEvent);
        events.Add(powerUpResilienceEvent);
        events.Add(powerUpBombRangeEvent);

        //�ǉ�����鋭�����ڂ�ŏI�������č폜����鋭�����ڂ����邽��
        //�����Ń��X�g�ɒǉ���폜�����鏈�����L�q����
    }

    //���X�g���烉���_���ɋ������ڃ{�^���̐����A�z��Ɋi�[
    private void GetRandomPowerUpItemEvents()
    {
        //���X�g�̐�
        int listNum = events.Count;

        //1��̃����_���Ȓl(�����l�ɂ́A�����_���ł͑I�΂�Ȃ��l�ł��郊�X�g�̐����i�[)
        int beforeRndFirst = listNum;
        //2��̃����_���Ȓl
        int beforeRndSecond = listNum;

        int i = 0;

        //�z��̐������X�g�Ɋi�[����
        while(i < powerUpButtonNum)
        {
            //0���烊�X�g�̐�-1�̐������烉���_���Ȓl���i�[
            int rnd = UnityEngine.Random.Range(0, listNum);

            //�����C�x���g���z��Ɋi�[����Ȃ��悤�ɂ���
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //Debug.Log($"rnd:{rnd}");
                //�z��ɒǉ�����
                powerUpEvents[i] = events[rnd];
                Debug.Log($"powerUpEvents[{i}]��events[{rnd}]��ǉ�");

                //1��ڂ̃����_���Ȓl���i�[
                if (i == 0)
                    beforeRndFirst = rnd;
                //2��ڂ̃����_���Ȓl���i�[
                else if(i == 1)
                    beforeRndSecond = rnd;
                i++;
            }
        }
    }
}
