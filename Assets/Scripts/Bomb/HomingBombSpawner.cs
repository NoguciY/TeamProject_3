using System.Collections;
using UnityEngine;

public class HomingBombSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    
    [SerializeField, Min(1), Header("��x�ɕ��o����e�̐�")]
    int iterationCount;     
    
    [SerializeField, Header("�����Ԋu")]
    float interval;      
    
    [SerializeField, Header("��������")]
    float lifeTime;         
    
    [SerializeField, Header("�N�[���^�C��")]
    float coolTime;         

    //�v���C���[�̃g�����X�t�H�[��
    public Transform playerTransform;

    //���e�̍����̔���
    private float bombHalfHeight;

    //�������̊�ƂȂ鍂��
    private float offsetHeight;

    private bool isSpawning = false;
    
    private Transform myTransform;

    WaitForSeconds intervalWait;

    private Quaternion bombRotation;

    //�Q�b�^�[
    public GameObject GetPrefab => prefab;
    public float GetBombHalfHeight => bombHalfHeight;
    public float GetCoolTime => coolTime;

    void Start()
    {
        myTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //���e�̍����̔������擾����
        CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
        bombHalfHeight = prefab.transform.localScale.y * capsuleCollider.radius;
        bombRotation = prefab.transform.rotation;

        offsetHeight = 10;
    }

    void Update()
    {
        if (playerTransform != null)
        transform.position = playerTransform.position + Vector3.up * bombHalfHeight * offsetHeight;  

        if (isSpawning)
            return;

        //�U�����e�𐶐�����
        StartCoroutine(nameof(SpawnHomingBomb));
    }

    //�U���^���e�𐶐�����R���[�`��
    IEnumerator SpawnHomingBomb()
    {
        isSpawning = true;

        BombHoming homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, myTransform.position, bombRotation).GetComponent<BombHoming>();
        }

        yield return intervalWait;

        isSpawning = false;
    }
}