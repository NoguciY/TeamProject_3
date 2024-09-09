using UnityEngine;
using System.Collections;

public class PlayParticles : MonoBehaviour 
{
    [SerializeField]
    private ParticleSystem[] particleSystems;

    /// <summary>
    /// 複数のパーティクルを発生する
    /// </summary>
    public void Play()
    {
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }
}
