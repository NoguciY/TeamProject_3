using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    //��������
    //enum PowerUpItems
    //{
    //    MaxLife,                //�ő�̗�
    //    Speed,                  //�ړ����x
    //    PlantedBombCoolDown,    //�ݒu�^���e�̎g�p���x
    //    PlantedBombRange,       //�ݒu�^���e�̍U���͈�
    //}

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PowerUpItems powerUpItems;

    //�������ڃ{�^��
    int maxlife;

    //�������ډ摜
    [SerializeField]
    private List<Image> poweUpItemImages;


    //�������ډ摜�Ƌ������ڃN���X�̃f�B�N�V���i��
    private Dictionary<Image, PowerUpItems> powerUpItemsDictionary;

    //�������ڂ̏�����
    public void InitPowerUpItems()
    {
        //powerUpItemsDictionary = new Dictionary<Image, PowerUpItems>()
        //{
        //    { poweUpItemImages, powerUpItems. },
        //};
    }


    //���x���A�b�v���ڂ������_���ɑI��
    //3�̐����������_���őI��(�ǉ��ƍ폜���s���̂Ń��X�g��������)
    //
}
