using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerLevelUp : MonoBehaviour
{
    //�o���l�R���|�[�l���g
    [SerializeField]
    private ExperienceValue experienceValue;

    //�ő僌�x��
    private int maxLevel = 300;

    //���݂̃��x��
    [SerializeField]
    private int level;

    //���݂̌o���l
    private float exp;

    //���x���A�b�v�ɕK�v�Ȍo���l
    private float needExp;


    //�Q�b�^�[
    public int GetLevel => level;
    public float GetExp => exp;
    public float GetNeedExp => needExp;

    //������
    public void InitLevel()
    {
        level = 1;
        exp = 0;

        //�e���x���ɕK�v�Ȍo���l���v�Z
        experienceValue.SetNeedExperience(maxLevel);

        //���݂̃��x���A�b�v�ɕK�v�Ȍo���l���擾
        needExp = experienceValue.GetNeedExp(level);
    }

    //�o���l�𓾂�
    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    //���x���A�b�v
    public void LevelUp(UnityEvent levelUpEvent)
    {
        //�o���l�����x���A�b�v�ɕK�v�Ȍo���l�ȏ�ɂȂ����ꍇ
        if (exp >= needExp)
        {
            level++;
            exp =�@exp - needExp;
            needExp = experienceValue.GetNeedExp(level);
            Debug.Log($"���x���A�b�v�I\n ���݂̃��x���F{level}");

            //�C�x���g(�p�l���̕\����G�t�F�N�g�Ȃ�)�����s
            levelUpEvent.Invoke();
        }
    }
}
