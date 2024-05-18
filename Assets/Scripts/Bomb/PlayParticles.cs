using UnityEngine;
using System.Collections;

public class PlayParticles : MonoBehaviour 
{
    [SerializeField]
    private ParticleSystem[] particleSystems;

    //複数のパーティクルを発生する
    public void Play()
    {
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
            Debug.Log("再生！");
        }
    }
}
