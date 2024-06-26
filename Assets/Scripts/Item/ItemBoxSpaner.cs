using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBoxSpaner : MonoBehaviour
{
    [SerializeField, Header("スポーンさせるオブジェクト")]
    private GameObject spawnObject;

    [SerializeField, Header("プレイヤーのTransform")]
    private Transform playerTransform;

    [SerializeField, Header("スポーン範囲の半径")]
    private float spawnRadius = 5f; 

    [SerializeField,Header("スポーン間隔")]
    private float spawnInterval = 2f;
    void Start()
    {
        InvokeRepeating("SpawnObjects", spawnInterval, spawnInterval);
    }

    void SpawnObjects()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = playerTransform.position.y + 5;  // 高さをプレイヤーと同じにする
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }
}
