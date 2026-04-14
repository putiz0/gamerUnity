using System.Collections;
using UnityEngine;

public class InimigoAtirador : MonoBehaviour
{
    public float velocidadeMovimento = 0f;
    public float velocidadeAtaque = 1f;
    public float danoFisico = 6f;
    public int Garregando_Ouro = 0;
    public int Garregando_Exp = 0;
    public float forcaDoProtetio = 5f;
    public GameObject player;
    public GameObject projecaoPrefab;
    private EntityStatus status;
    private bool podeAtacar = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        status = GetComponent<EntityStatus>();
    }
    void Start()
    {
         // Configura status do inimigo
        status.SetStat(Stat.VidaMaxima, 100f);
        status.SetStat(Stat.VidaAtual, status.GetStat(Stat.VidaMaxima));

        status.SetStat(Stat.AtaqueFisico, danoFisico);
        status.SetStat(Stat.AtaqueDeGelo,10f);
        status.SetStat(Stat.VelocidadeDeMovimento, velocidadeMovimento);

        status.SetStat(Stat.VelocidadeDeAtaque, velocidadeAtaque);
        status.SetStat(Stat.DistanciaDoAtaque, 10f);

        status.SetStat(Stat.DefesaFisica, 2f);
        status.SetStat(Stat.ResistenciaFisica, 0.1f); // 10%
        status.SetStat(Stat.CongelamentoChance, 0f);

        status.SetStat(Stat.Ouro, Garregando_Ouro);
        status.SetStat(Stat.Experiencia, Garregando_Exp);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() 
    {
        Atacar();
    }
      void Atacar()
    {
        if (!podeAtacar) return;
        if (player == null) return;

        float velocidadeAtaque = status.GetStat(Stat.VelocidadeDeAtaque);
        float delayAtaque = 1f / Mathf.Max(0.01f, velocidadeAtaque);

        StartCoroutine(CooldownAtaque(delayAtaque));

        Vector3 direcao = (player.transform.position - transform.position).normalized;

        GameObject proj = Instantiate(projecaoPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rbp = proj.GetComponent<Rigidbody2D>();
        rbp.linearVelocity = direcao * forcaDoProtetio; // velocidade fixa (melhor)

        Projetil p = proj.GetComponent<Projetil>();
        if (p != null)
        {
            p.dono = gameObject;
            p.atacante = GetComponent<EntityStatus>();

            // 🔥 NOVO SISTEMA
            p.danoFisico = status.GetStat(Stat.AtaqueFisico);
            p.danoFogo = status.GetStat(Stat.AtaqueDeFogo);
            p.danoGelo = status.GetStat(Stat.AtaqueDeGelo);
            p.danoEletrico = status.GetStat(Stat.AtaqueEletrico);

            p.distanciaMaxima = status.GetStat(Stat.DistanciaDoAtaque);
        }
    }

    IEnumerator CooldownAtaque(float time)
    {
        podeAtacar = false;
        yield return new WaitForSeconds(time);
        podeAtacar = true;
    }
}
