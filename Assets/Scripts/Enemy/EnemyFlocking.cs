using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//群を検知して、進行方向を決めて移動する
//群を検知する距離、接近を許す距離、敵の速さ、は、
//敵ごとに変えたい

public class EnemyFlocking : MonoBehaviour
{
    private Transform myTransform;

    //近隣の個体を格納する
    [SerializeField]
    private List<GameObject> neighbors;

    //速さ
    private float speed;

    //移動処理に使う速度
    [SerializeField]
    private Vector3 velocity;

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

        //stageScale = Utilities.STAGESIZE * 0.5f;

        speed = Random.Range(minSpeed, maxSpeed);

        velocity = speed * Vector3.forward;
    }

    /// <summary>
    /// 近隣の仲間を探してリストに追加
    /// </summary>
    /// <param name="flockManager">敵を生成するコンポーネント</param>
    /// <param name="ditectingNeiborDistance">近隣の個体を検知する距離</param>
    /// <param name="innerProductThred">視野角をもとにした内積</param>
    public void AddNeighbors(EnemySpawner flockManager, float ditectingNeiborDistance, float innerProductThred)
    {
        //リストをクリア
        neighbors.Clear();

        foreach(var boid in flockManager.boids)
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
                    if(innerProduct >= innerProductThred) 
                        neighbors.Add(boid);
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

        if (neighbors.Count == 0)
            return avoidanceForce;

        //近隣の群から離れるベクトルを返す
        foreach (GameObject neighbor in neighbors)
            //オブジェクトが破棄されてない場合
            if (neighbor != null)
                avoidanceForce += myTransform.position - neighbor.transform.position;

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

        if (neighbors.Count == 0)
            return averageVelocity;

        //近隣の群の平均速度の計算
        foreach (GameObject neighbor in neighbors)
        {
            //オブジェクトが破棄されていない場合
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
    /// 近隣の群の中心に近づく(結合する)力を返す
    /// </summary>
    /// <param name="coefficient">結合する力にかける係数</param>
    /// <returns>群と結合する力</returns>
    private Vector3 CombineNeighbors(float coefficient)
    {
        Vector3 centerPos = Vector3.zero;

        if (neighbors.Count == 0)
            return centerPos;

        //群の中心位置の計算
        foreach (GameObject neighbor in neighbors)
        {
            //オブジェクトが破棄されていない場合
            if (neighbor != null)
                centerPos += neighbor.transform.position;
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

        if(playerTransform == null)
            return combineForce;

        //プレイヤーの位置を取得する
        Vector3 playerPos = playerTransform.position;

        //プレイヤーに向かう力
        combineForce = playerPos - myTransform.position;

        return coefficient * combineForce.normalized;
    }

    //力をまとめて移動する
    //private void UpdateMove()
    //{
    //    //フレームごとに速度に加算する値(加速度)を求める
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

    //    //速度を制限する
    //    velocity = Mathf.Clamp(speed, enemyManager.enemyData.minSpeed, enemyManager.enemyData.maxSpeed) * direction;

    //    if (direction != Vector3.zero)
    //    {
    //        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
    //                             Quaternion.LookRotation(direction),        //向かせたい方向
    //                             enemyManager.enemyData.rotationSpeed * Time.deltaTime);
    //    }
    //    if (!float.IsNaN(velocity.x) && !float.IsNaN(velocity.y) && !float.IsNaN(velocity.z))
    //    {
    //        myTransform.position += velocity * Time.deltaTime;
    //    }
    //    acceleration = Vector3.zero;
    //}

    //群から分離する方向の力を返す
    //private Vector3 SeparateNeighbors()
    //{
    //    Vector3 avoidanceForce = Vector3.zero;

    //    if (neighbors.Count <= 0)
    //        return avoidanceForce;

    //    //近くの群から離れるベクトルを求める
    //    foreach (GameObject neighbor in neighbors)
    //    {
    //        //オブジェクトが破棄されていない場合
    //        if (neighbor != null)
    //            avoidanceForce += (myTransform.position - neighbor.transform.position);
    //    }

    //    return avoidanceForce.normalized;
    //}



    //群と整列する方向の力を返す
    //private Vector3 AlignNeighbors()
    //{
    //    Vector3 averageVelocity = Vector3.zero;

    //    if (neighbors.Count == 0)
    //        return averageVelocity;

    //    //近くの群の平均速度を求める
    //    foreach (GameObject neighbor in neighbors)
    //    {
    //        //オブジェクトが破棄されていない場合
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

 
    
    //群に結合する方向の力を返す
    //private Vector3 CombineNeighbors()
    //{
    //    Vector3 centerPos = Vector3.zero;

    //    if (neighbors.Count == 0)
    //        return centerPos;

    //    //近くの群の中心に近づくベクトルを求める
    //    foreach (GameObject neighbor in neighbors)
    //        //オブジェクトが破棄されていない場合
    //        if (neighbor != null)
    //            centerPos += neighbor.transform.position;

    //    centerPos /= neighbors.Count;

    //    //中心方向へ向かう力を返す
    //    return (centerPos - myTransform.position).normalized;
    //}
    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// 全ての壁での止めようとする力をまとめて返す
    /// </summary>
    /// <returns>範囲内から出ないようにする力</returns>
    //private Vector3 StayWithinRange()
    //{
    //    //範囲から出ないようにする力
    //    Vector3 stayRangeForce = Vector3.zero;

    //    //４つの壁からの止めようとする力をまとめる
    //    stayRangeForce = RebelAgainstWall(stageScale - myTransform.position.x, Vector3.left)
    //                   + RebelAgainstWall(stageScale - myTransform.position.z, Vector3.back)
    //                   + RebelAgainstWall(-stageScale - myTransform.position.x, Vector3.right)
    //                   + RebelAgainstWall(-stageScale - myTransform.position.z, Vector3.forward);

    //    return stayRangeForce;
    //}

    /// <summary>
    /// 壁の内側に止めようとする力を返す
    /// </summary>
    /// <param name="toWallDistance">壁との距離</param>
    /// <param name="fromWallDirection">壁の内側方向</param>
    /// <returns>壁の内側に止めようとする力</returns>
    //private Vector3 RebelAgainstWall(float toWallDistance, Vector3 fromWallDirection)
    //{
    //    //壁と反対方向の力
    //    Vector3 restitutionForce = Vector3.zero;

    //    //壁との距離がある値より小さくなった場合
    //    if (Mathf.Abs(toWallDistance) < enemyManager.enemyData.restitutionDistance)
    //    {
    //        //壁の内側方向の力を加える
    //        //壁との距離が近いほど力は大きくなる
    //        restitutionForce = fromWallDirection * 
    //            (enemyManager.enemyData.restitutionCoefficient / 
    //            (Mathf.Abs(toWallDistance / enemyManager.enemyData.restitutionDistance)));
    //    }

    //    return restitutionForce;
    //}


    //---------------------------------------------------------------------------
    //プレイヤーの方向へ向かう力を返す
    //位置、プレイヤーの位置、係数
    //private Vector3 CombinePlayer(float coefficient)
    //{
    //    //プレイヤーの位置を取得する
    //    Vector3 playerPos = playerTransform.position;

    //    //プレイヤーまでのベクトル
    //    Vector3 toPlayerVec = playerPos - myTransform.position;

    //    //プレイヤーに向かおうとする力
    //    Vector3 combineForce = coefficient * (toPlayerVec - velocity).normalized;

    //    return combineForce;
    //}

    //視角内のプレイヤーの方向へ向かう力を返す
    //private Vector3 CombineNearbyPlayer(float innerProductThred)
    //{
    //    //プレイヤーに向かおうとする力
    //    Vector3 combineForce = Vector3.zero;

    //    //プレイヤーの位置を取得する
    //    Vector3 playerPos = playerTransform.position;

    //    //プレイヤーまでのベクトル
    //    Vector3 toPlayerVec = playerPos - myTransform.position;

    //    //プレイヤーとの距離が設定した値以下の場合
    //    float sqrDistance = playerPos.sqrMagnitude;
    //    if (sqrDistance <= Mathf.Pow(enemyManager.enemyData.ditectingNeiborDistance, 2))
    //    {
    //        Vector3 direction = playerPos.normalized;
    //        Vector3 forward = velocity.normalized;

    //        float innerProduct = Vector3.Dot(forward, direction);
    //        if (innerProduct >= innerProductThred)
    //        {
    //            //プレイヤーの方向を返す
    //            combineForce = toPlayerVec.normalized;
    //        }
    //    }
    //    return combineForce;
    //}
}
