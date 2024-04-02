using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience //: MonoBehaviour
{
    //�ő僌�x��
    private int maxLevel = 100;
    //���x���A�b�v�ɕK�v�Ȍo���l�̊�l
    private int offsetNeedExp = 1;
    //���x���ƕK�v�o���l�����f�B�N�V���i��
    private Dictionary<int, int> needExpDictionary;


    //�R���X�g���N�^
    public Experience(int maxLevel, int offsetNeedExp)
    {
        this.maxLevel = maxLevel;
        this.offsetNeedExp = offsetNeedExp;
    }


    /// ���x���ɕK�v�Ȍo���l���v�Z���ăf�B�N�V���i���Ɋi�[����
    public void CalNeedExperience()
    {
        needExpDictionary = new Dictionary<int, int>();

        //���x��1�̎��́A�K�v�o���l��0
        needExpDictionary[1] = 0;

        for (int i = 2; i <= maxLevel; i++)
        {
            needExpDictionary[i] = offsetNeedExp + needExpDictionary[i - 1];
        }
    }


    //�����œn���ꂽ���x���ɕK�v�Ȍo���l��Ԃ�
    public int GetNeedExp(int level)
    {
        return needExpDictionary[level];
    }
}
