using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image imagemPreta;       // A imagem preta que cobre a tela
    public float duracao = 1f;      // Tempo total do fade (em segundos)

    void Start()
    {
        if (imagemPreta != null)
        {
            StartCoroutine(FazerFadeOut());
        }
    }

    System.Collections.IEnumerator FazerFadeOut()
    {
        Color corOriginal = imagemPreta.color;

        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tempo / duracao);
            imagemPreta.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            yield return null;
        }

        imagemPreta.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f);
        imagemPreta.gameObject.SetActive(false);  // Opcional: desativa a imagem após o fade
    }
}
