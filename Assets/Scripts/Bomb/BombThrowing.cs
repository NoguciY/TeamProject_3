using UnityEngine;

public class BombThrowing : MonoBehaviour
{
    //BombManager�Œl���Z�b�g���邽��public�ɂ���
    //�v���C���[�̈ʒu
    [System.NonSerialized]
    public Transform playerTransform;

    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("�_���[�W��")]
    private float damage;

    [SerializeField, Header("�e��(m/s)")]
    private float speed;

    [SerializeField, Header("���ˊԊu")]
    private float coolTime;

    public float GetCoolTime => coolTime;

    [SerializeField, Header("1�b�Ԃɉ�]����p�x(degree/s)")]
    private float angleOfRotationPerSecond;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan;

    [SerializeField, Header("�j�����鎞��(s)")]
    private float bombLifeSpan;
   
    //�g�����X�t�H�[��
    private Transform myTransform;

    //���x
    private Vector3 velocity;

    
    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentSceneType == SceneType.MainGame)
        {
            // �ړ�����
            myTransform.localPosition += velocity * Time.deltaTime;

            //��]����
            Rotate();

            Destroy(gameObject, bombLifeSpan);
        }
    }

    /// <summary>
    /// �e�𔭎˂��鎞�ɏ��������邽�߂̊֐�
    /// </summary>
    public void Initialize()
    {
        if (playerTransform != null)
        {
            // �e�̔��ˊp�x���x�N�g���ɕϊ�����
            var direction = playerTransform.forward;

            // ���ˊp�x�Ƒ������瑬�x�����߂�
            velocity = direction * speed;
        }
    }


    /// <summary>
    /// x�������ɉ�]����
    /// </summary>
    private void Rotate()
    {
        //1�t���[���ɉ�]����p�x
        float angle = angleOfRotationPerSecond * Time.deltaTime;

        //���t���[����]������
        myTransform.Rotate(angle, 0, 0);
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

            particle.GetComponent<ParticleSystem>().Play();

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            //���ʉ����Đ�
            SoundManager.uniqueInstance.Play("����1");

            Debug.Log("����!!");
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

            //���g��j�󂷂�
            Destroy(gameObject, bombLifeSpan);
        }
    }
}
