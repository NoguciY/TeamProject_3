using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceValue : MonoBehaviour
{
    //���x���A�b�v�ɕK�v�Ȍo���l�̊�l
    private float offsetNeedExp = 0.5f;
    
    //���x���ƕK�v�o���l�����f�B�N�V���i��
    private Dictionary<int, float> needExpDictionary;

    // ���x���ɕK�v�Ȍo���l���v�Z���ăf�B�N�V���i���Ɋi�[����
    public void SetNeedExperience(int maxLevel)
    {
        //���x���ƕK�v�o���l���i�[����
        needExpDictionary = new Dictionary<int, float>();

        //���x��1�̕K�v�o���l
        //needExpDictionary[1] = 0;
        needExpDictionary.Add(1, 0);

        //���x��2����ő僌�x���܂ł̕K�v�o���l
        for (int i = 2; i <= maxLevel; i++)
            //needExpDictionary[i] = offsetNeedExp + needExpDictionary[i - 1];
            needExpDictionary.Add(i, offsetNeedExp + needExpDictionary[i - 1]);
    }


    //�����œn���ꂽ���x���ɕK�v�Ȍo���l��Ԃ�
    public float GetNeedExp(int currentLevel)
    {
        //��u�C���N�������g�ɂ���Ƒ����ɃC���N�������g�����̂Œ���
        int nextLevel = ++currentLevel;
        return needExpDictionary[nextLevel];
    }
}
