using UnityEngine;
using TMPro;
using System.Collections;

public class Doctor : MonoBehaviour
{
    public TextMeshProUGUI textoDialogo;
    public GameObject caixaDialogo;
    public Animator animator;

    public PlayerController player;

    public float velocidade = 2f;
    public Vector2 destino = new Vector2(-6.93f, -0.98f);

    private Rigidbody2D rb;
    private string[] falas = new string[]
    {
        "Olá, como você está se sentindo?",
        "Entendo, que tal respondermos umas perguntinhas?",
        "Para entender melhor o que está acontecendo",
        "Siga-me"
    };

    private int indexFala = 0;
    private bool esperandoClique = false;
    private bool podeIniciarDialogo = false;
    private bool dialogoIniciado = false;
    private bool andar = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        caixaDialogo.SetActive(false);

        player.OnChegouPosicaoInicial += LiberarCliqueParaDialogo;
    }

    void Update()
    {
        if (podeIniciarDialogo && !dialogoIniciado && Input.GetMouseButtonDown(0))
        {
            dialogoIniciado = true;
            caixaDialogo.SetActive(true);
            StartCoroutine(MostrarFala());
            return;
        }

        if (dialogoIniciado && esperandoClique && Input.GetMouseButtonDown(0))
        {
            esperandoClique = false;
            indexFala++;

            if (indexFala < falas.Length)
            {
                StartCoroutine(MostrarFala());
            }
            else
            {
                FinalizarCutscene();
            }
        }
    }

    void FixedUpdate()
    {
        if (andar)
        {
            Vector2 novaPos = Vector2.MoveTowards(rb.position, destino, velocidade * Time.fixedDeltaTime);
            rb.MovePosition(novaPos);

            if (Vector2.Distance(rb.position, destino) < 0.01f)
            {
                rb.MovePosition(destino);
                andar = false;
                animator.SetBool("andar", false);
                Debug.Log("Doctor chegou ao destino!");
            }
        }
    }

    void LiberarCliqueParaDialogo()
    {
        podeIniciarDialogo = true;
    }

    IEnumerator MostrarFala()
    {
        textoDialogo.text = falas[indexFala];

        if (player != null)
        {
            if (indexFala == 0)
                player.MostrarEmoji1(); 
            else if (indexFala == 2)
                player.MostrarEmoji2(); 
        }

        yield return null;
        esperandoClique = true;
    }

    void FinalizarCutscene()
    {
        caixaDialogo.SetActive(false);
        andar = true;
        animator.SetBool("andar", true);

        if (player != null)
            player.ComecarAndar();
    }
}
