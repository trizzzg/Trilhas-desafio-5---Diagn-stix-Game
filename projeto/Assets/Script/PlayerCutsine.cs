using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    public Vector2 destino = new Vector2(-1.70f, -2.15f);
    public float velocidade = 2f;

    private Rigidbody2D rb;
    private bool movendo = false;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    IniciarMovimento();
}


    void FixedUpdate()
    {
        if (movendo)
        {
            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, destino, velocidade * Time.fixedDeltaTime);
            rb.MovePosition(novaPosicao);

            if (Vector2.Distance(rb.position, destino) < 0.01f)
            {
                rb.MovePosition(destino);
                movendo = false;
            }
        }
    }

    public void IniciarMovimento()
    {
        movendo = true;
    }
}
