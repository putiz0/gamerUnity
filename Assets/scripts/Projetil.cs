using Unity.VisualScripting;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public EntityStatus atacante;

    [Header("Danos")]
    public float danoFisico;
    public float danoFogo;
    public float danoGelo;
    public float danoEletrico;

    public GameObject dono;

    [Header("Movimento")]
    public float distanciaMaxima = 10f;
    private Vector3 posInicial;

    public float tempoDeVida = 5f;

    void Start()
    {
        posInicial = transform.position;

        Destroy(gameObject, tempoDeVida);

        // Ignorar colisão com o dono
        if (dono != null)
        {
            Collider2D colProjetil = GetComponent<Collider2D>();
            Collider2D colDono = dono.GetComponent<Collider2D>();

            if (colProjetil != null && colDono != null)
            {
                Physics2D.IgnoreCollision(colProjetil, colDono);
            }
        }
    }

    void Update()
    {
        float distancia = Vector3.Distance(posInicial, transform.position);

        if (distancia >= distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    // Ignora o dono
    if (other.gameObject == dono) return;

    // Ignora triggers que não são paredes (opcional)
    if (other.isTrigger && !other.CompareTag("wall")) return;

    // Colidiu com parede
    if (other.CompareTag("wall"))
    {
        Destroy(gameObject);
        return;
    }
      // Colidiu com outro projétil
    if (other.CompareTag("projetion"))
    {
        Destroy(other.gameObject);  // destrói o outro
        Destroy(gameObject);        // destrói este
        return;
    }

    // Tenta causar dano em EntityStatus
    EntityStatus alvo = other.GetComponent<EntityStatus>();
    if (alvo != null)
    {
        if (danoFisico > 0)
            alvo.ReceberDano(Stat.AtaqueFisico, danoFisico, atacante);
        if (danoFogo > 0)
            alvo.ReceberDano(Stat.AtaqueDeFogo, danoFogo, atacante);
        if (danoGelo > 0)
            alvo.ReceberDano(Stat.AtaqueDeGelo, danoGelo, atacante);
        if (danoEletrico > 0)
            alvo.ReceberDano(Stat.AtaqueEletrico, danoEletrico, atacante);
        
        Destroy(gameObject);
    }
}
}
