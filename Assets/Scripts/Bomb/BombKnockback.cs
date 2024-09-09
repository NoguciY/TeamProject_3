using UnityEngine;

//�m�b�N�o�b�N���e
//�v���C���[�̎�������
//���������G�̓m�b�N�o�b�N����

public class BombKnockback : MonoBehaviour
{
    //BombManager�Œl���Z�b�g���邽��public�ɂ���
    //�v���C���[�̈ʒu
    [System.NonSerialized]
    public Transform playerTransform;

    //���e�̏���
    [System.NonSerialized]
    public int myID;

    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("�_���[�W��")]
    private float damage;

    [SerializeField, Header("�m�b�N�o�b�N��")]
    private float knockbackForce;

    [SerializeField, Header("�p���x(�P�b������ɐi�ފp�x)")]
    private float angleSpeed;

    [SerializeField, Header("�v���C���[����̋���")]
    private float toPlayerDistance;

    public float GetToPlayerDistance => toPlayerDistance;

    [SerializeField, Header("�����ォ�甚�e��j������܂ł̎���(�b)")]
    private float bombLifeSpan;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan;

    [SerializeField, Header("��������锚�e��")]
    private int generatedBombNum;

    public int GetGeneratedBombNum => generatedBombNum;

    [SerializeField, Header("�N�[���^�C��(�b)")]
    private float coolTime;

    public float GetCoolTime => coolTime;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //�ړ�����
    private Vector3 movingDirection;

    //���e�̍����̔���
    private float halfHeight;

    //���݂̉�]�p�x
    private float currentAngle;

    //���ꂼ��̃I�u�W�F�N�g�𓙊Ԋu�ɔz�u���邽�߂̊p�x��
    private float angleBetweenObjects;

    private void Start()
    {
        myTransform = transform;
        movingDirection = Vector3.zero;
        halfHeight = GetHalfHeight();
        currentAngle = 0;
        angleBetweenObjects = 360f / generatedBombNum;
    }

    private void Update()
    {
        //�v���C���[�̎�������
        RotateAroundPlayer();
    }

    /// <summary>
    /// �v���C���[�𒆐S�ɂ���y�������ɉ�]����
    /// ���̃m�b�N�o�b�N���e�Ɠ��Ԋu�ŉ�]����
    /// </summary>
    private void RotateAroundPlayer()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame ||
            playerTransform == null) return;

        //�G�ɓ��������ꍇ�Ɉړ��������~��������
        Vector3 previousPosition = myTransform.position;

        //�v���C���[�̈ʒu
        Vector3 centerPos = playerTransform.position + Vector3.up * halfHeight;

        //�I�u�W�F�N�g���̉�]�p�x
        float objectAngle = currentAngle + myID * angleBetweenObjects;

        //�I�C���[�p����N�H�[�^�j�I������
        Quaternion rotation = Quaternion.Euler(0, objectAngle, 0);

        //��]��̈ʒu���v�Z
        Vector3 rotatedPos = rotation * (Vector3.forward * toPlayerDistance);

        myTransform.position = rotatedPos + centerPos;

        //�m�b�N�o�b�N����ɓn�����e�̈ړ�����
        movingDirection = myTransform.position - previousPosition;

        //�P�t���[���ŉ�]����p�x
        float angle = angleSpeed * Time.deltaTime;

        currentAngle += angle;

        Debug.Log($"��]����p�x:{objectAngle}");
    }

    /// <summary>
    /// �����p�[�e�B�N���𐶐�����
    /// </summary>
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //�����𐶐�
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            //���ʉ����Đ�
            SoundManager.uniqueInstance.Play("����3");

            Debug.Log("����!!");
        }
        else
        {
            Debug.LogWarning("�p�[�e�B�N��������܂���");
        }
    }


    /// <summary>
    /// �G�ɓ��������ꍇ�̏���
    /// </summary>
    /// <param name="other">�G</param>
    private void OnTriggerEnter(Collider other)
    {
        //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamageEnemy>();
        if (applicableDamageObject != null)
        {
            //���g���\��
            gameObject.SetActive(false);

            //��������
            Explode();

            //�_���[�W��^����
            applicableDamageObject.ReceiveDamage(damage);

            //�m�b�N�o�b�N�ł���I�u�W�F�N�g�̏ꍇ
            var knockbackObject = other.gameObject.GetComponent<IApplicableKnockback>();
            if (knockbackObject != null)
            {
                //�m�b�N�o�b�N����
                knockbackObject.Knockback(knockbackForce, movingDirection);
            }

            //���g��j�󂷂�
            Destroy(gameObject, bombLifeSpan);
        }
    }

    /// <summary>
    /// ���e�̍����̔������擾����
    /// </summary>
    /// <returns>���e�̔������̍���</returns>
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            transform.localScale.y * sphereCollider.radius;
    }
}
