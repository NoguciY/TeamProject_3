using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    //音の別名と音のペアを持つクラス
    //[System.Serializable] :インスペクターで編集するため
    [System.Serializable]
    public class SoundData
    {
        //音の別名
        public string soundName;

        //上記の別名に対応した音
        public AudioClip audioClip;

        //音量
        public float volume;
    }

    //オーディオミキサー
    [SerializeField]
    private AudioMixer audioMixer;

    //シングルトンインスタンス
    public static SoundManager uniqueInstance;

    //音の別名とそれに対応した音をインスペクターで編集、管理するための配列
    [SerializeField]
    private SoundData[] soundDatas;

    //AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意(仮で20個)
    private AudioSource[] audioSources;

    //別名(spondName)をキーとした管理用Dictionary
    private Dictionary<string, SoundData> soundDictionary;

    //AudioSourceの数
    private const int AUDIOSOURCENUM = 10;

    //BGM用のAudioSource
    [SerializeField]
    private AudioSource bgmAudioSource;

    private void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSources = new AudioSource[AUDIOSOURCENUM];

        soundDictionary = new Dictionary<string, SoundData>();

        //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        for (int i = 0; i < audioSources.Length; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();

            //音量調整のためにミキサーグループを取得
            audioSources[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups("SE")[0];
        }

        //soundDictionaryにセット
        foreach (SoundData soundData in soundDatas)
        {
            soundDictionary.Add(soundData.soundName, soundData);
        }
    }

    /// <summary>
    /// 未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    /// </summary>
    /// <returns>未使用のAudioSource</returns>
    private AudioSource GetUnusedAudioSource()
    {
        for (int i = 0; i < audioSources.Length; ++i)
        {
            if (audioSources[i].isPlaying == false)
            {
                return audioSources[i];
            }
        }

        //未使用のAudioSourceがない場合
        return null; 
    }

    /// <summary>
    /// 指定されたAudioClipを未使用のAudioSourceで再生
    /// </summary>
    /// <param name="clip">再生するAudioClip</param>
    /// <param name="volume">音量</param>
    private void Play(AudioClip clip, float volume)
    {
        AudioSource audioSource = GetUnusedAudioSource();

        //オーディオソースがない場合。再生させない
        if (audioSource == null) return;
      
        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();
    }

    /// <summary>
    /// 指定された別名で登録されたAudioClipを再生
    /// Play()のオーバーロード
    /// </summary>
    /// <param name="name">再生する音の名前</param>
    public void PlaySE(string name)
    {
        //管理用Dictionary から、別名で探索
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            //見つかった場合、再生
            Play(soundData.audioClip, soundData.volume);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="name">再生するBGMの名前</param>
    public void PlayBgm(string name)
    {
        //管理用Dictionary から、別名で探索
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            //見つかった場合、再生
            bgmAudioSource.clip = soundData.audioClip;
            bgmAudioSource.volume = soundData.volume;
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
}
