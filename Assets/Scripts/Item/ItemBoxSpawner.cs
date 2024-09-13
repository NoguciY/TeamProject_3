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

    //�O��̊Ԋu
    private float beforeInterval;

    private void Start()
    {
        beforeInterval = 0;
    }

    private void Update()
    {
        //�o�ߎ���
        float elapsedTime = GameManager.Instance.GetDeltaTimeInMain;

        //�Ԋu = �o�ߎ��� / �����Ԋu �̗]��
        float interval = elapsedTime % spawnInterval;

        //�ݒ肵���b�����Ɏ��s����
        if (interval < beforeInterval)
        {
            SpawnObjects();
        }

        beforeInterval = interval;
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
