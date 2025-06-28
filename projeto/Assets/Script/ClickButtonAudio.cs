using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public AudioSource AudioSource;
    public void PlayButton()
    {
        GetComponent<AudioSource>().Play();
    }
}
