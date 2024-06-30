//using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

//�͈́A�������߂ēG�𐶐�����

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Header("�v���C���[��Transform")]
    private Transform playerTransform;

    //�G�̏��
    [SerializeField]
    private EnemySetting enemySetting;

    [SerializeField, Header("��������G")]
    private GameObject[] enemyPrefab;

    [SerializeField,Header("�������鐔")]
    private int generatedNum;

    [SerializeField, Header("�����Ԋu(�b)")]
    private float generatedInterval;

    //[SerializeField, Header("��������b���ƓG��")]
    //private Dictionary<float, string> generatedEnemyDictionary;

    [SerializeField, Header("�~��ɐ�������ۂ̔��a")]
    private float radius;

    [SerializeField, Header("�L�m�R���~��ɐ�������ۂ̔��a")]
    private float radiusMushroom;

    //�C���X�^���X�������Q�̌̂��i�[����
    public List<GameObject> boids;

    private Transform myTransform;

    //�~��ɐ�������ۂ̐������鍂��
    private float spawnHeight;

    //�O��̊Ԋu
    private float beforeInterval;

    //�G�̖��O�ƃv���n�u���Z�b�g�̃f�B�N�V���i���[
    private Dictionary<string, GameObject> enemyDictionary;

    //�~��ɐ������ꂽ��
    private bool isGeneratedInCircle;

    //1����
    private const float ONECYCLE = 2.0f * Mathf.PI;

    private void Awake()
    {
        enemyDictionary = new Dictionary<string, GameObject>();

        //�G�̖��O�ƃv���n�u���Z�b�g����
        foreach (GameObject enemy in enemyPrefab)
            enemyDictionary.Add(enemy.name, enemy);
    }

    private void Start()
    {
        myTransform = transform;

        //�G�̐������鍂���������̃v���C���[�̍����ɂ���
        spawnHeight = playerTransform.position.y;
        beforeInterval = 0;
        isGeneratedInCircle = false;
    }

    private void Update()
    {
        //�o�ߎ���
        float elapsedTime = GameManager.Instance.GetDeltaTimeInMain;

        //�Ԋu = �o�ߎ��� / �����Ԋu �̗]��
        float interval = elapsedTime % generatedInterval;

        //�ݒ肵���b�����Ɏ��s����
        if (interval < beforeInterval)
            //�����Ԋu���Ƃɐ������𑝂₷
            generatedNum += 3;

        beforeInterval = interval;

        if (elapsedTime < 120)
        {
            //�ݒ肵�����̓G���v���C���[�𒆐S�ɉ~��ɐ�������
            GenerateEnemy("RedBlob");
        }
        else if (elapsedTime >= 120)
        {
            GenerateEnemy("Orc");
        }

        if (elapsedTime >= 180)
        {
            //�����Ȃ�v���C���[�𒆐S�Ɉ͂�
            GenerateEnemyInCircle("Mushroom");
        }
    }

    //�w�肵���G�̏���Ԃ�
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
    /// �~��ɓG�𐶐�����
    /// ��ɐݒ肵�������t�B�[���h�ɂ���悤�ɂ���
    /// </summary>
    /// <param name="name">�G�̖��O</param>
    private void GenerateEnemy(string name)
    {
        //��������G���擾����
        GameObject generatedEnemy = GetEnemyObjectFromName(name);

        //���݂̐��������ݒ肵����������菬�����ꍇ
        if (boids.Count < generatedNum)
        {
            Vector3 playerPos = playerTransform.position;

            //�����_���Ȓl
            int randomPoint = Random.Range(0, generatedNum);

            //�����̈ʒu (1.0 = 100% �̎� 2�� �ƂȂ�)
            float point = ((float)randomPoint / generatedNum) * ONECYCLE;

            float spawnPosX = Mathf.Cos(point) * radius + playerPos.x;
            float spawnPosZ = Mathf.Sin(point) * radius + playerPos.z;

            //�����ʒu
            var spawnPos = new Vector3(spawnPosX, spawnHeight, spawnPosZ);

            //�G�𐶐�����
            GameObject boid = Instantiate(generatedEnemy, spawnPos, Quaternion.identity, myTransform);

            //���������G��enemyManager�̕ϐ��ɒl��n��
            var enemyManager = boid.GetComponent<EnemyManager>();
            enemyManager.flockManager = this;
            enemyManager.enemyData = GetEnemyData(name);
            enemyManager.playerTransform = playerTransform;

            //���X�g�ɒǉ�
            boids.Add(boid);
        }
    }

    private void GenerateEnemyInCircle(string name)
    {
        //��������G���擾����
        GameObject generatedEnemy = GetEnemyObjectFromName(name);

        Vector3 playerPos = playerTransform.position;

        if (!isGeneratedInCircle)
        {
            for (int i = 0; i < generatedNum; i++)
            {
                //�����̈ʒu (1.0 = 100% �̎� 2�� �ƂȂ�)
                float point = ((float)i / generatedNum) * ONECYCLE;

                float spawnPosX = Mathf.Cos(point) * radiusMushroom + playerPos.x;
                float spawnPosZ = Mathf.Sin(point) * radiusMushroom + playerPos.z;

                //�����ʒu
                var spawnPos = new Vector3(spawnPosX, spawnHeight, spawnPosZ);

                //�G�𐶐�����
                GameObject boid = Instantiate(generatedEnemy, spawnPos, Quaternion.identity, myTransform);

                //���������G��enemyManager�̕ϐ��ɒl��n��
                var enemyManager = boid.GetComponent<EnemyManager>();
                enemyManager.flockManager = this;
                enemyManager.enemyData = GetEnemyData(name);
                enemyManager.playerTransform = playerTransform;

                //���X�g�ɒǉ�
                boids.Add(boid);
            }

            isGeneratedInCircle = true;
        }
    }

    /// <summary>
    /// ���O����f�B�N�V���i���[�Ɋi�[���ꂽ�G���擾����
    /// </summary>
    /// <param name="name">�G�̖��O</param>
    /// <returns>�G�̃Q�[���I�u�W�F�N�g</returns>
    private GameObject GetEnemyObjectFromName(string name)
    {
        GameObject generatedEnemy = null;

        //�f�B�N�V���i���[�Ɋi�[���ꂽ�G�̖��O����v���n�u���擾����
        if (enemyDictionary.TryGetValue(name, out var enemyPrefab))
            generatedEnemy = enemyPrefab;
        else
            Debug.LogError($"�f�B�N�V���i���[��{name}���o�^����Ă��܂���");

        return generatedEnemy;
    }
}
