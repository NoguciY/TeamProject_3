using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float fuseTime;          //��������܂ł̎���
    public float explosionRadius;   //�͈�

    public float maxUpwardHeight = 5f; // �㏸����ő卂��
    public float moveSpeed = 10f; // �ړ����x

    private bool isAscending = true; // �㏸�����ǂ����̃t���O
    private Vector3 initialPosition; // �����ʒu
    private Transform target; // ���݂̃^�[�Q�b�g
    internal GameObject Target;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private CapsuleCollider capsuleCollider;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 _forward = Vector3.forward;

    //�X�t�B�A�L���X�g�̍ő勗��
    private float maxDistance;

    void Start()
    {
        initialPosition = transform.position;
        SetRandomTarget(); // �ŏ��̃^�[�Q�b�g��ݒ�
        Invoke("Detonate", fuseTime);
        maxDistance = 0;
    }

    void Update()
    {
        if (isAscending)
        {
            // �㏸
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // ���̍����܂ŏ㏸������
            if (transform.position.y >= initialPosition.y + maxUpwardHeight)
            {
                isAscending = false;
                SetRandomTarget(); // �^�[�Q�b�g��ݒ�
            }
        }
        else if (isAscending == false)
        {
            if (target == null)
            {
                // �㏸
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                isAscending = true;
                return;
            }
            // �^�[�Q�b�g�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // �^�[�Q�b�g�ւ̌����x�N�g���v�Z
            var dir = target.position - transform.position;
            // �^�[�Q�b�g�̕����ւ̉�]
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // ��]�␳
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

            // ��]�␳���^�[�Q�b�g�����ւ̉�]�̏��ɁA���g�̌����𑀍삷��
            transform.rotation = lookAtRotation * offsetRotation;

            // �^�[�Q�b�g�ɓ��B������
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Detonate();
            }
        }
    }

    void SetRandomTarget()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy"); // "Target"�^�O�̃I�u�W�F�N�g���擾
        if (targetObjects.Length > 0)
        {
            // �����_���Ƀ^�[�Q�b�g��I��
            int randomIndex = Random.Range(0, targetObjects.Length);
            target = targetObjects[randomIndex].transform;
        }
    }
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward, maxDistance);

        foreach (var hit in hits)
        {
            Player playe = hit.collider.gameObject.GetComponent<Player>();
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //�G�ɓ��������ꍇ�A�_���[�W��^����
                hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
            }
        }

        Destroy(gameObject);
    }
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            transform.localScale.y * capsuleCollider.radius; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Field"))
        {
            Detonate();
        }
    }
}
