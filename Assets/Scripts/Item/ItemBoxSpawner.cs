using UnityEngine;

public class ItemBoxSpawner : MonoBehaviour
{
    [SerializeField, Header("�X�|�[��������I�u�W�F�N�g")]
    private GameObject spawnObject;

    [SerializeField, Header("�v���C���[��Transform")]
    private Transform playerTransform;

    [SerializeField, Header("�X�|�[���͈͂̔��a")]
    private float spawnRadius; 

    [SerializeField,Header("�X�|�[���Ԋu")]
    private float spawnInterval;

    private void Start()
    {
        //���̐������w�肵���Ԋu�ōs��
        InvokeRepeating("SpawnObjects", spawnInterval, spawnInterval);
    }

    /// <summary>
    /// �����_���Ȉʒu�ɔ��𐶐�����
    /// </summary>
    private void SpawnObjects()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = playerTransform.position.y;  // �������v���C���[�Ɠ����ɂ���
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }
}
