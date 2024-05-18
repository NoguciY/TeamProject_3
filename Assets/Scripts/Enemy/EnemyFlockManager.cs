//using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

//範囲、数を決めて敵を生成する

public class EnemyFlockManager : MonoBehaviour
{
    //プレイヤーコンポーネント
    [SerializeField]
    private Player player;

    //生成する敵
    [SerializeField]
    private GameObject enemy;

    //生成する最小範囲
    [SerializeField]
    private float generatedMinRange;

    //生成する最大範囲
    [SerializeField]
    private float generatedMaxRange;

    //生成する数
    [SerializeField]
    private int generatedNum;

    [SerializeField, Header("生成間隔(秒)")]
    private float generatedInterval;

    // 円の中心点
    [SerializeField, Header("円状に生成する際の中心座標")]
    private Vector3 centerPos;

    // 円の半径
    [SerializeField, Header("円状に生成する際の半径")]
    private float radius;

    //インスタンス化した群の個体を格納する
    public List<GameObject> boids = new List<GameObject>();

    //前回の間隔
    private float beforeInterval;


    float oneCycle = 2.0f * Mathf.PI; // sin の周期は 2π


    private void Awake()
    {
        //敵をランダムな位置に生成する
        //AddBoid();
        //GenerateEnemiesInCircle(enemy, radius, generatedNum);
    }

    private void Update()
    {
        //間隔 = 経過時間 / 生成間隔 の余り
        float interval = GameManager.Instance.GetDeltaTimeInMain % generatedInterval;

        //設定した秒数毎に繰り返す
        if (interval < beforeInterval)
        {
            //GenerateEnemiesInCircle(enemy, radius, generatedNum);
            
            //生成間隔ごとに生成数を10増やす
            generatedNum += 10;
        }
        beforeInterval = interval;

        //AddBoid();

        //常に設定した数をプレイヤーを中心に円状に生成する
        AddBoidInCircle();
    }

    /// <summary>
    /// //敵をランダムな位置に生成して
    /// リストに生成した分追加する
    /// </summary>
    private void AddBoid()
    {
        Transform parent = this.transform;

        while (boids.Count < generatedNum)
        {
            //ランダムな位置
            float randomPosX = Random.Range(generatedMinRange, generatedMaxRange);
            float randomPosZ = Random.Range(generatedMinRange, generatedMaxRange);
            Vector3 generationPos = new Vector3(randomPosX, 0, randomPosZ);

            //敵の生成
            GameObject boid = Instantiate(enemy, generationPos, Quaternion.identity, parent);
            boid.GetComponent<EnemyFlocking>().flockManager = this;
            
            //リストに追加
            boids.Add(boid);
        }
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="generatedPrefab">生成するオブジェクト</param>
    /// <param name="radius">生成する範囲の半径</param>
    /// <param name="centerPos">生成する中心座標</param>
    /// <param name="generatedNum">生成数</param>
    private void GenerateEnemies(GameObject generatedPrefab, float radius, Vector3 centerPos, int generatedNum)
    {
        Transform parent = this.transform;

        if (generatedNum <= 0) return;

        //敵を生成する
        for (int i = 0; i < generatedNum; i++)
        {
            //指定された半径の円内のランダム位置を取得
            Vector3 circlePos = radius * Random.insideUnitCircle;

            //XZ平面で指定された半径、中心点の円内のランダム位置を計算
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + centerPos;

            //オブジェクトの生成
            GameObject boid = Instantiate(generatedPrefab, spawnPos, Quaternion.identity, parent);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //リストに追加
            boids.Add(boid);
        }
    }

    /// <summary>
    /// 円状にオブジェクトを生成する
    /// </summary>
    /// <param name="generatedPrefab">生成するオブジェクト</param>
    /// <param name="radius">生成する半径</param>
    /// <param name="generatedNum">生成する数</param>
    private void GenerateEnemiesInCircle(GameObject generatedPrefab, float radius, int generatedNum)
    {
        for (int i = 0; i < generatedNum; ++i)
        {
            // 周期の位置 (1.0 = 100% の時 2π となる)
            float point = ((float)i / generatedNum) * oneCycle; 

            float x = Mathf.Cos(point) * radius + centerPos.x;
            float z = Mathf.Sin(point) * radius + centerPos.z;

            var spawnPos = new Vector3(x, centerPos.y, z);

            GameObject boid = Instantiate(generatedPrefab,spawnPos,Quaternion.identity,transform);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //リストに追加
            boids.Add(boid);
        }
    }

    //円状に敵を生成する
    //常に設定した数がフィールドにいるようにする
    private void AddBoidInCircle()
    {
        //生成数が設定した生成数より小さい場合
        if (boids.Count < generatedNum)
        {
            Vector3 playerPos = player.transform.position;

            //ランダムな数
            int randomPoint = Random.Range(0, generatedNum);

            //周期の位置 (1.0 = 100% の時 2π となる)
            float point = ((float)randomPoint / generatedNum) * oneCycle;

            float x = Mathf.Cos(point) * radius + playerPos.x;
            float z = Mathf.Sin(point) * radius + playerPos.z;

            //生成位置
            var spawnPos = new Vector3(x, centerPos.y, z);

            GameObject boid = Instantiate(enemy, spawnPos, Quaternion.identity, transform);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //リストに追加
            boids.Add(boid);
        }
    }
}
