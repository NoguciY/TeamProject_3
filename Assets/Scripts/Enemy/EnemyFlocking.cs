using System.Collections.Generic;
using UnityEngine;

//群を検知して、進行方向を決めて移動する
//群を検知する距離、接近を許す距離、敵の速さ、は、
//敵ごとに変えたい

public class EnemyFlocking : MonoBehaviour
{
    //近隣の個体を格納する
    [SerializeField]
    private List<GameObject> neighbors;

    //移動処理に使う速度
    [SerializeField]
    private Vector3 velocity;

    private Transform myTransform;

    //速さ
    private float speed;

    //加速度
    private Vector3 acceleration;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="minSpeed">速さの下限</param>
    /// <param name="maxSpeed">速さの上限</param>
    public void Init(float minSpeed, float maxSpeed)
    {
        myTransform = transform;

        speed = Random.Range(minSpeed, maxSpeed);

        velocity = speed * Vector3.forward;
    }

    /// <summary>
    /// 近隣の仲間を探してリストに追加
    /// </summary>
    /// <param name="flockManager">敵を生成するコンポーネント</param>
    /// <param name="index">敵の登場順</param>
    /// <param name="ditectingNeiborDistance">近隣の個体を検知する距離</param>
    /// <param name="innerProductThred">視野角をもとにした内積</param>
    public void AddNeighbors(EnemySpawner flockManager, int index, float ditectingNeiborDistance, float innerProductThred)
    {
        //リストをクリア
        neighbors.Clear();

        foreach(var boid in flockManager.boids[index])
        {
            //自身以外のfloackMangerを持ったオブジェクト
            if(boid != this.gameObject)
            {
                //自身から仲間のベクトル
                Vector3 toOtherVec = boid.transform.position - this.transform.position;
                
                //magnitudeより処理が速いためsqrMagnitudeを使う
                float sqrDistance = toOtherVec.sqrMagnitude;

                //自身と仲間の距離がditectingNeiborDistance以下の場合
                if (sqrDistance <= Mathf.Pow(ditectingNeiborDistance, 2))
                {
                    //自身から仲間の方向と自身の正面方向
                    Vector3 direction = toOtherVec.normalized;
                    Vector3 forward = velocity.normalized;

                    //内積を使い視野角内にいる仲間のみをリストに追加する
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
    /// 力をまとめて速度として返す
    /// </summary>
    /// <param name="playerTransform">プレイヤーのTransform</param>
    /// <param name="enemyData">敵の情報</param>
    /// <returns>移動速度</returns>
    public Vector3 Move(Transform playerTransform, EnemySetting.EnemyData enemyData)
    {
        //フレームごとに速度に加算する値(加速度)を求める
        acceleration = SeparateNeighbors(enemyData.separationCoefficient)
                     + AlignNeighbors(enemyData.alignmentCoefficient)
                     + CombineNeighbors(enemyData.combiningCoefficient)
                     + CombinePlayer(playerTransform, enemyData.combiningPlayerCoefficient);

        //視野角内のプレイヤーに向かう力
        //+ CombineNearbyPlayer() * combiningPlayerCoefficient
        //フィールド内にとどめる力(壁に近づくと離れる)
        //+ StayWithinRange();

        velocity += acceleration * Time.deltaTime;

        float speed = velocity.magnitude;
        Vector3 direction = velocity.normalized;

        //速度を制限する
        velocity = Mathf.Clamp(speed, enemyData.minSpeed, enemyData.maxSpeed) * direction;

        //移動方向を正面方向にするために回転する
        if (direction != Vector3.zero)
        {
            //上下に回転しないようにする
            direction.y = 0;

            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                        Quaternion.LookRotation(direction),
                                        enemyData.rotationSpeed * Time.deltaTime);
        }
        
        //次のフレームに備えてリセット
        acceleration = Vector3.zero;

        return velocity;
    }

    /// <summary>
    /// 近隣の個体から離れる(分離する)力を返す
    /// </summary>
    /// <param name="coefficient">分離する力にかける係数</param>
    /// <returns>近隣の群れから離れる力</returns>
    private Vector3 SeparateNeighbors(float coefficient)
    {
        Vector3 avoidanceForce = Vector3.zero;

        if (neighbors.Count == 0) return avoidanceForce;

        //近隣の群から離れるベクトルを返す
        foreach (GameObject neighbor in neighbors)
        {
            //オブジェクトが破棄されてない場合
            if (neighbor != null)
            {
                avoidanceForce += myTransform.position - neighbor.transform.position;
            }
        }

        return coefficient * avoidanceForce.normalized;
    }

    /// <summary>
    /// 近隣の群に合わせて整列させる方向の力を返す
    /// </summary>
    /// <param name="coefficient">整列する力にかける係数</param>
    /// <returns>群と整列させようとする力</returns>
    private Vector3 AlignNeighbors(float coefficient)
    {
        Vector3 averageVelocity = Vector3.zero;

        if (neighbors.Count == 0) return averageVelocity;

        //近隣の群の平均速度の計算
        foreach (GameObject neighbor in neighbors)
        {
            //オブジェクトが破棄されていない場合
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
    /// 近隣の群の中心に近づく(結合する)力を返す
    /// </summary>
    /// <param name="coefficient">結合する力にかける係数</param>
    /// <returns>群と結合する力</returns>
    private Vector3 CombineNeighbors(float coefficient)
    {
        Vector3 centerPos = Vector3.zero;

        if (neighbors.Count == 0) return centerPos;

        //群の中心位置の計算
        foreach (GameObject neighbor in neighbors)
        {
            //オブジェクトが破棄されていない場合
            if (neighbor != null)
            {
                centerPos += neighbor.transform.position;
            }
        }
        centerPos /= neighbors.Count;

        //中心方向へ向かう力を返す
        return coefficient * (centerPos - myTransform.position).normalized;
    }

    /// <summary>
    /// プレイヤー方向へ向かう力を返す
    /// </summary>
    /// <param name="playerTransform">プレイヤーのTransform</param>
    /// <param name="coefficient">プレイヤー方向に向かう力にかける係数</param>
    /// <returns></returns>
    private Vector3 CombinePlayer(Transform playerTransform, float coefficient)
    {
        Vector3 combineForce = Vector3.zero;

        if(playerTransform == null) return combineForce;

        //プレイヤーの位置を取得する
        Vector3 playerPos = playerTransform.position;

        //プレイヤーに向かう力
        combineForce = playerPos - myTransform.position;

        return coefficient * combineForce.normalized;
    }
}
