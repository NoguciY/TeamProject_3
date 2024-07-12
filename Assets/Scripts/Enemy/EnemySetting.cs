using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Setting", fileName = "EnemySetting")]
public class EnemySetting : ScriptableObject
{
    //�G�̎�ނ��ƂɃf�[�^�������߂̃��X�g
    public List<EnemyData> enemyDataList;

    //�G�P�̕��̃f�[�^
    [Serializable]
    public class EnemyData
    {
        [Header("���O")]
        public string name;

        [Header("�o�ꏇ")]
        public int generationOrder;

        [Header("�ߗׂ̌̂����m���鋗��")]
        public float ditectingNeiborDistance;

        [Header("����p")]
        public float fieldOfView;

        [Header("��]���x")]
        public float rotationSpeed;

        [Header("�����W��")]
        public float separationCoefficient;

        [Header("����W��")]
        public float alignmentCoefficient;

        [Header("�����W��")]
        public float combiningCoefficient;

        [Header("�v���C���[�Ɍ�������͂̌W��")]
        public float combiningPlayerCoefficient;

        [Header("�ŏ����x")]
        public float minSpeed;

        [Header("�ő呬�x")]
        public float maxSpeed;

        [Header("�ő�̗�")]
        public float maxHealth;

        [Header("�U����")]
        public float attack;
    }
}
