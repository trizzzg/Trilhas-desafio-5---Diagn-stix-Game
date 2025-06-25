using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image imagemPreta;
    public float duracao = 1f;

    public void IniciarFadeOut(System.Action aoTerminar)
    {
        StartCoroutine(FazerFadeOut(aoTerminar));
    }

    IEnumerator FazerFadeOut(System.Action aoTerminar)
    {
        imagemPreta.gameObject.SetActive(true);
        Color cor = imagemPreta.color;

        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tempo / duracao);
            imagemPreta.color = new Color(cor.r, cor.g, cor.b, alpha);
            yield return null;
        }

        imagemPreta.color = new Color(cor.r, cor.g, cor.b, 1f);
        aoTerminar?.Invoke();
    }
}
