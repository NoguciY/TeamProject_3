using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Utilities;

public class PowerUpButton : MonoBehaviour
{
    //�������ڃ{�^���̐�
    private const int POWERUPBUTTONNUM = 3;

    //�p���[�A�b�v�{�^��
    public Button[] powerUpButtons = new Button[POWERUPBUTTONNUM];

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
    public PowerUpEvent powerUpDifenseEvent = new PowerUpEvent { };
    
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
    private Sprite[] powerUpButtonSprites = new Sprite[POWERUPBUTTONNUM];

    //�����{�^���ɓo�^���鋭���C�x���g���i�[����z��
    [NonSerialized]
    public UnityEvent[] powerUpEvents = new UnityEvent[POWERUPBUTTONNUM];

    //�����{�^���ɓo�^���鋭���C�x���g��ێ�����z��(���X�i�[)
    private UnityAction[] powerUpListeners = new UnityAction[POWERUPBUTTONNUM];

    //�����{�^���ɓo�^���鋭���C�x���g�̌�⃊�X�g
    private List<PowerUpEvent> powerUpEventList = new List<PowerUpEvent>(POWERUPBUTTONNUM);


    //���e�ǉ��{�^��
    public Button addNewBombButton;

    //���e�ǉ��{�^���摜
    [SerializeField]
    private Sprite[] addNewBombButtonSprites;

    //���e�ǉ��{�^���ɓo�^����C�x���g
    [NonSerialized]
    public UnityEvent addNewBombEvent = new UnityEvent();

    //���e�ǉ��{�^���ɓo�^����C�x���g��ێ�����
    private UnityAction addNewBombListener = null;

    private void Awake()
    {
        //powerUpButtons = new Button[POWERUPBUTTONNUM];

        //powerUpMaxLifeEvent = new PowerUpEvent { };
        //powerUpSpeedEvent = new PowerUpEvent { };
        //powerUpDifenseEvent = new PowerUpEvent { };
        //powerUpCollectionRangeRateEvent = new PowerUpEvent { };
        //powerUpResilienceEvent = new PowerUpEvent { };
        //powerUpBombRangeEvent = new PowerUpEvent { };

        //powerUpButtonSprites = new Sprite[POWERUPBUTTONNUM];

        //powerUpEvents = new UnityEvent[POWERUPBUTTONNUM];

        //powerUpListeners = new UnityAction[POWERUPBUTTONNUM];

        //powerUpEventList = new List<PowerUpEvent>(POWERUPBUTTONNUM);


        //addNewBombEvent = new UnityEvent();
        //addNewBombListener = null;
    }

    /// <summary>
    /// �����C�x���g�N���X��sprite�ɋ����{�^���摜��ݒ�
    /// </summary>
    public void SetPowerUpButtoSprites()
    {
        powerUpMaxLifeEvent.powerUpButtonSprite = powerUpButtonSpriteList[0];
        powerUpSpeedEvent.powerUpButtonSprite = powerUpButtonSpriteList[1];
        powerUpDifenseEvent.powerUpButtonSprite = powerUpButtonSpriteList[2];
        powerUpCollectionRangeRateEvent.powerUpButtonSprite = powerUpButtonSpriteList[3];
        powerUpResilienceEvent.powerUpButtonSprite = powerUpButtonSpriteList[4];
        powerUpBombRangeEvent.powerUpButtonSprite = powerUpButtonSpriteList[5];
    }

    /// <summary>
    /// ���x���A�b�v�C�x���g�ŌĂ΂��
    /// �����{�^���ɋ����֐���o�^����
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void RegisterPowerUpItemEvents(PlayerManager player)
    {
        //���X�g�ɋ����\�ȋ����C�x���g��ǉ�����
        SetPowerUpItemsList(player);

        //���X�g���烉���_���ɋ������ڃ{�^���̐����A�z��Ɋi�[
        GetRandomPowerUpItemEvents();

        for (int i = 0; i < POWERUPBUTTONNUM; i++)
        {
            //�O��{�^���ɓo�^�������X�i�[(�����֐��̂�)���폜����
            if (powerUpListeners[i] != null)
            {
                powerUpButtons[i].onClick.RemoveListener(powerUpListeners[i]);
            }

            //�����_���ň�������̃��X�i�[��o�^���邽�ߕK�v
            int index = i;

            //�{�^���ɓo�^���郊�X�i�[��ێ�����
            powerUpListeners[index] = () => powerUpEvents[index].Invoke();

            //�{�^���Ƀ��X�i�[��o�^����
            powerUpButtons[i].onClick.AddListener(powerUpListeners[i]);

            //�{�^���̉摜��ύX
            powerUpButtons[i].image.sprite = powerUpButtonSprites[i];
        }
    }

