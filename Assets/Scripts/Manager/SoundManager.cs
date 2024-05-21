using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

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
    private const int AUDIOSOURCENUM = 20;

    private void Awake()
    {
        if(uniqueInstance == null)
        {
            uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        audioSources = new AudioSource[AUDIOSOURCENUM];

        soundDictionary = new Dictionary<string, SoundData>();

        //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        for (int i = 0; i < audioSources.Length; ++i)
            audioSources[i] = gameObject.AddComponent<AudioSource>();

        //soundDictionaryにセット
        foreach (SoundData soundData in soundDatas)
            soundDictionary.Add(soundData.soundName, soundData);
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedAudioSource()
    {
        for (int i = 0; i < audioSources.Length; ++i)
            if (audioSources[i].isPlaying == false) 
                return audioSources[i];

        //未使用のAudioSourceがない場合
        return null; 
    }

    //指定されたAudioClipを未使用のAudioSourceで再生
    private void Play(AudioClip clip)
    {
        AudioSource audioSource = GetUnusedAudioSource();

        //オーディオソースがない場合。再生させない
        if (audioSource == null) return;
      
        audioSource.clip = clip;
        audioSource.Play();
    }

    //指定された別名で登録されたAudioClipを再生
    //Play()のオーバーロード
    public void Play(string name)
    {
        //管理用Dictionary から、別名で探索
        if (soundDictionary.TryGetValue(name, out var soundData)) 
            //見つかった場合、再生
            Play(soundData.audioClip);
        else
            Debug.LogWarning($"その別名は登録されていません:{name}");
    }
}
