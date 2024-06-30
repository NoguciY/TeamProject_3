//using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

//範囲、数を決めて敵を生成する

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのTransform")]
    private Transform playerTransform;

    //敵の情報
    [SerializeField]
    private EnemySetting enemySetting;

    [SerializeField, Header("生成する敵")]
    private GameObject[] enemyPrefab;

    [SerializeField,Header("生成する数")]
    private int generatedNum;

    [SerializeField, Header("生成間隔(秒)")]
    private float generatedInterval;

    //[SerializeField, Header("生成する秒数と敵名")]
    //private Dictionary<float, string> generatedEnemyDictionary;

    [SerializeField, Header("円状に生成する際の半径")]
    private float radius;

    [SerializeField, Header("キノコを円状に生成する際の半径")]
    private float radiusMushroom;

    //インスタンス化した群の個体を格納する
    public List<GameObject> boids;

    private Transform myTransform;

    //円状に生成する際の生成する高さ
    private float spawnHeight;

    //前回の間隔
    private float beforeInterval;

    //敵の名前とプレハブがセットのディクショナリー
    private Dictionary<string, GameObject> enemyDictionary;

    //円状に生成されたか
    private bool isGeneratedInCircle;

    //1周期
    private const float ONECYCLE = 2.0f * Mathf.PI;

    private void Awake()
    {
        enemyDictionary = new Dictionary<string, GameObject>();

        //敵の名前とプレハブをセットする
        foreach (GameObject enemy in enemyPrefab)
            enemyDictionary.Add(enemy.name, enemy);
    }

    private void Start()
    {
        myTransform = transform;

        //敵の生成する高さを初期のプレイヤーの高さにする
        spawnHeight = playerTransform.position.y;
        beforeInterval = 0;
        isGeneratedInCircle = false;
    }

    private void Update()
    {
        //経過時間
        float elapsedTime = GameManager.Instance.GetDeltaTimeInMain;

        //間隔 = 経過時間 / 生成間隔 の余り
        float interval = elapsedTime % generatedInterval;

        //設定した秒数毎に実行する
        if (interval < beforeInterval)
            //生成間隔ごとに生成数を増やす
            generatedNum += 3;

        beforeInterval = interval;

        if (elapsedTime < 120)
        {
            //設定した数の敵をプレイヤーを中心に円状に生成する
            GenerateEnemy("RedBlob");
        }
        else if (elapsedTime >= 120)
        {
            GenerateEnemy("Orc");
        }

        if (elapsedTime >= 180)
        {
            //いきなりプレイヤーを中心に囲む
            GenerateEnemyInCircle("Mushroom");
        }
    }

    //指定した敵の情報を返す
    private EnemySetting.EnemyData GetEnemyData(string name)
    {
        foreach (EnemySetting.EnemyData enemyData in enemySetting.enemyDataList)
        {
            if (enemyData.name == name)
                return enemyData;
        }

        return null;
    }

    /// <summary>
    /// 円状に敵を生成する
    /// 常に設定した数がフィールドにいるようにする
    /// </summary>
    /// <param name="name">敵の名前</param>
    private void GenerateEnemy(string name)
    {
        //生成する敵を取得する
        GameObject generatedEnemy = GetEnemyObjectFromName(name);

        //現在の生成数が設定した生成数より小さい場合
        if (boids.Count < generatedNum)
        {
            Vector3 playerPos = playerTransform.position;

            //ランダムな値
            int randomPoint = Random.Range(0, generatedNum);

            //周期の位置 (1.0 = 100% の時 2π となる)
            float point = ((float)randomPoint / generatedNum) * ONECYCLE;

            float spawnPosX = Mathf.Cos(point) * radius + playerPos.x;
            float spawnPosZ = Mathf.Sin(point) * radius + playerPos.z;

            //生成位置
            var spawnPos = new Vector3(spawnPosX, spawnHeight, spawnPosZ);

            //敵を生成する
            GameObject boid = Instantiate(generatedEnemy, spawnPos, Quaternion.identity, myTransform);

            //生成した敵のenemyManagerの変数に値を渡す
            var enemyManager = boid.GetComponent<EnemyManager>();
            enemyManager.flockManager = this;
            enemyManager.enemyData = GetEnemyData(name);
            enemyManager.playerTransform = playerTransform;

            //リストに追加
            boids.Add(boid);
        }
    }

    private void GenerateEnemyInCircle(string name)
    {
        //生成する敵を取得する
        GameObject generatedEnemy = GetEnemyObjectFromName(name);

        Vector3 playerPos = playerTransform.position;

        if (!isGeneratedInCircle)
        {
            for (int i = 0; i < generatedNum; i++)
            {
                //周期の位置 (1.0 = 100% の時 2π となる)
                float point = ((float)i / generatedNum) * ONECYCLE;

                float spawnPosX = Mathf.Cos(point) * radiusMushroom + playerPos.x;
                float spawnPosZ = Mathf.Sin(point) * radiusMushroom + playerPos.z;

                //生成位置
                var spawnPos = new Vector3(spawnPosX, spawnHeight, spawnPosZ);

                //敵を生成する
                GameObject boid = Instantiate(generatedEnemy, spawnPos, Quaternion.identity, myTransform);

                //生成した敵のenemyManagerの変数に値を渡す
                var enemyManager = boid.GetComponent<EnemyManager>();
                enemyManager.flockManager = this;
                enemyManager.enemyData = GetEnemyData(name);
                enemyManager.playerTransform = playerTransform;

                //リストに追加
                boids.Add(boid);
            }

            isGeneratedInCircle = true;
        }
    }

    /// <summary>
    /// 名前からディクショナリーに格納された敵を取得する
    /// </summary>
    /// <param name="name">敵の名前</param>
    /// <returns>敵のゲームオブジェクト</returns>
    private GameObject GetEnemyObjectFromName(string name)
    {
        GameObject generatedEnemy = null;

        //ディクショナリーに格納された敵の名前からプレハブを取得する
        if (enemyDictionary.TryGetValue(name, out var enemyPrefab))
            generatedEnemy = enemyPrefab;
        else
            Debug.LogError($"ディクショナリーに{name}が登録されていません");

        return generatedEnemy;
    }
}