    /// <summary>
    /// ���X�g�ɋ����\�ȋ����C�x���g��ǉ�����
    /// </summary>
    /// <param name="player">PlayerManager</param>
    private void SetPowerUpItemsList(PlayerManager player)
    {
        powerUpEventList.Clear();

        //�������ڃC�x���g���X�g�ɋ����\�ȋ������ڂ��i�[
        //�����̋������ڂ�ǉ�����
        powerUpEventList.Add(powerUpMaxLifeEvent);

        //����������l��艺�̏ꍇ�A�ǉ�
        if (player.speed < Utilities.SPEEDUPPERLIMIT)
        {
            powerUpEventList.Add(powerUpSpeedEvent);
        }

        powerUpEventList.Add(powerUpCollectionRangeRateEvent);

        //�h��͂�����l��艺�̏ꍇ�A�ǉ�
        if (player.difense < Utilities.DEFENSEUPPERLIMIT)
        {
            powerUpEventList.Add(powerUpDifenseEvent);
        }

        powerUpEventList.Add(powerUpResilienceEvent);

        //�ݒu�^���e���g�p�\�̏ꍇ�A�ǉ�
        if (player.GetBombManager.CanUseBumb((int)BombManager.BombType.Planted))
        {
            powerUpEventList.Add(powerUpBombRangeEvent);
        }
    }

    /// <summary>
    /// �����C�x���g���X�g���烉���_���ɋ������ڃ{�^���̐������A�z��Ɋi�[
    /// </summary>
    private void GetRandomPowerUpItemEvents()
    {
        //���X�g�̐�
        int listNum = powerUpEventList.Count;

        //1��̃����_���Ȓl(�����l�ɂ́A�����_���ł͑I�΂�Ȃ��l�ł��郊�X�g�̐����i�[)
        int beforeRndFirst = listNum;
        //2��̃����_���Ȓl
        int beforeRndSecond = listNum;

        int i = 0;

        while(i < POWERUPBUTTONNUM)
        {
            //0���烊�X�g�̐�-1�̐������烉���_���Ȓl���i�[
            int rnd = UnityEngine.Random.Range(0, listNum);

            //�����C�x���g���z��Ɋi�[����Ȃ��悤�ɂ���
            if (beforeRndFirst != rnd && beforeRndSecond != rnd)
            {
                //�z��ɒǉ�����
                powerUpEvents[i] = powerUpEventList[rnd];
                powerUpButtonSprites[i] = powerUpEventList[rnd].powerUpButtonSprite;
                //Debug.Log($"powerUpEvents[{i}]��events[{rnd}]��ǉ�");

                //1��ڂ̃����_���Ȓl���i�[
                if (i == 0)
                {
                    beforeRndFirst = rnd;
                }
                //2��ڂ̃����_���Ȓl���i�[
                else if (i == 1)
                {
                    beforeRndSecond = rnd;
                }

                i++;
            }
        }
    }

    /// <summary>
    /// ���e�ǉ��C�x���g�ɓo�^����֐�
    /// ���e�ǉ��{�^���ɔ��e�ǉ��֐���o�^����
    /// </summary>
    /// <param name="newBombID">�V�����ǉ����锚�e�̌ŗL�ԍ�</param>
    public void RegisterAddNewBombEvent(int newBombID)
    {
        //�O��{�^���ɓo�^�������X�i�[(���e�ǉ��֐��̂�)���폜����
        if(addNewBombListener != null)
        addNewBombButton.onClick.RemoveListener(addNewBombListener);

        //�{�^�������������Ɏ��s����C�x���g��o�^����
        addNewBombListener = () => addNewBombEvent.Invoke();

        //�{�^���Ƀ��X�i�[��o�^����
        addNewBombButton.onClick.AddListener(addNewBombListener);

        //�{�^���̉摜��ύX , newBombID - 1�FnewBombID��1����n�܂邽�� -1 ���Ă���
        addNewBombButton.image.sprite = addNewBombButtonSprites[newBombID - 1];
    }
}
