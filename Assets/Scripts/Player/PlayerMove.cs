using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// ���͒l���瑬�x��Ԃ�
    /// </summary>
    /// <param name="speed">����</param>
    /// <returns>���x</returns>
    public Vector3 InputMove(float speed)
    {
        //�ړ��x�N�g��
        Vector3 moveVec = Vector3.zero;

        //���͒l���擾����
        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveVec -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVec -= Vector3.right;
        }

        //�^�������Ǝ΂߂̈ړ��Ńx�N�g���̑傫�����ς��Ȃ��悤�ɂ���
        if (moveVec.magnitude > 1f)
        {
            moveVec = moveVec.normalized;
        }

        return moveVec * speed * Time.deltaTime;
    }

    /// <summary>
    /// �}�E�X�J�[�\�������ɉ�]���W���X�V����
    /// </summary>
    public void UpdateRotationForMouse(Camera camera, Transform myTransform)
    {
        //�v���C���[�̃��[���h��ԍ��W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = camera.WorldToScreenPoint(myTransform.position);

        //�v���C���[����}�E�X�J�[�\���̕���
        Vector3 direction = Input.mousePosition - screenPos;

        //2�̃x�N�g���̂Ȃ��p���擾
        //float angle = Utilities.GetAngle(Vector3.zero, direction);
        var dx = direction.x;
        var dy = direction.y;
        var rad = Mathf.Atan2(dx, dy);
        float angle = rad * Mathf.Rad2Deg;

        //�v���C���[�̉�]���W�̍X�V
        Vector3 angles = myTransform.localEulerAngles;
        angles.y = angle;
        myTransform.localEulerAngles = angles;
    }
}
