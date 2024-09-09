using UnityEngine;


public class BombPlanted : MonoBehaviour
{
    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("�_���[�W��")]
    private float damage;

    [SerializeField, Header("��������܂ł̎���")]
    public float fuseTime;

    [SerializeField, Header("�����͈�")]
    private float explosionRadius;

    public float ExplosionRadius
    {
        get { return explosionRadius; }
        set
        {
            explosionRadius = value;
            Debug.Log($"�ݒu�^���e�̔����͈�:{explosionRadius}");
        }
    }

    [SerializeField, Header("�����ォ�甚�e��j������܂ł̎���(�b)")]
    private float bombLifeSpan;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan;

    [SerializeField, Header("�N�[���^�C��(�b)")]
    private float coolTime;

    public float GetCoolTime => coolTime;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //���C���΂��ő勗��
    private float maxDistance;

    private void Start()
    {
        myTransform = transform;
        maxDistance = 0;

        //fuseTime��ɔ�������
        Invoke("Detonate", fuseTime);
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
            SoundManager.uniqueInstance.Play("����2");

            Debug.Log("����!!");
        }
    }

    /// <summary>
    /// �͈͓��ɂ���I�u�W�F�N�g�Ƀ_���[�W��^����
    /// </summary>
    private void Detonate()
    {
        //���g���\��
        gameObject.SetActive(false);
        
        //��������
        Explode();

        //����̃��C�Ƀq�b�g�����S�ẴR���C�_�[���擾����
        //�����F���̒��S�A���̔��a�A���C���΂������A��΂��ő勗��
        RaycastHit[] hits = Physics.SphereCastAll(
            myTransform.position,explosionRadius, Vector3.forward, maxDistance);
        
        foreach (var hit in hits)
        {
            //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
            var applicableDamageObject = 
                hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();

            if (applicableDamageObject != null)
            {
                applicableDamageObject.ReceiveDamage(damage);
            }
        }

        //���e�𔚔��p�[�e�B�N���j����ɔj������
        Destroy(this.gameObject, bombLifeSpan);
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
