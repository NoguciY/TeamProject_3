using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //�A�j���[�^�[�R���|�[�l���g
    [SerializeField]
    private Animator playerAnimator;

    //����A�j���[�V�������I��������
    public bool isFinishedDeadAnimation;

    //�ҋ@�A�j���[�V����������
    public void SetIdleAnimation()
    {
        playerAnimator.SetBool("isMoving", false);
    }

    //����A�j���[�V����������
    public void SetRunAnimation()
    {
        playerAnimator.SetBool("isMoving", true);

        //S�L�[�������̂ݑ���A�j���[�V�������t�Đ�����
        if (Input.GetKey(KeyCode.S))
            playerAnimator.SetFloat("speed", -1);
        else
            playerAnimator.SetFloat("speed", 1);
    }

    //�U���A�j���[�V����������
    public void SetAttackAnimation()
    {
        playerAnimator.SetTrigger("attackTrigger");
    }

    //����A�j���[�V����������
    public void SetDeadAnimation()
    {
        isFinishedDeadAnimation = false;
        playerAnimator.SetTrigger("deadTrigger");
    }

    //����A�j���[�V�����̏I����m�点��
    public void FinishDeadAnimation()
    {
        isFinishedDeadAnimation = true;
    }
}
