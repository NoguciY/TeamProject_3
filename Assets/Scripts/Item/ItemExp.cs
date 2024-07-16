using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�����������ꍇ
//�E�v���C���[�����l���㏸
//�E�A�C�e�����j�������

public class ItemExp : MonoBehaviour
{
    //�o���l
    private int exp = 1;

    [SerializeField]
    private Transform _playerTransform;

    //����J�n���Ă�ł��炷���ɓ����o���̂������
    [SerializeField]
    private float _delayTimer = 0.5f;

    //��������̕s��ŏ����Ȃ��ꍇ�ɔ����ĉ���ɂ�����ő�̎��Ԃ�ݒ肵�Ă���
    [SerializeField]
    private float _maxTimer = 10.0f;

    //����̑��x
    [SerializeField]
    private float _speed = 0.4f;

    //�v���C���[�ɂǂ̒��x�߂Â��������������Ƃɂ��邩
    [SerializeField]
    private float _collectDistance = 0.3f;

    private float _timer = 0.0f;
    private bool _isCollect = false;

    private void FixedUpdate()
    {
        if (!_isCollect)
        {
            return;
        }

        _timer += Time.deltaTime;
        if (_timer < _delayTimer)
        {
            return;
        }
        //����̍ő厞�Ԃ𒴂��Ă��Ȃ����`�F�b�N
        else if (_timer > _maxTimer)
        {
            FinishCollect();
            return;
        }

        //�v���C���[�Ɍ������Đi�܂���
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed);

        //����̋����܂ŋ߂Â�����������
        var diff = _playerTransform.position - transform.position;
        if (diff.magnitude < _collectDistance)
        {
            FinishCollect();
        }
    }

    /// <summary>
    /// ������J�n����
    /// </summary>
    public void Collect(Transform playerTransform)
    {
        _timer = 0.0f;
        _isCollect = true;
        _playerTransform = playerTransform;
    }

    /// <summary>
    /// ���������������
    /// </summary>
    public void FinishCollect()
    {
        _isCollect = false;
        this.gameObject.SetActive(false);

        //������������̃^�C�~���O�ŏ������������ꍇ�͂����ɏ�����ǉ�����
    }

    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //�o���l���擾������
            gettableItemObject.GetExp(exp);

            //���X�g����폜����
            GameManager.Instance.items.Remove(this);

            Destroy(this.gameObject);
        }
    }
}    
