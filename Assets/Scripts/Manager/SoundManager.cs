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

    //���̕ʖ��Ƃ���ɑΉ����������C���X�y�N�^�[�ŕҏW�A�Ǘ����邽�߂̔z��
    [SerializeField]
    private SoundData[] soundDatas;

    //AudioSource�i�X�s�[�J�[�j�𓯎��ɖ炵�������̐������p��(����20��)
    private AudioSource[] audioSourceList = new AudioSource[20];

    //�ʖ�(spondName)���L�[�Ƃ����Ǘ��pDictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    private void Awake()
    {
        //auidioSourceList�z��̐�����AudioSource���������g�ɐ������Ĕz��Ɋi�[
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        //soundDictionary�ɃZ�b�g
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.soundName, soundData);
        }
    }

    //���g�p��AudioSource�̎擾 �S�Ďg�p���̏ꍇ��null��ԋp
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //���g�p��AudioSource�͌�����܂���ł���
    }

    //�w�肳�ꂽAudioClip�𖢎g�p��AudioSource�ōĐ�
    private void Play(AudioClip clip)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //�Đ��ł��܂���ł���
        audioSource.clip = clip;
        audioSource.Play();
    }

    //�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
    //Play()�̃I�[�o�[���[�h
    public void Play(string name)
    {
        //�Ǘ��pDictionary ����A�ʖ��ŒT��
        if (soundDictionary.TryGetValue(name, out var soundData)) 
        {
            //���������ꍇ�A�Đ�
            Play(soundData.audioClip);
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
        }
    }
}
