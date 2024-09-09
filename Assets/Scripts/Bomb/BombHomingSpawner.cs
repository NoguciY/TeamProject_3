using System.Collections;
using UnityEngine;

public class BombHomingSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject homingBomb;

    public GameObject GetPrefab => homingBomb;

    [SerializeField, Min(1), Header("��x�ɕ��o����e�̐�")]
    int iterationCount;     
    
    [SerializeField, Header("�����Ԋu")]
    float interval;      
    
    [SerializeField, Header("��������")]
    float lifeTime;         
    
    [SerializeField, Header("�N�[���^�C��")]
    float coolTime;

    [SerializeField, Header("�������鍂���̊�l")]
    private float offsetHeight;

    //�ǔ����e�̃R���C�_�[�R���|�[�l���g
    [SerializeField]
    private CapsuleCollider bombCapsuleCollider;

    public float GetCoolTime => coolTime;

    //�v���C���[�̃g�����X�t�H�[��
    public Transform playerTransform;

    //���e�̍����̔���
    private float bombHalfHeight;
    
    private bool isSpawning = false;
    
    private Transform myTransform;

    private WaitForSeconds intervalWait;

    private Quaternion bombRotation;
    

    private void Start()
    {
        myTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //���e�̍����̔������擾����
        bombHalfHeight = GetBombHalfHeight();

        bombRotation = homingBomb.transform.rotation;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + Vector3.up * bombHalfHeight * offsetHeight;
        }

        if (isSpawning) return;

        //�U�����e�𐶐�����
        StartCoroutine(nameof(SpawnHomingBomb));
    }

    /// <summary>
    /// �U���^���e�𐶐�����R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnHomingBomb()
    {
        isSpawning = true;

        BombHoming homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(homingBomb, myTransform.position, bombRotation).GetComponent<BombHoming>();
        }

        yield return intervalWait;

        isSpawning = false;
    }

    /// <summary>
    /// ���e�̍����̔������擾����
    /// </summary>
    /// <returns>���e�̔������̍���</returns>
    public float GetBombHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            homingBomb.transform.localScale.y * bombCapsuleCollider.radius;
    }
}