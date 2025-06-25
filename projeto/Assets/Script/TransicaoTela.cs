using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TransicaoTela : MonoBehaviour
{
    public float duracao = 1f;
    public float tempoEspera = 5f;

    [SerializeField]
#if UNITY_EDITOR
    public SceneAsset cenaParaCarregar;
#else
    public string nomeCena; // usado apenas no build
#endif

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Começa preto
        var cor = spriteRenderer.color;
        cor.a = 1f;
        spriteRenderer.color = cor;

        StartCoroutine(FazerTransicao());
    }

    private IEnumerator FazerTransicao()
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(tempoEspera);
        yield return StartCoroutine(FadeIn());

#if UNITY_EDITOR
        if (cenaParaCarregar != null)
        {
            string caminho = AssetDatabase.GetAssetPath(cenaParaCarregar);
            string nomeCena = System.IO.Path.GetFileNameWithoutExtension(caminho);
            SceneManager.LoadScene(nomeCena);
        }
#else
        SceneManager.LoadScene(nomeCena);
#endif
    }

    private IEnumerator FadeOut()
    {
        float tempo = 0f;
        var cor = spriteRenderer.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Lerp(1f, 0f, tempo / duracao);
            spriteRenderer.color = cor;
            yield return null;
        }

        cor.a = 0f;
        spriteRenderer.color = cor;
    }

    private IEnumerator FadeIn()
    {
        float tempo = 0f;
        var cor = spriteRenderer.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Lerp(0f, 1f, tempo / duracao);
            spriteRenderer.color = cor;
            yield return null;
        }

        cor.a = 1f;
        spriteRenderer.color = cor;
    }
}
