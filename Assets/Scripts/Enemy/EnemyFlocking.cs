using System.Collections.Generic;
using UnityEngine;

//�Q�����m���āA�i�s���������߂Ĉړ�����
//�Q�����m���鋗���A�ڋ߂����������A�G�̑����A�́A
//�G���Ƃɕς�����

public class EnemyFlocking : MonoBehaviour
{
    //�ߗׂ̌̂��i�[����
    [SerializeField]
    private List<GameObject> neighbors;

    //�ړ������Ɏg�����x
    [SerializeField]
    private Vector3 velocity;

    private Transform myTransform;

    //����
    private float speed;

    //�����x
    private Vector3 acceleration;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="minSpeed">�����̉���</param>
    /// <param name="maxSpeed">�����̏��</param>
    public void Init(float minSpeed, float maxSpeed)
    {
        myTransform = transform;

        speed = Random.Range(minSpeed, maxSpeed);

        velocity = speed * Vector3.forward;
    }

    /// <summary>
    /// �ߗׂ̒��Ԃ�T���ă��X�g�ɒǉ�
    /// </summary>
    /// <param name="flockManager">�G�𐶐�����R���|�[�l���g</param>
    /// <param name="index">�G�̓o�ꏇ</param>
    /// <param name="ditectingNeiborDistance">�ߗׂ̌̂����m���鋗��</param>
    /// <param name="innerProductThred">����p�����Ƃɂ�������</param>
    public void AddNeighbors(EnemySpawner flockManager, int index, float ditectingNeiborDistance, float innerProductThred)
    {
        //���X�g���N���A
        neighbors.Clear();

        foreach(var boid in flockManager.boids[index])
        {
            //���g�ȊO��floackManger���������I�u�W�F�N�g
            if(boid != this.gameObject)
            {
                //���g���璇�Ԃ̃x�N�g��
                Vector3 toOtherVec = boid.transform.position - this.transform.position;
                
                //magnitude��菈������������sqrMagnitude���g��
                float sqrDistance = toOtherVec.sqrMagnitude;

                //���g�ƒ��Ԃ̋�����ditectingNeiborDistance�ȉ��̏ꍇ
                if (sqrDistance <= Mathf.Pow(ditectingNeiborDistance, 2))
                {
                    //���g���璇�Ԃ̕����Ǝ��g�̐��ʕ���
                    Vector3 direction = toOtherVec.normalized;
                    Vector3 forward = velocity.normalized;

                    //���ς��g������p���ɂ��钇�Ԃ݂̂����X�g�ɒǉ�����
                    float innerProduct = Vector3.Dot(forward, direction);
                    if (innerProduct >= innerProductThred)
                    {
                        neighbors.Add(boid);
                    }
                }
            }
        }
    }

    /// <summary>
    /// �͂��܂Ƃ߂đ��x�Ƃ��ĕԂ�
    /// </summary>
    /// <param name="playerTransform">�v���C���[��Transform</param>
    /// <param name="enemyData">�G�̏��</param>
    /// <returns>�ړ����x</returns>
    public Vector3 Move(Transform playerTransform, EnemySetting.EnemyData enemyData)
    {
        //�t���[�����Ƃɑ��x�ɉ��Z����l(�����x)�����߂�
        acceleration = SeparateNeighbors(enemyData.separationCoefficient)
                     + AlignNeighbors(enemyData.alignmentCoefficient)
                     + CombineNeighbors(enemyData.combiningCoefficient)
                     + CombinePlayer(playerTransform, enemyData.combiningPlayerCoefficient);

        //����p���̃v���C���[�Ɍ�������
        //+ CombineNearbyPlayer() * combiningPlayerCoefficient
        //�t�B�[���h���ɂƂǂ߂��(�ǂɋ߂Â��Ɨ����)
        //+ StayWithinRange();

        velocity += acceleration * Time.deltaTime;

        float speed = velocity.magnitude;
        Vector3 direction = velocity.normalized;

        //���x�𐧌�����
        velocity = Mathf.Clamp(speed, enemyData.minSpeed, enemyData.maxSpeed) * direction;

        //�ړ������𐳖ʕ����ɂ��邽�߂ɉ�]����
        if (direction != Vector3.zero)
        {
            //�㉺�ɉ�]���Ȃ��悤�ɂ���
            direction.y = 0;

            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                        Quaternion.LookRotation(direction),
                                        enemyData.rotationSpeed * Time.deltaTime);
        }
        
