using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�g���N���X�Ɗ֘A�������܂Ƃ߂��N���X
//�e�X�e�[�g���܂Ƃ߂ă��\�b�h���Ăяo��

public class StateMachine<TOwner>
{
    //�X�e�[�g���N���X
    public abstract class StateBase
    {
        //��������X�e�[�g�}�V��
        public StateMachine<TOwner> stateMachine;
        
        //�Q�b�^�[
        protected TOwner GetOwner => stateMachine.owner;

        //�X�e�[�g�J�n���Ɏ��s����
        public virtual void OnStart() { }

        //Update���\�b�h���ŌĂяo��
        //���t���[�����ɍs����������
        public virtual void OnUpdate() { }

        //FixedUpdate���\�b�h���ŌĂяo��
        public virtual void OnFixedupdate() { }

        //�X�e�[�g�I�����Ɏ��s����
        public virtual void OnExit() { }
    }

    //�X�e�[�g�}�V�[���̎g�p��
    private TOwner owner;

    //�Q�b�^�[
    //public TOwner GetOwner => owner;

    //���݂̃X�e�[�g
    private StateBase currentState;

    //�S�ẴX�e�[�g���`���邽�߂̃f�B�N�V���i���[
    //int:enum�ŗ񋓂����X�e�[�g
    //StateBase:�X�e�[�g�N���X�Ɍp����������N���X�^
    private readonly Dictionary<int, StateBase> states = new Dictionary<int, StateBase>();

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="owner">�X�e�[�g�}�V���̎g�p��</param>
    public StateMachine(TOwner owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// �X�e�[�g��`��o�^
    /// �X�e�[�g�}�V����������ɂ��̃��\�b�h���Ă�
    /// </summary>
    /// <typeparam name="TState">�X�e�[�g�^</typeparam>
    /// <param name="stateId">�X�e�[�gID</param>
    public void Add<TState>(int stateId) where TState : StateBase, new()
    {
        //�����X�e�[�gID(Key)��states�Ɋi�[����̂�h��
        if (states.ContainsKey(stateId))
        {
            Debug.LogError($"���ɃX�e�[�gID���o�^����Ă��܂�:{stateId}");
            return;
        }
        //�X�e�[�g��`��o�^(�X�e�[�g���f�B�N�V���i���[�Ɋi�[)
        var newState = new TState
        {
            //��������X�e�[�g�}�V���ɃA�N�Z�X���邽��
            //�V�����X�e�[�g�ɃX�e�[�g�}�V���̎Q�Ƃ�ݒ�
            stateMachine = this 
        };
        states.Add(stateId, newState);
    }

    /// <summary>
    /// �X�e�[�g�J�n����
    /// </summary>
    /// <param name="stateId">���݂̃X�e�[�gID</param>
    public void OnStart(int stateId)
    {
        //StateID���X�e�[�g�Ɗ֘A�t�����Ă��Ȃ��ꍇ�A�G���[
        if (!states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError($"�X�e�[�gID���ݒ肳��Ă��܂���:{stateId}");
            return;
        }

        //���݂̃X�e�[�g�ɐݒ肵�ď������J�n
        currentState = nextState;
        currentState.OnStart();
    }

    /// <summary>
    /// �X�e�[�g�X�V����
    /// </summary>
    public void OnUpdate()
    {
        currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState.OnFixedupdate();
    }

    /// <summary>
    /// ���̃X�e�[�g�ɐ؂�ւ���
    /// </summary>
    /// <param name="stateId">�؂�ւ���X�e�[�gID</param>
    public void ChangeState(int stateId)
    {
        if (!states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError($"�X�e�[�gID���ݒ肳��Ă��܂���{stateId}");
            return;
        }

        //�X�e�[�g��؂�ւ���
        currentState.OnExit();
        currentState = nextState;
        currentState.OnStart();
    }
}