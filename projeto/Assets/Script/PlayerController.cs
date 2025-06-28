using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public System.Action OnChegouPosicaoInicial;

    [Header("Movimento")]
    public float velocidade = 2f;
    public Vector2 destinoInicial = new Vector2(1.295f, -2.496f);
    public Vector2 destinoFinal = new Vector2(-5.8f, -0.98f);

    private Rigidbody2D rb;
    private Animator animator;

    private bool indoParaInicio = true;  // Vai para posição inicial no começo
    private bool andandoParaPorta = false;  // Depois vai para destinoFinal

    private bool parar = false;  // Controla animação e parada do movimento

    public FadeController fadeController;

    [Header("Balões de Emoji (2 balões apenas)")]
    public GameObject[] balaoEmojis = new GameObject[2];
    public SpriteRenderer[] spriteEmojis = new SpriteRenderer[2];
    public Sprite[] spritesEmoji = new Sprite[2];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        EsconderTodosBaloes();
    }

    void FixedUpdate()
    {
        if (indoParaInicio && !parar)
        {
            MoverPara(destinoInicial, () =>
            {
                indoParaInicio = false;
                parar = true;
                OnChegouPosicaoInicial?.Invoke();
            });
        }
        else if (andandoParaPorta && !parar)
        {
            MoverPara(destinoFinal, () =>
            {
                andandoParaPorta = false;
                parar = true;
                Debug.Log("Player chegou ao destino final!");

                // Transição para cena questionario
    if (fadeController != null)
{
    fadeController.IniciarFadeOut(() =>
    {
        SceneManager.LoadScene("questionario");
    });
}
else
{
    SceneManager.LoadScene("questionario");
}
            });
        }

        if (animator != null)
        {
            animator.SetBool("parar", parar);
        }
    }

    void MoverPara(Vector2 destino, System.Action aoChegar)
    {
        Vector2 novaPos = Vector2.MoveTowards(rb.position, destino, velocidade * Time.fixedDeltaTime);
        rb.MovePosition(novaPos);

        if (Vector2.Distance(rb.position, destino) < 0.01f)
        {
            rb.MovePosition(destino);
            aoChegar?.Invoke();
        }
    }

    // Chame esse método para começar a andar para o destinoFinal
    public void ComecarAndar()
    {
        parar = false;
        andandoParaPorta = true;
    }

    // Métodos para mostrar emojis, mantém igual
    public void MostrarEmoji1()
    {
        MostrarBalao(0);
    }

    public void MostrarEmoji2()
    {
        MostrarBalao(1);
    }

    void MostrarBalao(int index)
    {
        if (index < 0 || index >= 2)
        {
            Debug.LogWarning($"❗ Índice {index} inválido para balões (esperado 0 ou 1)");
            return;
        }

        if (balaoEmojis[index] == null || spriteEmojis[index] == null || spritesEmoji[index] == null)
        {
            Debug.LogWarning($"❗ Balão, SpriteRenderer ou Sprite não configurado no índice {index}");
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            bool ativo = (i == index);
            if (balaoEmojis[i] != null)
                balaoEmojis[i].SetActive(ativo);

            if (ativo && spriteEmojis[i] != null)
                spriteEmojis[i].sprite = spritesEmoji[index];
        }

        CancelInvoke(nameof(EsconderTodosBaloes));
        Invoke(nameof(EsconderTodosBaloes), 2f);
    }

    void EsconderTodosBaloes()
    {
        for (int i = 0; i < 2; i++)
        {
            if (balaoEmojis[i] != null)
                balaoEmojis[i].SetActive(false);
        }
    }
}
