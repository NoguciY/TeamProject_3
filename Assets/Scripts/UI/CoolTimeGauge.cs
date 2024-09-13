using UnityEngine;
using UnityEngine.UI;


public class CoolTimeGauge : MonoBehaviour
{
    [SerializeField, Header("�N�[���^�C���Q�[�W")]
    private Image[] coolTimeGauges;

    //�N�[���^�C��
    private float[] coolTimes;
    
    //�Q�[�W�������Ă��邩
    private bool[] isFilling;

    //�N�[���^�C���Q�[�W�̐�
    private int gaugeNum;
    
    public int GetGeugeNum => gaugeNum;

    void Start()
    {
        gaugeNum = coolTimeGauges.Length;
        coolTimes = new float[gaugeNum];
        isFilling = new bool[gaugeNum];
    }

    /// <summary>
    /// �N�[���^�C���Q�[�W�𖞂���
    /// </summary>
    /// <param name="bombID">���e�̌ŗL�ԍ�</param>
    public void FillGauge(int bombID)
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame) return;

        //�Q�[�W�𖞂���
        if (isFilling[bombID] && coolTimes[bombID] > 0)
        {
            coolTimeGauges[bombID].fillAmount += 1f / coolTimes[bombID] * Time.deltaTime;

            //�������������ꍇ�A�������̂��~�߂�
            if (coolTimeGauges[bombID].fillAmount >= 1f)
            {
                isFilling[bombID] = false;
            }
        }
    }
    
    /// <summary>
    /// �N�[���^�C���Q�[�W�𖞂����n�߂�
    /// </summary>
    /// <param name="bombID">���e�̌ŗL�ԍ�</param>
    /// <param name="coolTime">�N�[���_�E���̎���(�b)</param>
    public void StartFillingCoolTimeGauge(int bombID, float coolTime)
    {
        //�N�[���^�C�����擾���Ă��Ȃ��ꍇ�A�N�[���^�C�����擾����
        if (coolTimes[bombID] <= 0)
        {
            coolTimes[bombID] = coolTime;
        }

        coolTimeGauges[bombID].fillAmount = 0;
        isFilling[bombID] = true;
    }
}
