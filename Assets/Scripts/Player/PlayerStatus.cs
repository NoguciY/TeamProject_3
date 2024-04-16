using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//�Q�[����ʂ��Ẵv���C���[�̃X�e�[�^�X��ێ�����

public class PlayerStatus : MonoBehaviour
{
    //�ő�HP
    public float maxLife = 100;

    //�ړ����x
    public float speed = 3f;

    //����͈͗�(1�ł̓v���C���[�Ɠ����傫��)
    public float collectionRangeRate = 1f;

    //�h���
    public float difence = 0;
    
    //�񕜗�
    public float resilience = 0f;


    //�X�e�[�^�X�����Z�b�g����
    public void ResetStatus()
    {
        maxLife = 100;
        speed = 3f;
        collectionRangeRate = 1f;
        difence = 0;
        resilience = 0f;
    }
}
