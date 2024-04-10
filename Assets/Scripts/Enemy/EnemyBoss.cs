using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    //最大HP
    [SerializeField]
    private int maxLife;
    private int Life;

    //スピード
    public float Speed;


    //GameObject型を変数targetで宣言します。
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
        Debug.Log($"プレイヤーは{damage}ダメージ食らった\n" +
            $"残りの体力：{Life}");
    }
    private void OnTriggerEnter(Collider other)
    {
        //ダメージを受けることができるオブジェクトを取得
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
        {
            //ダメージを受けさせる
            applicableDamageObject.RecieveDamage(10);
        }
    }
}