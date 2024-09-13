using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeSlider : MonoBehaviour
{
    //スライダーのvalueとBGM、SEの音量をリンクさせる
    [SerializeField]
    private Slider bgmVolumeSlider;

    [SerializeField]
    private Slider seVolumeSlider;

    //オーディオミキサー
    [SerializeField]
    private AudioMixer audioMixer;

    /// <summary>
    /// スライダーの値を設定
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
    /// ・スライダーのonValueChangedイベントに登録する
    /// BGMスライダーの値が変化した場合に
    /// スライダーの値をミキサーに反映させる
    /// </summary>
    /// <param name="volume">スライダーの値</param>
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMParameters", volume);
    }

    /// <summary>
    /// ・スライダーのonValueChangedイベントに登録する
    /// BGMスライダーの値が変化した場合に
    /// スライダーの値をミキサーに反映させる
    /// </summary>
    /// <param name="volume">スライダーの値</param>
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEParameters", volume);
    }
}
