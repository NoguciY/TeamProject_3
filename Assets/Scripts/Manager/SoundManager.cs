using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //���̕ʖ��Ɖ��̃y�A�����N���X
    //[System.Serializable] :�C���X�y�N�^�[�ŕҏW���邽��
    [System.Serializable]
    public class SoundData
    {
        //���̕ʖ�
        public string soundName;

        //��L�̕ʖ��ɑΉ�������
        public AudioClip audioClip;
    }

    //�V���O���g���C���X�^���X
    public static SoundManager uniqueInstance;

    //���̕ʖ��Ƃ���ɑΉ����������C���X�y�N�^�[�ŕҏW�A�Ǘ����邽�߂̔z��
    [SerializeField]
    private SoundData[] soundDatas;

    //AudioSource�i�X�s�[�J�[�j�𓯎��ɖ炵�������̐������p��(����20��)
    private AudioSource[] audioSources;

    //�ʖ�(spondName)���L�[�Ƃ����Ǘ��pDictionary
    private Dictionary<string, SoundData> soundDictionary;

    //AudioSource�̐�
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

        //auidioSourceList�z��̐�����AudioSource���������g�ɐ������Ĕz��Ɋi�[
        for (int i = 0; i < audioSources.Length; ++i)
            audioSources[i] = gameObject.AddComponent<AudioSource>();

        //soundDictionary�ɃZ�b�g
        foreach (SoundData soundData in soundDatas)
            soundDictionary.Add(soundData.soundName, soundData);
    }

    //���g�p��AudioSource�̎擾 �S�Ďg�p���̏ꍇ��null��ԋp
    private AudioSource GetUnusedAudioSource()
    {
        for (int i = 0; i < audioSources.Length; ++i)
            if (audioSources[i].isPlaying == false) 
                return audioSources[i];

        //���g�p��AudioSource���Ȃ��ꍇ
        return null; 
    }

    //�w�肳�ꂽAudioClip�𖢎g�p��AudioSource�ōĐ�
    private void Play(AudioClip clip)
    {
        AudioSource audioSource = GetUnusedAudioSource();

        //�I�[�f�B�I�\�[�X���Ȃ��ꍇ�B�Đ������Ȃ�
        if (audioSource == null) return;
      
        audioSource.clip = clip;
        audioSource.Play();
    }

    //�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
    //Play()�̃I�[�o�[���[�h
    public void Play(string name)
    {
        //�Ǘ��pDictionary ����A�ʖ��ŒT��
        if (soundDictionary.TryGetValue(name, out var soundData)) 
            //���������ꍇ�A�Đ�
            Play(soundData.audioClip);
        else
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
    }
}
