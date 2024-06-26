using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBoxSpaner : MonoBehaviour
{
    [SerializeField, Header("�X�|�[��������I�u�W�F�N�g")]
    private GameObject spawnObject;

    [SerializeField, Header("�v���C���[��Transform")]
    private Transform playerTransform;

    [SerializeField, Header("�X�|�[���͈͂̔��a")]
    private float spawnRadius; 

    [SerializeField,Header("�X�|�[���Ԋu")]
    private float spawnInterval;
    void Start()
    {
        InvokeRepeating("SpawnObjects", spawnInterval, spawnInterval);
    }

    void SpawnObjects()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = playerTransform.position.y;  // �������v���C���[�Ɠ����ɂ���
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }
}
