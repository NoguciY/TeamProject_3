using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//敵はフロッキングアルゴリズムを持っている
//フロッキングアルゴリズムには近隣の群、敵の位置、敵の速度が必要
//コンストラクタで取得する

public class EnemyFlock : MonoBehaviour
{
    //近くの群を格納するリスト
    private List<GameObject> neighbors;

    private void AddNeighbors(float fieldOfView, EnemyFlockManager flockManager, float ditectingNeiborDistance, Vector3 velocity)
    {
        //リストをクリア
        neighbors.Clear();

        //視野角を内積の閾値に使う
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

    //群から分離する方向の力を返す
    private Vector3 SeparateNeighbors(Vector3 pos, Vector3 neighborPos)
    {
        Vector3 avoidanceForce = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return avoidanceForce;
        }

        //近くの群から離れるベクトルを求める
        foreach (GameObject neighbor in neighbors)
        {
            avoidanceForce += (pos - neighborPos);
        }

        return avoidanceForce.normalized;
    }

    //群と整列する方向の力を返す
    private Vector3 AlignNeighbors(Vector3 neighborVelocity)
    {
        Vector3 averageVelocity = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return averageVelocity;
        }

        //近くの群の平均速度を求める
        foreach (GameObject neighbor in neighbors)
        {
            averageVelocity += neighborVelocity;
        }
        averageVelocity /= neighbors.Count;

        return averageVelocity.normalized;
    }

    //群に結合する方向の力を返す
    private Vector3 CombineNeighbors(Vector3 pos, Vector3 neighborPos)
    {
        Vector3 centerPos = Vector3.zero;

        if (neighbors.Count == 0)
        {
            return centerPos;
        }

        //近くの群の中心に近づくベクトルを求める
        foreach (GameObject neighbor in neighbors)
        {
            centerPos += neighborPos;
        }
        centerPos /= neighbors.Count;

        //中心方向へ向かう力を返す
        return (centerPos - pos).normalized;
    }
}
