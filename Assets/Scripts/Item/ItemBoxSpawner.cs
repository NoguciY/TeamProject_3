using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxSpawner : MonoBehaviour
{
    //経験値リスト


    [SerializeField, Header("スポーンさせるオブジェクト")]
    private GameObject spawnObject;

    [SerializeField, Header("プレイヤーのTransform")]
    private Transform playerTransform;

    [SerializeField, Header("スポーン範囲の半径")]
    private float spawnRadius; 

    [SerializeField,Header("スポーン間隔")]
    private float spawnInterval;
    void Start()
    {
        InvokeRepeating("SpawnObjects", spawnInterval, spawnInterval);
    }

    void SpawnObjects()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = playerTransform.position.y;  // 高さをプレイヤーと同じにする
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }
}
