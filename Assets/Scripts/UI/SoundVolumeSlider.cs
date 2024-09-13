using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeSlider : MonoBehaviour
{
    //�X���C�_�[��value��BGM�ASE�̉��ʂ������N������
    [SerializeField]
    private Slider bgmVolumeSlider;

    [SerializeField]
    private Slider seVolumeSlider;

    //�I�[�f�B�I�~�L�T�[
    [SerializeField]
    private AudioMixer audioMixer;

    /// <summary>
    /// �X���C�_�[�̒l��ݒ�
    /// </summary>
    public void Initialize()
    {
        //BGM
        audioMixer.GetFloat("BGMParameters", out float bgmVolume);
        bgmVolumeSlider.value = bgmVolume;

        //SE
        audioMixer.GetFloat("SEParameters", out float seVolume);
        seVolumeSlider.value = seVolume;
    }

    /// <summary>
    /// �E�X���C�_�[��onValueChanged�C�x���g�ɓo�^����
    /// BGM�X���C�_�[�̒l���ω������ꍇ��
    /// �X���C�_�[�̒l���~�L�T�[�ɔ��f������
    /// </summary>
    /// <param name="volume">�X���C�_�[�̒l</param>
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMParameters", volume);
    }

    /// <summary>
    /// �E�X���C�_�[��onValueChanged�C�x���g�ɓo�^����
    /// BGM�X���C�_�[�̒l���ω������ꍇ��
    /// �X���C�_�[�̒l���~�L�T�[�ɔ��f������
    /// </summary>
    /// <param name="volume">�X���C�_�[�̒l</param>
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEParameters", volume);
    }
}