        //���̃t���[���ɔ����ă��Z�b�g
        acceleration = Vector3.zero;

        return velocity;
    }

    /// <summary>
    /// �ߗׂ̌̂��痣���(��������)�͂�Ԃ�
    /// </summary>
    /// <param name="coefficient">��������͂ɂ�����W��</param>
    /// <returns>�ߗׂ̌Q�ꂩ�痣����</returns>
    private Vector3 SeparateNeighbors(float coefficient)
    {
        Vector3 avoidanceForce = Vector3.zero;

        if (neighbors.Count == 0) return avoidanceForce;

        //�ߗׂ̌Q���痣���x�N�g����Ԃ�
        foreach (GameObject neighbor in neighbors)
        {
            //�I�u�W�F�N�g���j������ĂȂ��ꍇ
            if (neighbor != null)
            {
                avoidanceForce += myTransform.position - neighbor.transform.position;
            }
        }

        return coefficient * avoidanceForce.normalized;
    }

    /// <summary>
    /// �ߗׂ̌Q�ɍ��킹�Đ��񂳂�������̗͂�Ԃ�
    /// </summary>
    /// <param name="coefficient">���񂷂�͂ɂ�����W��</param>
    /// <returns>�Q�Ɛ��񂳂��悤�Ƃ����</returns>
    private Vector3 AlignNeighbors(float coefficient)
    {
        Vector3 averageVelocity = Vector3.zero;

        if (neighbors.Count == 0) return averageVelocity;

        //�ߗׂ̌Q�̕��ϑ��x�̌v�Z
        foreach (GameObject neighbor in neighbors)
        {
            //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
            if (neighbor != null)
            {
                EnemyFlocking anotherFlock = neighbor.GetComponent<EnemyFlocking>();
                averageVelocity += anotherFlock.velocity;
            }
        }
        averageVelocity /= neighbors.Count;

        return coefficient * (averageVelocity - velocity).normalized;
    }

    /// <summary>
    /// �ߗׂ̌Q�̒��S�ɋ߂Â�(��������)�͂�Ԃ�
    /// </summary>
    /// <param name="coefficient">��������͂ɂ�����W��</param>
    /// <returns>�Q�ƌ��������</returns>
    private Vector3 CombineNeighbors(float coefficient)
    {
        Vector3 centerPos = Vector3.zero;

        if (neighbors.Count == 0) return centerPos;

        //�Q�̒��S�ʒu�̌v�Z
        foreach (GameObject neighbor in neighbors)
        {
            //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
            if (neighbor != null)
            {
                centerPos += neighbor.transform.position;
            }
        }
        centerPos /= neighbors.Count;

        //���S�����֌������͂�Ԃ�
        return coefficient * (centerPos - myTransform.position).normalized;
    }

    /// <summary>
    /// �v���C���[�����֌������͂�Ԃ�
    /// </summary>
    /// <param name="playerTransform">�v���C���[��Transform</param>
    /// <param name="coefficient">�v���C���[�����Ɍ������͂ɂ�����W��</param>
    /// <returns></returns>
    private Vector3 CombinePlayer(Transform playerTransform, float coefficient)
    {
        Vector3 combineForce = Vector3.zero;

        if(playerTransform == null) return combineForce;

        //�v���C���[�̈ʒu���擾����
        Vector3 playerPos = playerTransform.position;

        //�v���C���[�Ɍ�������
        combineForce = playerPos - myTransform.position;

        return coefficient * combineForce.normalized;
    }
}
