using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    //�Q�[���I�[�o�[�̏ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    public UnityEvent gameOverEvent = new UnityEvent();

    //�o���l�𓾂��ꍇ�Ɏ��s����C�x���g
    public class ExpEvent : UnityEvent<int, int> { }
    public ExpEvent expEvent = new ExpEvent();

    //�_���[�W���󂯂��ꍇ�Ɏ��s����C�x���g
    public class AddLifeEvent : UnityEvent<float> { }
    public AddLifeEvent addLifeEvent = new AddLifeEvent();

    //�̗͂̍ő�l��n���ꍇ�Ɏ��s����C�x���g
    public class GetMaxLifeEvent : UnityEvent<float> { }
    public GetMaxLifeEvent getMaxLifeEvent = new GetMaxLifeEvent();

    //���x���A�b�v�̏ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    public UnityEvent levelUpEvent = new UnityEvent();

    //���x���A�b�v�ŐV�������e��ǉ�����ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    public UnityEvent AddNewBombEvent = new UnityEvent();
}
