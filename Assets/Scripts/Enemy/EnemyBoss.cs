using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    //�ő�HP
    [SerializeField]
    private int maxLife;
    private int Life;

    //�X�s�[�h
    public float Speed;


    //GameObject�^��ϐ�target�Ő錾���܂��B
    public GameObject target;

    void Start()
    {
        Life = maxLife;
    }

    void Update()
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        lookRotation.z = 0;
        lookRotation.x = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

        Vector3 p = new Vector3(0f, 0f, Speed);

        transform.Translate(p);
    }
    public void RecieveDamage(int damage)
    {
        Life -= damage;
        Debug.Log($"�v���C���[��{damage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{Life}");
    }
    private void OnTriggerEnter(Collider other)
    {
        //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
        {
            //�_���[�W���󂯂�����
            applicableDamageObject.RecieveDamage(10);
        }
    }
}