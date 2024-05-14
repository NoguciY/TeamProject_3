using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    GameObject target;
    [SerializeField]
    GameObject prefab;
    [SerializeField, Min(1)]
    int iterationCount = 3;     //��x�ɕ��o����e�̐�
    [SerializeField]
    float interval = 0.1f;      //�����Ԋu
    [SerializeField]
    float lifeTime = 2;         //��������

    //�v���C���[�̃g�����X�t�H�[��
    public Transform playerTransform;

    //���e�̍����̔���
    private float bombHalfHeight;

    bool isSpawning = false;
    Transform thisTransform;
    WaitForSeconds intervalWait;

    private Quaternion bombRotation;

    public float GetBombHalfHeight => bombHalfHeight;

    void Start()
    {
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);
        Destroy(gameObject, lifeTime);

        //���e�̍����̔������擾����
        CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
        bombHalfHeight = prefab.transform.localScale.y * capsuleCollider.radius;
        bombRotation = prefab.transform.rotation;
    }

    void Update()
    {
        if (playerTransform != null)
        transform.position = playerTransform.position + Vector3.up * bombHalfHeight * 10;  

        if (isSpawning)
        {
            return;
        }

        StartCoroutine(nameof(SpawnMissile));
    }

    IEnumerator SpawnMissile()
    {
        isSpawning = true;

        //Vector3 euler;
        //Quaternion rot;
        Bullet homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, thisTransform.position, bombRotation).GetComponent<Bullet>();
            homing.Target = target;
        }

        yield return intervalWait;

        isSpawning = false;
    }
}