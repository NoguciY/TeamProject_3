using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Setting", fileName = "EnemySetting")]
public class EnemySetting : ScriptableObject
{
    //敵の種類ごとにデータを持つためのリスト
    public List<EnemyData> enemyDataList;

    //敵１体分のデータ
    [Serializable]
    public class EnemyData
    {
        [Header("名前")]
        public string name;

        [Header("登場順")]
        public int generationOrder;

        [Header("近隣の個体を検知する距離")]
        public float ditectingNeiborDistance;

        [Header("視野角")]
        public float fieldOfView;

        [Header("回転速度")]
        public float rotationSpeed;

        [Header("分離係数")]
        public float separationCoefficient;

        [Header("整列係数")]
        public float alignmentCoefficient;

        [Header("結合係数")]
        public float combiningCoefficient;

        [Header("プレイヤーに結合する力の係数")]
        public float combiningPlayerCoefficient;

        [Header("最小速度")]
        public float minSpeed;

        [Header("最大速度")]
        public float maxSpeed;

        [Header("最大体力")]
        public float maxHealth;

        [Header("攻撃力")]
        public float attack;
    }
}
