//using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

//�͈́A�������߂ēG�𐶐�����

public class EnemyFlockManager : MonoBehaviour
{
    //�v���C���[�R���|�[�l���g
    [SerializeField]
    private Player player;

    //��������G
    [SerializeField]
    private GameObject enemy;

    //��������ŏ��͈�
    [SerializeField]
    private float generatedMinRange;

    //��������ő�͈�
    [SerializeField]
    private float generatedMaxRange;

    //�������鐔
    [SerializeField]
    private int generatedNum;

    [SerializeField, Header("�����Ԋu(�b)")]
    private float generatedInterval;

    // �~�̒��S�_
    [SerializeField, Header("�~��ɐ�������ۂ̒��S���W")]
    private Vector3 centerPos;

    // �~�̔��a
    [SerializeField, Header("�~��ɐ�������ۂ̔��a")]
    private float radius;

    //�C���X�^���X�������Q�̌̂��i�[����
    public List<GameObject> boids = new List<GameObject>();

    //�O��̊Ԋu
    private float beforeInterval;


    float oneCycle = 2.0f * Mathf.PI; // sin �̎����� 2��


    private void Awake()
    {
        //�G�������_���Ȉʒu�ɐ�������
        //AddBoid();
        //GenerateEnemiesInCircle(enemy, radius, generatedNum);
    }

    private void Update()
    {
        //�Ԋu = �o�ߎ��� / �����Ԋu �̗]��
        float interval = GameManager.Instance.GetDeltaTimeInMain % generatedInterval;

        //�ݒ肵���b�����ɌJ��Ԃ�
        if (interval < beforeInterval)
        {
            //GenerateEnemiesInCircle(enemy, radius, generatedNum);
            
            //�����Ԋu���Ƃɐ�������10���₷
            generatedNum += 10;
        }
        beforeInterval = interval;

        //AddBoid();

        //��ɐݒ肵�������v���C���[�𒆐S�ɉ~��ɐ�������
        AddBoidInCircle();
    }

    /// <summary>
    /// //�G�������_���Ȉʒu�ɐ�������
    /// ���X�g�ɐ����������ǉ�����
    /// </summary>
    private void AddBoid()
    {
        Transform parent = this.transform;

        while (boids.Count < generatedNum)
        {
            //�����_���Ȉʒu
            float randomPosX = Random.Range(generatedMinRange, generatedMaxRange);
            float randomPosZ = Random.Range(generatedMinRange, generatedMaxRange);
            Vector3 generationPos = new Vector3(randomPosX, 0, randomPosZ);

            //�G�̐���
            GameObject boid = Instantiate(enemy, generationPos, Quaternion.identity, parent);
            boid.GetComponent<EnemyFlocking>().flockManager = this;
            
            //���X�g�ɒǉ�
            boids.Add(boid);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="generatedPrefab">��������I�u�W�F�N�g</param>
    /// <param name="radius">��������͈͂̔��a</param>
    /// <param name="centerPos">�������钆�S���W</param>
    /// <param name="generatedNum">������</param>
    private void GenerateEnemies(GameObject generatedPrefab, float radius, Vector3 centerPos, int generatedNum)
    {
        Transform parent = this.transform;

        if (generatedNum <= 0) return;

        //�G�𐶐�����
        for (int i = 0; i < generatedNum; i++)
        {
            //�w�肳�ꂽ���a�̉~���̃����_���ʒu���擾
            Vector3 circlePos = radius * Random.insideUnitCircle;

            //XZ���ʂŎw�肳�ꂽ���a�A���S�_�̉~���̃����_���ʒu���v�Z
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + centerPos;

            //�I�u�W�F�N�g�̐���
            GameObject boid = Instantiate(generatedPrefab, spawnPos, Quaternion.identity, parent);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //���X�g�ɒǉ�
            boids.Add(boid);
        }
    }

    /// <summary>
    /// �~��ɃI�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="generatedPrefab">��������I�u�W�F�N�g</param>
    /// <param name="radius">�������锼�a</param>
    /// <param name="generatedNum">�������鐔</param>
    private void GenerateEnemiesInCircle(GameObject generatedPrefab, float radius, int generatedNum)
    {
        for (int i = 0; i < generatedNum; ++i)
        {
            // �����̈ʒu (1.0 = 100% �̎� 2�� �ƂȂ�)
            float point = ((float)i / generatedNum) * oneCycle; 

            float x = Mathf.Cos(point) * radius + centerPos.x;
            float z = Mathf.Sin(point) * radius + centerPos.z;

            var spawnPos = new Vector3(x, centerPos.y, z);

            GameObject boid = Instantiate(generatedPrefab,spawnPos,Quaternion.identity,transform);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //���X�g�ɒǉ�
            boids.Add(boid);
        }
    }

    //�~��ɓG�𐶐�����
    //��ɐݒ肵�������t�B�[���h�ɂ���悤�ɂ���
    private void AddBoidInCircle()
    {
        //���������ݒ肵����������菬�����ꍇ
        if (boids.Count < generatedNum)
        {
            Vector3 playerPos = player.transform.position;

            //�����_���Ȑ�
            int randomPoint = Random.Range(0, generatedNum);

            //�����̈ʒu (1.0 = 100% �̎� 2�� �ƂȂ�)
            float point = ((float)randomPoint / generatedNum) * oneCycle;

            float x = Mathf.Cos(point) * radius + playerPos.x;
            float z = Mathf.Sin(point) * radius + playerPos.z;

            //�����ʒu
            var spawnPos = new Vector3(x, centerPos.y, z);

            GameObject boid = Instantiate(enemy, spawnPos, Quaternion.identity, transform);
            boid.GetComponent<EnemyFlocking>().flockManager = this;

            //���X�g�ɒǉ�
            boids.Add(boid);
        }
    }
}
