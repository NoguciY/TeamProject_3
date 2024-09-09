using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    //���x���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI levelText;          

    //���݂̃��x��
    private int currentLevel;

    //�O�̃��x��
    private int beforeLevel;

    /// <summary>
    /// ���x�����ω������ꍇ�A���݂̃��x���Ƀe�L�X�g������������
    /// </summary>
    /// <param name="level">���x��</param>
    public void CountLevel(int level)
    {
        currentLevel = level;

        if (currentLevel != beforeLevel)
        {
            levelText.text = level.ToString() + "Lv";
        }
        beforeLevel = currentLevel;
    }
}
