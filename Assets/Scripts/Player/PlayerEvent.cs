using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    //�Q�[���I�[�o�[�̏ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    //public UnityEvent gameOverEvent;
    public UnityEvent gameOverEvent = new UnityEvent();

    //�o���l�𓾂��ꍇ�Ɏ��s����C�x���g
    public class ExperienceValueEvent : UnityEvent<float, float> { }
    //public ExpEvent expEvent;
    public ExperienceValueEvent experienceValueEvent = new ExperienceValueEvent();

    //�_���[�W���󂯂��ꍇ�Ɏ��s����C�x���g
    public class AddLifeEvent : UnityEvent<float> { }
    //public AddLifeEvent addLifeEvent;
    public AddLifeEvent addLifeEvent = new AddLifeEvent();

    //�̗͂̍ő�l��n���ꍇ�Ɏ��s����C�x���g
    public class GetMaxLifeEvent : UnityEvent<float> { }
    //public GetMaxLifeEvent getMaxLifeEvent;
    public GetMaxLifeEvent getMaxLifeEvent = new GetMaxLifeEvent();

    //�o���l�̍ő�l��n���ꍇ�Ɏ��s����C�x���g
    public class GetMaxExperienceValueEvent : UnityEvent<float, float> { }
    public GetMaxExperienceValueEvent getMaxExperienceValueEvent = new GetMaxExperienceValueEvent();

    //���x���A�b�v�̏ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    //public UnityEvent levelUpEvent;
    public UnityEvent levelUpEvent = new UnityEvent();

    //���x���A�b�v�ŐV�������e��ǉ�����ꍇ�Ɏ��s����C�x���g
    [NonSerialized]
    //public UnityEvent addNewBombEvent;
    public UnityEvent addNewBombEvent = new UnityEvent();

    //�N�[���_�E���C�x���g
    public class CoolDownEvent : UnityEvent<int, float> { }
    public CoolDownEvent coolDownEvent = new CoolDownEvent();

    //private void Awake()
    //{
    //    gameOverEvent = new UnityEvent();
    //    expEvent = new ExpEvent();
    //    addLifeEvent = new AddLifeEvent();
    //    getMaxLifeEvent = new GetMaxLifeEvent();
    //    levelUpEvent = new UnityEvent();
    //    addNewBombEvent = new UnityEvent();
    //}
}
