using UnityEngine;
using UnityEngine.Playables;

public class CutsceneStarter : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject UiTelaInicial;

    public void IniciarCutscene()
    {
        if (UiTelaInicial != null)
        {
            UiTelaInicial.SetActive(false);
        }

        if (timeline != null)
        {
            timeline.Play();
        }
        else
        {
            Debug.LogWarning("Timeline não atribuída!");
        }
    }
}
