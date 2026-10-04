using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SoundEffectsCoin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource src;
    public AudioClip sfx1;

    public void Coin()
    {
        src.clip = sfx1;
        src.Play();
    }
}
