using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HomingSample : MonoBehaviour
{
    GameObject target;

    [SerializeField, Min(0)]
    float time = 1;

    [SerializeField]
    float lifeTime = 2;

    [SerializeField]
    bool limitAcceleration = false;

    [SerializeField, Min(0)]
    float maxAcceleration = 100;

    [SerializeField]
    Vector3 minInitVelocity;

    [SerializeField]
    Vector3 maxInitVelocity;


    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    Transform thisTransform;
    internal GameObject Target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy");

        thisTransform = transform;
        position = thisTransform.position;
        velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));

        Destroy(gameObject, lifeTime) ;
    }


    public void Update()
    {
        if (target == null){ target = GameObject.FindGameObjectWithTag("Enemy"); }

        //�����x�̎Z�o
        acceleration = 2f / (time * time) * (target.transform.position - position - time * velocity);

        //���������x������ON�Ȃ�Ή����x������l�ɐ���
        if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        //���������`�F�b�N
        time -= Time.deltaTime;

        if (time < 0f){return;}

        //���x�ƍ��W�̎Z�o
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        thisTransform.position = position;
        thisTransform.rotation = Quaternion.LookRotation(velocity);
    }

}
