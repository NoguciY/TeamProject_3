using UnityEngine;

//�v���C���[�����������ꍇ
//�E�v���C���[�����l���㏸
//�E�A�C�e�����j�������

public class ItemExperienceValue : MonoBehaviour
{
    [SerializeField, Header("�o���l")]
    private float experienceValue;

    //����J�n���Ă�ł��炷���ɓ����o���̂������
    [SerializeField, Header("�}�O�l�b�g������Ă��甽������܂ł̎���")]
    private float delayTimer;

    //��������̕s��ŏ����Ȃ��ꍇ�ɔ����ĉ���ɂ�����ő�̎��Ԃ�ݒ肵�Ă���
    [SerializeField, Header("�}�O�l�b�g�ł̍ő�������")]
    private float maxTimer;

    [SerializeField, Header("����̑��x")]
    private float speed;

    //�v���C���[�ɂǂ̒��x�߂Â��������������Ƃɂ��邩
    [SerializeField, Header("�}�O�l�b�g�ł̉���\����")]
    private float collectDistance;

    private Transform playerTransform;

    private float timer;

    private bool isCollect;

    private void Start()
    {
        timer = 0.0f;

        isCollect = false;
    }

    private void FixedUpdate()
    {
        if (!isCollect)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer < delayTimer)
        {
            return;
        }
        //����̍ő厞�Ԃ𒴂��Ă��Ȃ����`�F�b�N
        else if (timer > maxTimer)
        {
            FinishCollect();
            return;
        }

        //�v���C���[�Ɍ������Đi�܂���
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed);

        //����̋����܂ŋ߂Â�����������
        var diff = playerTransform.position - transform.position;
        if (diff.magnitude < collectDistance)
        {
            FinishCollect();
        }
    }

    /// <summary>
    /// ������J�n����
    /// </summary>
    public void Collect(Transform playerTransform)
    {
        timer = 0.0f;
        isCollect = true;
        this.playerTransform = playerTransform;
    }

    /// <summary>
    /// ���������������
    /// </summary>
    public void FinishCollect()
    {
        isCollect = false;
        this.gameObject.SetActive(false);

        //������������̃^�C�~���O�ŏ������������ꍇ�͂����ɏ�����ǉ�����
    }

    /// <summary>
    /// �����蔻��
    /// </summary>
    /// <param name="other">�v���C���[</param>
    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //�o���l���擾������
            gettableItemObject.GetExperienceValue(experienceValue);

            //���X�g����폜����
            GameManager.Instance.items.Remove(this);

            Destroy(this.gameObject);
        }
    }
}    
