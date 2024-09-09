using System.Collections.Generic;
using UnityEngine;

public class ExperienceValue : MonoBehaviour
{
    [SerializeField, Header("���x���A�b�v�ɕK�v�Ȍo���l�̊�l")]
    private float offsetNeedExperienceValue;
    
    //���x���ƕK�v�o���l�����f�B�N�V���i��
    private Dictionary<int, float> needExperienceValueDictionary;

    /// <summary>
    /// ���x���ɕK�v�Ȍo���l���v�Z���ăf�B�N�V���i���Ɋi�[����
    /// </summary>
    /// <param name="maxLevel">���x������l</param>
    public void SetNeedExperience(int maxLevel)
    {
        //���x���ƕK�v�o���l���i�[����
        needExperienceValueDictionary = new Dictionary<int, float>();

        //���x��1�̕K�v�o���l
        needExperienceValueDictionary.Add(1, 0);

        //���x��2����ő僌�x���܂ł̕K�v�o���l
        for (int i = 2; i <= maxLevel; i++)
        {
            needExperienceValueDictionary.Add(i, offsetNeedExperienceValue + needExperienceValueDictionary[i - 1]);
        }
    }


    /// <summary>
    /// �����œn���ꂽ���x���ɕK�v�Ȍo���l��Ԃ�
    /// </summary>
    /// <param name="currentLevel">���݂̃��x��</param>
    /// <returns>���x���A�b�v�ɕK�v�Ȍo���l</returns>
    public float GetNeedExperienceValue(int currentLevel)
    {
        //��u�C���N�������g�ɂ���Ƒ����ɃC���N�������g�����̂Œ���
        int nextLevel = ++currentLevel;
        return needExperienceValueDictionary[nextLevel];
    }
}
