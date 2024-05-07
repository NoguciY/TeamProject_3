using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    //�������ڃ{�^���̐�
    private const int powerUpButtonNum = 3;

    //�p���[�A�b�v�{�^��
    public Button[] powerUpButtons = new Button[powerUpButtonNum];

    //���e�ǉ��{�^��
    public Button addNewBombButton;

    //�����C�x���g
    public class PowerUpEvent : UnityEvent
    {
        //�C�x���g�ɍ����������{�^���摜
        public Sprite powerUpButtonSprite; 
    }
    //�ő�HP�����C�x���g
    public PowerUpEvent powerUpMaxLifeEvent = new PowerUpEvent { };

    //�ړ����x�����C�x���g
    public PowerUpEvent powerUpSpeedEvent = new PowerUpEvent { };
    
    //�h��͋����C�x���g
    public PowerUpEvent powerUpDifenceEvent = new PowerUpEvent { };
    
    //����͈͋����C�x���g
    public PowerUpEvent powerUpCollectionRangeRateEvent = new PowerUpEvent { };
    
    //�񕜗͋����C�x���g
    public PowerUpEvent powerUpResilienceEvent = new PowerUpEvent { };
    
    //���e�͈̔͋����C�x���g
    public PowerUpEvent powerUpBombRangeEvent = new PowerUpEvent { };
    
    //�����C�x���g�N���X���i�[���郊�X�g
    //private List<PowerUpEvent> powerUpMEventList = new List<PowerUpEvent>(powerUpButtonNum);
    
    //�����{�^���摜���X�g
    [SerializeField]
    private List<Sprite> powerUpButtonSpriteList;
    
    //�����{�^���ɓo�^���鋭���{�^���摜���i�[����z��
    private Sprite[] powerUpButtonsSprites = new Sprite[powerUpButtonNum];

    //�����{�^���ɓo�^���鋭���C�x���g���i�[����z��
    [NonSerialized]
    public UnityEvent[] powerUpEvents = new UnityEvent[powerUpButtonNum];

    //�����{�^���ɓo�^���鋭���C�x���g��ێ�����z��(���X�i�[)
    private UnityAction[] powerUpListeners = new UnityAction[powerUpButtonNum];

    //�����{�^���ɓo�^���鋭���C�x���g�̌�⃊�X�g
    private List<PowerUpEvent> powerUpEventList = new List<PowerUpEvent>(powerUpButtonNum);

    //���e�ǉ��{�^���摜
    [SerializeField]
    private Sprite[] addNewBombButtons;

    //���e�ǉ��{�^���ɓo�^����C�x���g
    [NonSerialized]
    public UnityEvent addNewBombEvent = new UnityEvent();

    //���e�ǉ��{�^���ɓo�^����C�x���g��ێ�����
    private UnityAction addNewBombListener = null;

    //�����C�x���g�N���X��sprite�ɋ����{�^���摜��ݒ�
    public void SetPowerUpButtoSprites()
    {
        powerUpMaxLifeEvent.powerUpButtonSprite = powerUpButtonSpriteList[0];
        powerUpSpeedEvent.powerUpButtonSprite = powerUpButtonSpriteList[1];
        powerUpDifenceEvent.powerUpButtonSprite = powerUpButtonSpriteList[2];
        powerUpCollectionRangeRateEvent.powerUpButtonSprite = powerUpButtonSpriteList[3];
        powerUpResilienceEvent.powerUpButtonSprite = powerUpButtonSpriteList[4];
        powerUpBombRangeEvent.powerUpButtonSprite = powerUpButtonSpriteList[5];
    }

    //���x���A�b�v�C�x���g�ŌĂ΂��
    //�����{�^���ɋ����֐���o�^����
    public void RegisterPowerUpItemEvents()
    {
        //���X�g�ɋ����\�ȋ����C�x���g��ǉ�����
        SetPowerUpItemsList();
        //���X�g���烉���_���ɋ������ڃ{�^���̐����A�z��Ɋi�[
        GetRandomPowerUpItemEvents();

        for (int i = 0; i < powerUpButtonNum; i++)
        {
            //�O��{�^���ɓo�^�������X�i�[(�����֐��̂�)���폜����
            if (powerUpListeners[i] != null)
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);

            //�����_���ň�������̃��X�i�[��o�^���邽�߂ɕK�v
            int index = i;

            //�{�^���ɓo�^���郊�X�i�[��ێ�����
            powerUpListeners[index] = () => powerUpEvents[index].Invoke();

            //�{�^���Ƀ��X�i�[��o�^����
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);

            //�{�^���̉摜��ύX
            powerUpButtons[i].image.sprite = powerUpButtonsSprites[i];
        }
    }

    //���X�g�ɋ����\�ȋ����C�x���g��ǉ�����
    private void SetPowerUpItemsList()
    {
        powerUpEventList.Clear();

        //�������ڃC�x���g���X�g�ɋ����\�ȋ������ڂ��i�[
        //�����̋������ڂ�ǉ�����
        powerUpEventList.Add(powerUpMaxLifeEvent);
        powerUpEventList.Add(powerUpSpeedEvent);
        powerUpEventList.Add(powerUpCollectionRangeRateEvent);
        powerUpEventList.Add(powerUpDifenceEvent);
        powerUpEventList.Add(powerUpResilienceEvent);
        powerUpEventList.Add(powerUpBombRangeEvent);

        //�ǉ�����鋭�����ڂ�ŏI�������č폜����鋭�����ڂ����邽��
        //�����Ń��X�g�ɒǉ���폜�����鏈�����L�q����
    }

    //�����C�x���g���X�g���烉���_���ɋ������ڃ{�^���̐������A�z��Ɋi�[
    private void GetRandomPowerUpItemEvents()
    {
        //���X�g�̐�
        int listNum = powerUpEventList.Count;

        //1��̃����_���Ȓl(�����l�ɂ́A�����_���ł͑I�΂�Ȃ��l�ł��郊�X�g�̐����i�[)
        int beforeRndFirst = listNum;
        //2��̃����_���Ȓl
        int beforeRndSecond = listNum;

        int i = 0;

        while(i < powerUpButtonNum)
        {
            //0���烊�X�g�̐�-1�̐������烉���_���Ȓl���i�[
            int rnd = UnityEngine.Random.Range(0, listNum);

            //�����C�x���g���z��Ɋi�[����Ȃ��悤�ɂ���
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //Debug.Log($"rnd:{rnd}");
                //�z��ɒǉ�����
                powerUpEvents[i] = powerUpEventList[rnd];
                powerUpButtonsSprites[i] = powerUpEventList[rnd].powerUpButtonSprite;
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

    //���e�ǉ��C�x���g�ŌĂ΂��
    //���e�ǉ��{�^���ɔ��e�ǉ��֐���o�^����
    public void RegisterAddNewBombEvent()
    {
        //�O��{�^���ɓo�^�������X�i�[(���e�ǉ��֐��̂�)���폜����
        //addNewBombButton.onClick.RemoveListener(addNewBombListener);

        //�����_���ň�������̃��X�i�[��o�^���邽�߂ɕK�v


        //�{�^���ɓo�^���郊�X�i�[��ێ�����


        //�{�^���ɓo�^���郊�X�i�[��o�^����
        //addNewBombButton.onClick.AddListener(powerUpListeners);

        //�{�^���̉摜��ύX
        //addNewBombButton.image.sprite = 
    }
}
