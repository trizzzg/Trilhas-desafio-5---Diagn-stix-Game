using UnityEngine;
using UnityEngine.Playables;

public class CutsceneStarter : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject UiTelaInicial;
    public GameObject player;
    public GameObject transicao;
    public Vector2 NewPosition = new Vector2(-5f, -9f);


    public void IniciarCutscene()
    {
        if (UiTelaInicial != null)
        {
            UiTelaInicial.SetActive(false);
            player.SetActive(true);
            transicao.SetActive(true);
            player.transform.position = NewPosition;

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
