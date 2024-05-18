using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�G�̓t���b�L���O�A���S���Y���������Ă���
//�t���b�L���O�A���S���Y���ɂ͋ߗׂ̌Q�A�G�̈ʒu�A�G�̑��x���K�v
//�R���X�g���N�^�Ŏ擾����

public class EnemyFlock : MonoBehaviour
{
    //�߂��̌Q���i�[���郊�X�g
    private List<GameObject> neighbors;

    private void AddNeighbors(float fieldOfView, EnemyFlockManager flockManager, float ditectingNeiborDistance, Vector3 velocity)
    {
        //���X�g���N���A
        neighbors.Clear();

        //����p����ς�臒l�Ɏg��
        float innerProductThred = Mathf.Cos(fieldOfView * Mathf.Deg2Rad);

        foreach (var boid in flockManager.boids)
        {
            if (boid != this.gameObject)
            {
                Vector3 toOtherVec = boid.transform.position - this.transform.position;
                float sqrDistance = toOtherVec.sqrMagnitude;

                if (sqrDistance <= (ditectingNeiborDistance * ditectingNeiborDistance))
                {
                    Vector3 direction = toOtherVec.normalized;
                    Vector3 forward = velocity.normalized;

                    float innerProduct = Vector3.Dot(forward, direction);
                    if (innerProduct >= innerProductThred)
                    {
                        neighbors.Add(boid);
                    }
                }
            }
        }
    }

    //�Q���番����������̗͂�Ԃ�
    private Vector3 SeparateNeighbors(Vector3 pos, Vector3 neighborPos)
    {
        Vector3 avoidanceForce = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return avoidanceForce;
        }

        //�߂��̌Q���痣���x�N�g�������߂�
        foreach (GameObject neighbor in neighbors)
        {
            avoidanceForce += (pos - neighborPos);
        }

        return avoidanceForce.normalized;
    }

    //�Q�Ɛ��񂷂�����̗͂�Ԃ�
    private Vector3 AlignNeighbors(Vector3 neighborVelocity)
    {
        Vector3 averageVelocity = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return averageVelocity;
        }

        //�߂��̌Q�̕��ϑ��x�����߂�
        foreach (GameObject neighbor in neighbors)
        {
            averageVelocity += neighborVelocity;
        }
        averageVelocity /= neighbors.Count;

        return averageVelocity.normalized;
    }

    //�Q�Ɍ�����������̗͂�Ԃ�
    private Vector3 CombineNeighbors(Vector3 pos, Vector3 neighborPos)
    {
        Vector3 centerPos = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return centerPos;
        }

        //�߂��̌Q�̒��S�ɋ߂Â��x�N�g�������߂�
        foreach (GameObject neighbor in neighbors)
        {
            centerPos += neighborPos;
        }
        centerPos /= neighbors.Count;

        //���S�����֌������͂�Ԃ�
        return (centerPos - pos).normalized;
    }
}
