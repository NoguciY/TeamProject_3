using UnityEngine;
using UnityEngine.Events;


public class PlayerLevelUp : MonoBehaviour
{
    //�o���l�R���|�[�l���g
    [SerializeField]
    private ExperienceValue experienceValue;

    //�ő僌�x��
    private int maxLevel;

    //���݂̃��x��
    [SerializeField]
    private int level;

    //���݂̌o���l
    private float currentExperienceValue;

    //���x���A�b�v�ɕK�v�Ȍo���l
    private float needExperienceValue;


    //�Q�b�^�[
    public int GetLevel => level;
    public float GetExperienceValue => currentExperienceValue;
    public float GetNeedExperienceValue => needExperienceValue;

    /// <summary>
    /// ������
    /// </summary>
    public void InitLevel()
    {
        level = 1;
        currentExperienceValue = 0;
        maxLevel = 300;

        //�e���x���ɕK�v�Ȍo���l���v�Z
        experienceValue.SetNeedExperience(maxLevel);

        //���݂̃��x���A�b�v�ɕK�v�Ȍo���l���擾
        needExperienceValue = experienceValue.GetNeedExperienceValue(level);
    }

    /// <summary>
    /// �o���l�𓾂�
    /// </summary>
    /// <param name="experienceValue">�o���l</param>
    public void AddExperienceValue(float experienceValue)
    {
        currentExperienceValue += experienceValue;
        Debug.Log($"���݂̌o���l�F{currentExperienceValue}");
    }

    /// <summary>
    /// ���x���A�b�v
    /// </summary>
    /// <param name="normalLevelUpEvent">�ʏ�̃��x���A�b�v�C�x���g</param>
    /// <param name="addNewBombEvent">�V�������e��ǉ�����C�x���g</param>
    /// <param name="addNewBombLevel">�V�������e��ǉ����郌�x��</param>
    public void LevelUp(UnityEvent normalLevelUpEvent, UnityEvent addNewBombEvent, int addNewBombLevel)
    {
        //�o���l�����x���A�b�v�ɕK�v�Ȍo���l�ȏ�ɂȂ����ꍇ
        if (currentExperienceValue >= needExperienceValue)
        {
            level++;
            currentExperienceValue -= needExperienceValue;
            needExperienceValue = experienceValue.GetNeedExperienceValue(level);

            Debug.Log($"���x���A�b�v�I\n ���݂̃��x���F{ level }");
            Debug.Log($"�K�v�Ȍo���l�F{ needExperienceValue }");

            //�V�������e��ǉ�����C�x���g�����s
            if (level == addNewBombLevel)
            {
                addNewBombEvent.Invoke();
            }
            //�ʏ�̃��x���A�b�v�C�x���g�����s
            else
            {
                normalLevelUpEvent.Invoke();
            }
        }
    }
}
