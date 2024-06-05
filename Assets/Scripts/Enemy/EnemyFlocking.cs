using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//�Q�����m���āA�i�s���������߂Ĉړ�����
//�Q�����m���鋗���A�ڋ߂����������A�G�̑����A�́A
//�G���Ƃɕς�����

public class EnemyFlocking : MonoBehaviour
{
    private Transform myTransform;

    //�ߗׂ̌̂��i�[����
    [SerializeField]
    private List<GameObject> neighbors;

    //����
    private float speed;

    //�ړ������Ɏg�����x
    [SerializeField]
    private Vector3 velocity;

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

        //stageScale = Utilities.STAGESIZE * 0.5f;

        speed = Random.Range(minSpeed, maxSpeed);

        velocity = speed * Vector3.forward;
    }

    /// <summary>
    /// �ߗׂ̒��Ԃ�T���ă��X�g�ɒǉ�
    /// </summary>
    /// <param name="flockManager">�G�𐶐�����R���|�[�l���g</param>
    /// <param name="ditectingNeiborDistance">�ߗׂ̌̂����m���鋗��</param>
    /// <param name="innerProductThred">����p�����Ƃɂ�������</param>
    public void AddNeighbors(EnemySpawner flockManager, float ditectingNeiborDistance, float innerProductThred)
    {
        //���X�g���N���A
        neighbors.Clear();

        foreach(var boid in flockManager.boids)
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
                    if(innerProduct >= innerProductThred) 
                        neighbors.Add(boid);
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

        if (neighbors.Count == 0)
            return avoidanceForce;

        //�ߗׂ̌Q���痣���x�N�g����Ԃ�
        foreach (GameObject neighbor in neighbors)
            //�I�u�W�F�N�g���j������ĂȂ��ꍇ
            if (neighbor != null)
                avoidanceForce += myTransform.position - neighbor.transform.position;

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

        if (neighbors.Count == 0)
            return averageVelocity;

        //�ߗׂ̌Q�̕��ϑ��x�̌v�Z
        foreach (GameObject neighbor in neighbors)
        {
            //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
            if (neighbor != null)
            {
                //float flockDistance = Vector3.Distance(myTransform.position, neighbor.transform.position);
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

        if (neighbors.Count == 0)
            return centerPos;

        //�Q�̒��S�ʒu�̌v�Z
        foreach (GameObject neighbor in neighbors)
        {
            //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
            if (neighbor != null)
                centerPos += neighbor.transform.position;
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

        if(playerTransform == null)
            return combineForce;

        //�v���C���[�̈ʒu���擾����
        Vector3 playerPos = playerTransform.position;

        //�v���C���[�Ɍ�������
        combineForce = playerPos - myTransform.position;

        return coefficient * combineForce.normalized;
    }

    //�͂��܂Ƃ߂Ĉړ�����
    //private void UpdateMove()
    //{
    //    //�t���[�����Ƃɑ��x�ɉ��Z����l(�����x)�����߂�
    //    //acceleration = SeparateNeighbors(separationCoefficient)
    //    //             + StayWithinRange(restitutionDistance)
    //    //             //+ AlignNeighbors(alignmentCoefficient)
    //    //             + CombineNeighbors(combiningCoefficient)
    //    //             + CombinePlayer(combiningPlayerCoefficient);

    //    acceleration = SeparateNeighbors() * enemyManager.enemyData.separationCoefficient
    //                 + AlignNeighbors() * enemyManager.enemyData.alignmentCoefficient
    //                 + CombineNeighbors() * enemyManager.enemyData.combiningCoefficient
    //                 + CombinePlayer() * enemyManager.enemyData.combiningPlayerCoefficient
    //                 //+ CombineNearbyPlayer() * combiningPlayerCoefficient
    //                 + StayWithinRange();


    //    velocity += acceleration * Time.deltaTime;

    //    float speed = velocity.magnitude;
    //    Vector3 direction = velocity.normalized;

    //    //���x�𐧌�����
    //    velocity = Mathf.Clamp(speed, enemyManager.enemyData.minSpeed, enemyManager.enemyData.maxSpeed) * direction;

    //    if (direction != Vector3.zero)
    //    {
    //        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
    //                             Quaternion.LookRotation(direction),        //��������������
    //                             enemyManager.enemyData.rotationSpeed * Time.deltaTime);
    //    }
    //    if (!float.IsNaN(velocity.x) && !float.IsNaN(velocity.y) && !float.IsNaN(velocity.z))
    //    {
    //        myTransform.position += velocity * Time.deltaTime;
    //    }
    //    acceleration = Vector3.zero;
    //}

    //�Q���番����������̗͂�Ԃ�
    //private Vector3 SeparateNeighbors()
    //{
    //    Vector3 avoidanceForce = Vector3.zero;

    //    if (neighbors.Count <= 0)
    //        return avoidanceForce;

    //    //�߂��̌Q���痣���x�N�g�������߂�
    //    foreach (GameObject neighbor in neighbors)
    //    {
    //        //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
    //        if (neighbor != null)
    //            avoidanceForce += (myTransform.position - neighbor.transform.position);
    //    }

    //    return avoidanceForce.normalized;
    //}



    //�Q�Ɛ��񂷂�����̗͂�Ԃ�
    //private Vector3 AlignNeighbors()
    //{
    //    Vector3 averageVelocity = Vector3.zero;

    //    if (neighbors.Count == 0)
    //        return averageVelocity;

    //    //�߂��̌Q�̕��ϑ��x�����߂�
    //    foreach (GameObject neighbor in neighbors)
    //    {
    //        //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
    //        if (neighbor != null)
    //        {
    //            EnemyFlocking anotherFlock = neighbor.GetComponent<EnemyFlocking>();
    //            averageVelocity += anotherFlock.velocity;
    //        }
    //    }
    //    averageVelocity /= neighbors.Count;

    //    return averageVelocity.normalized;
    //}
    //---------------------------------------------------------------------------------------------

 
    
    //�Q�Ɍ�����������̗͂�Ԃ�
    //private Vector3 CombineNeighbors()
    //{
    //    Vector3 centerPos = Vector3.zero;

    //    if (neighbors.Count == 0)
    //        return centerPos;

    //    //�߂��̌Q�̒��S�ɋ߂Â��x�N�g�������߂�
    //    foreach (GameObject neighbor in neighbors)
    //        //�I�u�W�F�N�g���j������Ă��Ȃ��ꍇ
    //        if (neighbor != null)
    //            centerPos += neighbor.transform.position;

    //    centerPos /= neighbors.Count;

    //    //���S�����֌������͂�Ԃ�
    //    return (centerPos - myTransform.position).normalized;
    //}
    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// �S�Ă̕ǂł̎~�߂悤�Ƃ���͂��܂Ƃ߂ĕԂ�
    /// </summary>
    /// <returns>�͈͓�����o�Ȃ��悤�ɂ����</returns>
    //private Vector3 StayWithinRange()
    //{
    //    //�͈͂���o�Ȃ��悤�ɂ����
    //    Vector3 stayRangeForce = Vector3.zero;

    //    //�S�̕ǂ���̎~�߂悤�Ƃ���͂��܂Ƃ߂�
    //    stayRangeForce = RebelAgainstWall(stageScale - myTransform.position.x, Vector3.left)
    //                   + RebelAgainstWall(stageScale - myTransform.position.z, Vector3.back)
    //                   + RebelAgainstWall(-stageScale - myTransform.position.x, Vector3.right)
    //                   + RebelAgainstWall(-stageScale - myTransform.position.z, Vector3.forward);

    //    return stayRangeForce;
    //}

    /// <summary>
    /// �ǂ̓����Ɏ~�߂悤�Ƃ���͂�Ԃ�
    /// </summary>
    /// <param name="toWallDistance">�ǂƂ̋���</param>
    /// <param name="fromWallDirection">�ǂ̓�������</param>
    /// <returns>�ǂ̓����Ɏ~�߂悤�Ƃ����</returns>
    //private Vector3 RebelAgainstWall(float toWallDistance, Vector3 fromWallDirection)
    //{
    //    //�ǂƔ��Ε����̗�
    //    Vector3 restitutionForce = Vector3.zero;

    //    //�ǂƂ̋���������l��菬�����Ȃ����ꍇ
    //    if (Mathf.Abs(toWallDistance) < enemyManager.enemyData.restitutionDistance)
    //    {
    //        //�ǂ̓��������̗͂�������
    //        //�ǂƂ̋������߂��قǗ͂͑傫���Ȃ�
    //        restitutionForce = fromWallDirection * 
    //            (enemyManager.enemyData.restitutionCoefficient / 
    //            (Mathf.Abs(toWallDistance / enemyManager.enemyData.restitutionDistance)));
    //    }

    //    return restitutionForce;
    //}


    //---------------------------------------------------------------------------
    //�v���C���[�̕����֌������͂�Ԃ�
    //�ʒu�A�v���C���[�̈ʒu�A�W��
    //private Vector3 CombinePlayer(float coefficient)
    //{
    //    //�v���C���[�̈ʒu���擾����
    //    Vector3 playerPos = playerTransform.position;

    //    //�v���C���[�܂ł̃x�N�g��
    //    Vector3 toPlayerVec = playerPos - myTransform.position;

    //    //�v���C���[�Ɍ��������Ƃ����
    //    Vector3 combineForce = coefficient * (toPlayerVec - velocity).normalized;

    //    return combineForce;
    //}

    //���p���̃v���C���[�̕����֌������͂�Ԃ�
    //private Vector3 CombineNearbyPlayer(float innerProductThred)
    //{
    //    //�v���C���[�Ɍ��������Ƃ����
    //    Vector3 combineForce = Vector3.zero;

    //    //�v���C���[�̈ʒu���擾����
    //    Vector3 playerPos = playerTransform.position;

    //    //�v���C���[�܂ł̃x�N�g��
    //    Vector3 toPlayerVec = playerPos - myTransform.position;

    //    //�v���C���[�Ƃ̋������ݒ肵���l�ȉ��̏ꍇ
    //    float sqrDistance = playerPos.sqrMagnitude;
    //    if (sqrDistance <= Mathf.Pow(enemyManager.enemyData.ditectingNeiborDistance, 2))
    //    {
    //        Vector3 direction = playerPos.normalized;
    //        Vector3 forward = velocity.normalized;

    //        float innerProduct = Vector3.Dot(forward, direction);
    //        if (innerProduct >= innerProductThred)
    //        {
    //            //�v���C���[�̕�����Ԃ�
    //            combineForce = toPlayerVec.normalized;
    //        }
    //    }
    //    return combineForce;
    //}
}
