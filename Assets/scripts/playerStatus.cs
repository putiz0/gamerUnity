using UnityEngine;

public class playerStatus : EntityStatus
{
    [Header("Sobrevivência")]
    public float vidaMaxima = 100f;
    public float energia = 0f;
    public float escudo = 0f;

    [Header("Ataque")]
    public float ataqueFisico = 15f;
    public float velocidadeDeAtaque = 1.34f;
    public float distanciaDoAtaque = 0.2f;
   
    [Header("Defesas")]
    public float defesaFisica = 10f;

    [Header("Movimento")]
    public float velocidadeDeMovimento = 1f;
    [Header("Mecanica")]
    public float RouboDeVida;

    [Header("Elemental")]
    public float ataqueDeGelo = 0f;

    [Header("Progressao")]
    public int Nivel = 1;
    public int Experiencia = 0;

    protected override void Awake()
    {
        base.Awake(); // 🔥 MUITO IMPORTANTE

        // Sobrevivência
        SetStat(Stat.VidaMaxima, vidaMaxima);
        SetStat(Stat.VidaAtual, vidaMaxima);
        SetStat(Stat.Energia, energia);
        SetStat(Stat.Escudo, escudo);
        SetStat(Stat.DefesaFisica, defesaFisica);

        // Ataque
        SetStat(Stat.AtaqueFisico, ataqueFisico);
        SetStat(Stat.VelocidadeDeAtaque, velocidadeDeAtaque);
        SetStat(Stat.DistanciaDoAtaque, distanciaDoAtaque);

        // Movimento
        SetStat(Stat.VelocidadeDeMovimento, velocidadeDeMovimento);

        // Mecanica 
        SetStat(Stat.RouboDeVida, RouboDeVida);

        // Elemental
        SetStat(Stat.AtaqueDeGelo, ataqueDeGelo);

        SetStat(Stat.Nivel, Nivel);
        SetStat(Stat.Experiencia, Experiencia);
    }

    void Update()
    {
        Nivel = Mathf.RoundToInt(GetStat(Stat.Nivel));
        Experiencia = Mathf.RoundToInt(GetStat(Stat.Experiencia));
    }
}
