using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicaDaCena : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (!audio.isPlaying)
        {
            audio.loop = true;
            audio.Play();
        }
    }
}
