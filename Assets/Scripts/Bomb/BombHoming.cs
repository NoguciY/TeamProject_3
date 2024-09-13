using UnityEngine;

public class BombHoming : MonoBehaviour
{
    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan;

    [SerializeField, Header("�_���[�W��")]
    private float damage;

    [Header("��������܂ł̎���")]
    public float fuseTime;

    [Header("�����͈�")]
    public float explosionRadius;

    [Header("�ړ����x")]
    public float moveSpeed;

    [SerializeField, Header("�㏸����ő卂��")]
    private float maxUpwardHeight;

    //�㏸�����ǂ����̃t���O
    private bool isAscending;

    //�����ʒu
    private Vector3 initialPosition;

    //���݂̃^�[�Q�b�g
    private Transform target; 

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private CapsuleCollider capsuleCollider;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 _forward = Vector3.forward;

    //�X�t�B�A�L���X�g�̍ő勗��
    private float maxDistance;

    //�o�ߎ���
    private float elapsedTime;

    private void Start()
    {
        initialPosition = transform.position;
        maxDistance = 0;
        isAscending = true;

        // �ŏ��̃^�[�Q�b�g��ݒ�
        SetRandomTarget();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame) return;

        if (isAscending)
        {
            // �㏸
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // ���̍����܂ŏ㏸������
            if (transform.position.y >= initialPosition.y + maxUpwardHeight)
            {
                isAscending = false;
                SetRandomTarget(); // �^�[�Q�b�g��ݒ�
            }
        }
        else if (isAscending == false)
        {
            if (target == null)
            {
                // �㏸
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                isAscending = true;
                return;
            }
            // �^�[�Q�b�g�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // �^�[�Q�b�g�ւ̌����x�N�g���v�Z
            var dir = target.position - transform.position;
            // �^�[�Q�b�g�̕����ւ̉�]
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // ��]�␳
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

            // ��]�␳���^�[�Q�b�g�����ւ̉�]�̏��ɁA���g�̌����𑀍삷��
            transform.rotation = lookAtRotation * offsetRotation;

            // �^�[�Q�b�g�ɓ��B������
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Explode();
                Detonate();
            }
        }

        //�o�ߎ��Ԃ��������Ԃ��߂����ꍇ�A����
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= fuseTime)
        {
            Explode();
            Detonate();
        }

    }

    /// <summary>
    /// �����_���Ƀ^�[�Q�b�g��ݒ肷��
    /// </summary>
    private void SetRandomTarget()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Enemy"); // "Target"�^�O�̃I�u�W�F�N�g���擾
        if (targetObjects.Length > 0)
        {
            // �����_���Ƀ^�[�Q�b�g��I��
            int randomIndex = Random.Range(0, targetObjects.Length);
            target = targetObjects[randomIndex].transform;
        }
    }

    /// <summary>
    /// �Ώۂɓ��������ꍇ�A�_���[�W��^����
    /// </summary>
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward, maxDistance);

        foreach (var hit in hits)
        {
            var applicableDamageObject = hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();

            PlayerManager playe = hit.collider.gameObject.GetComponent<PlayerManager>();
            if (applicableDamageObject != null)
            {
                applicableDamageObject.ReceiveDamage(damage);
            }
        }

        Destroy(gameObject);
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
                Instantiate(explosionParticle, transform.position, Quaternion.identity);

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            //���ʉ����Đ�
            SoundManager.uniqueInstance.PlaySE("����4");

            Debug.Log("����!!");
        }
        else
        {
            Debug.LogWarning("�p�[�e�B�N��������܂���");
        }
    }

    /// <summary>
    /// �p�[�e�B�N���𐶐��A�_���[�W��^����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //�t�B�[���h�ɓ��������ꍇ
        if(other.gameObject.CompareTag("Field"))
        {
            Explode();
            Detonate();
        }
    }
}
