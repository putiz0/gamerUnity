using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float velocidadeMovimento = 2f;
    public float danoFisico = 50f;
    public float chanseCongelar = 0.0f;
    public int Garregando_Ouro = 0;
    public int Garregando_Exp = 0;

    private EntityStatus status;
    private Rigidbody2D rb;
    public EntityStatus atacante;
    private Transform player;

    void Awake()
    {
        status = GetComponent<EntityStatus>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Configura status do inimigo
        status.SetStat(Stat.VidaMaxima, 100f);
        status.SetStat(Stat.VidaAtual, status.GetStat(Stat.VidaMaxima));

        status.SetStat(Stat.AtaqueFisico, danoFisico);
        status.SetStat(Stat.AtaqueDeGelo,10f);
        status.SetStat(Stat.VelocidadeDeMovimento, velocidadeMovimento);

        status.SetStat(Stat.DefesaFisica, 2f);
        status.SetStat(Stat.ResistenciaFisica, 0.1f); // 10%
        status.SetStat(Stat.CongelamentoChance, chanseCongelar);

        status.SetStat(Stat.Ouro, Garregando_Ouro);
        status.SetStat(Stat.Experiencia, Garregando_Exp);

    }

    void FixedUpdate()
    {
        SeguirPlayer();
    }

    void SeguirPlayer()
    {
        if (player == null) return;

        float velocidade = status.GetStat(Stat.VelocidadeDeMovimento);
        if (status.EstaCongelado() && velocidade > 0f)
        {
            status.QueimaduraPorGelo(); 
            return;
        }
        
          // Calcula direção e movimento usando física
        Vector2 direcao = (player.position - transform.position).normalized;
        Vector2 novaPosicao = rb.position + direcao * velocidade * Time.fixedDeltaTime;
        rb.MovePosition(novaPosicao);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        EntityStatus statusPlayer = collision.gameObject.GetComponent<EntityStatus>();
        if (statusPlayer == null)
            return;

        float dano = status.GetStat(Stat.AtaqueFisico);

        statusPlayer.ReceberDano(Stat.AtaqueFisico, dano, status);
    }
}
