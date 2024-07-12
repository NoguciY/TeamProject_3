using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Setting", fileName = "EnemySetting")]
public class EnemySetting : ScriptableObject
{
    //“G‚Ìí—Ş‚²‚Æ‚Éƒf[ƒ^‚ğ‚Â‚½‚ß‚ÌƒŠƒXƒg
    public List<EnemyData> enemyDataList;

    //“G‚P‘Ì•ª‚Ìƒf[ƒ^
    [Serializable]
    public class EnemyData
    {
        [Header("–¼‘O")]
        public string name;

        [Header("“oê‡")]
        public int generationOrder;

        [Header("‹ß—×‚ÌŒÂ‘Ì‚ğŒŸ’m‚·‚é‹——£")]
        public float ditectingNeiborDistance;

        [Header("‹–ìŠp")]
        public float fieldOfView;

        [Header("‰ñ“]‘¬“x")]
        public float rotationSpeed;

        [Header("•ª—£ŒW”")]
        public float separationCoefficient;

        [Header("®—ñŒW”")]
        public float alignmentCoefficient;

        [Header("Œ‹‡ŒW”")]
        public float combiningCoefficient;

        [Header("ƒvƒŒƒCƒ„[‚ÉŒ‹‡‚·‚é—Í‚ÌŒW”")]
        public float combiningPlayerCoefficient;

        [Header("Å¬‘¬“x")]
        public float minSpeed;

        [Header("Å‘å‘¬“x")]
        public float maxSpeed;

        [Header("Å‘å‘Ì—Í")]
        public float maxHealth;

        [Header("UŒ‚—Í")]
        public float attack;
    }
}
