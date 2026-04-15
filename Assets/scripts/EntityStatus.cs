using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class EntityStatus : MonoBehaviour
{
    protected Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

    protected bool estaStunado = false;
    protected bool estaCongelado = false;
    float tempoUltimoDano;
    public float intervaloDanoCongelado = 0.5f;
    private float danoGeloArmazenado = 0f;
    public spamwMenege spamw_Menege;
    bool morreu = false;

    protected virtual void Awake()
    {
        stats[Stat.VidaMaxima] = 10f;
        stats[Stat.VidaAtual] = stats[Stat.VidaMaxima];
        stats[Stat.Escudo] = 0f;
        stats[Stat.Energia] = 0f;

        stats[Stat.AcertoCritico] = 0f;
        stats[Stat.DanoCritico] = 0.15f;
        stats[Stat.StunChance] = 0.05f;
        stats[Stat.BonusAtaqueFisicoPct] = 0f;
        stats[Stat.BonusVelocidadeAtaquePct] = 0f;
        stats[Stat.BonusDistanciaAtaquePct] = 0f;
        stats[Stat.BonusVelocidadeMovimentoPct] = 0f;
        stats[Stat.BonusRouboVidaPct] = 0f;
        stats[Stat.BonusCongelamentoChancePct] = 0f;
        stats[Stat.BonusCuraPct] = 0f;
        stats[Stat.BonusExperienciaPct] = 0f;

        stats[Stat.Nivel] = 1;
        stats[Stat.Experiencia] = 0f;
    }

    // ======================
    // DANO
    // ======================
    public void ReceberDano(Stat tipoAtaque, float dano, EntityStatus atacante = null)
    {
        VidaUI ui = VidaUI.GetActive();
        if (ui != null && ui.Popup != null)
        {
             GameObject new_Popup = Instantiate(ui.Popup, gameObject.transform.position, Quaternion.identity);
            Rigidbody2D popupRb = new_Popup.GetComponent<Rigidbody2D>();
            if (popupRb != null)
                popupRb.AddForce(new Vector2(Random.Range(-1.2f, 1.2f), 5), ForceMode2D.Impulse);

            Text popupTexto = new_Popup.GetComponentInChildren<Text>();
            if (popupTexto != null)
                popupTexto.text = dano.ToString("0.#");

            Destroy(new_Popup, 1f);
        }
        if (tipoAtaque == Stat.AtaqueVerdadeiro)
        {
            AplicarDanoFinal(dano, atacante);
            return;
        }

        Stat defesaStat;
        Stat resistenciaStat;

        switch (tipoAtaque)
        {
            case Stat.AtaqueFisico:
                defesaStat = Stat.DefesaFisica;
                resistenciaStat = Stat.ResistenciaFisica;
                break;

            case Stat.AtaqueDeFogo:
                defesaStat = Stat.DefesaDeFogo;
                resistenciaStat = Stat.ResistenciaMagica;
                break;

            case Stat.AtaqueDeGelo:
                defesaStat = Stat.DefesaDeGelo;
                resistenciaStat = Stat.ResistenciaMagica;
                break;

            case Stat.AtaqueEletrico:
                defesaStat = Stat.DefesaEletrico;
                resistenciaStat = Stat.ResistenciaMagica;
                break;

            default:
                defesaStat = Stat.DefesaFisica;
                resistenciaStat = Stat.ResistenciaFisica;
                break;
        }

        float defesa = GetStat(defesaStat);
        float resistencia = Mathf.Clamp01(GetStat(resistenciaStat));

        float danoFinal = dano - defesa;
        danoFinal *= (1f - resistencia);
        danoFinal = Mathf.Max(1f, danoFinal);

        AplicarDanoFinal(danoFinal, atacante);
    }

    // ======================
    // DANO FINAL (CRÍTICO + ESCUDO)
    // ======================
    protected void AplicarDanoFinal(float dano, EntityStatus atacante)
    {
        // 🎯 CRÍTICO
        if (atacante != null)
        {
            float chanceCritico = atacante.GetStat(Stat.AcertoCritico);
            float multCritico = atacante.GetStat(Stat.DanoCritico);

            if (Random.value <= chanceCritico)
            {
                dano *= multCritico;
            }
        }

        // 🛡️ ESCUDO
        float escudoAtual = GetStat(Stat.Escudo);

        if (escudoAtual > 0f)
        {
            float absorvido = Mathf.Min(escudoAtual, dano);
            AddStat(Stat.Escudo, -absorvido);
            dano -= absorvido;
        }

        if (dano > 0f)
        {
            AddStat(Stat.VidaAtual, -dano);
        }

        // ❤️ ROUBO DE VIDA
        if (atacante != null)
            atacante.AplicarRouboDeVida(dano);

        // ⚡ STUN
        if (atacante != null)
        {
            float stunChance = atacante.GetStat(Stat.StunChance);
            if (Random.value <= stunChance)
            {
                AplicarStun(1.5f);
            }
        }
        if (atacante != null)
        {
              float CongelamentoChance = atacante.GetStat(Stat.CongelamentoChance);
              CongelamentoChance *= 1f + Mathf.Max(0f, atacante.GetStat(Stat.BonusCongelamentoChancePct));
            
            // Verifica a chance
            if (Random.value <= CongelamentoChance)
            {
                // Pega o status de ataque de gelo do INIMIGO (atacante)
                float forcaDoGelo = atacante.GetStat(Stat.AtaqueDeGelo);
                
                // Aplica passando o dano
                AplicarCongelamento(3.0f, forcaDoGelo);
            }
        }

        VerificarMorte();
    }

    // ======================
    // STUN
    // ======================
    public void AplicarStun(float duracao)
    {
        if (estaStunado) return;
        StartCoroutine(StunCoroutine(duracao));
    }

    IEnumerator StunCoroutine(float tempo)
    {
        estaStunado = true;
        yield return new WaitForSeconds(tempo);
        estaStunado = false;
    }

    public bool EstaStunado()
    {
        return estaStunado;
    }


     public void AplicarCongelamento(float duracao, float danoBase)
    {
        // Se já está congelado, apenas reinicia o tempo (opcional)
        // Aqui vamos permitir atualizar o dano
        danoGeloArmazenado = danoBase; 

        if (estaCongelado) return; // Se quiser impedir re-congelar, pare aqui.

        estaCongelado = true;
        StartCoroutine(CongelamentoCoroutine(duracao));
        Debug.Log(name + " foi CONGELADO!");
    }

    IEnumerator CongelamentoCoroutine(float tempo)
    {
        estaCongelado = true;
        yield return new WaitForSeconds(tempo);
        estaCongelado = false;
        danoGeloArmazenado = 0f; // Limpa o dano ao descongelar
        Debug.Log(name + " descongelou.");
    }

    public bool EstaCongelado()
    {
        return estaCongelado;
    }

    public void QueimaduraPorGelo()
    {
        if (!estaCongelado) return;
        if (Time.time < tempoUltimoDano + intervaloDanoCongelado) return;

        tempoUltimoDano = Time.time;

        // Usa o dano armazenado do inimigo, e não o status de gelo da própria vítima
        // Se danoGeloArmazenado for 0 (ex: atacante não tinha gelo), usa um valor mínimo de 5
        float dano = Mathf.Max(5f, danoGeloArmazenado * 0.5f);
        
        Debug.Log("Queimadura de Gelo aplicada: " + dano);
        AddStat(Stat.VidaAtual, -dano);
        VerificarMorte(); // Importante verificar se morreu pela queimadura
    }

    // ======================
    // ROUBO DE VIDA
    // ======================
    protected void AplicarRouboDeVida(float danoCausado)
    {
        float roubo = GetStat(Stat.RouboDeVida);
        roubo *= 1f + Mathf.Max(0f, GetStat(Stat.BonusRouboVidaPct));
        if (roubo <= 0f) return;

        float cura = danoCausado * roubo;
        Curar(cura);
    }

    public void Curar(float valor)
    {
        valor *= 1f + Mathf.Max(0f, GetStat(Stat.BonusCuraPct));
        AddStat(Stat.VidaAtual, valor);
    }

    // ======================
    // STATS
    // ======================
    public float GetStat(Stat stat)
    {
        if (stats.TryGetValue(stat, out float valor))
            return valor;

        return 0f;
    }

    public void SetStat(Stat stat, float valor)
    {
        stats[stat] = valor;

        if (stat == Stat.VidaAtual || stat == Stat.VidaMaxima)
            ClampVida();
    }

    public void AddStat(Stat stat, float valor)
    {
        if (!stats.ContainsKey(stat))
            stats[stat] = 0f;

        stats[stat] += valor;

        if (stat == Stat.VidaAtual)
            ClampVida();
    }

    protected void ClampVida()
    {
        float vidaAtual = GetStat(Stat.VidaAtual);
        float vidaMax = GetStat(Stat.VidaMaxima);
        stats[Stat.VidaAtual] = Mathf.Clamp(vidaAtual, 0f, vidaMax);
    }
    
   public void AddExp(int exp_)
{
    stats[Stat.Experiencia] += exp_;
    if (stats[Stat.Experiencia] >= stats[Stat.Nivel] * 100)
    {
        stats[Stat.Experiencia] = 0;
        stats[Stat.Nivel]++;

        VidaUI ui = VidaUI.GetActive();
        if (ui != null)
        {
            ui.Info_up_Text();
        }
    }
}
       protected void VerificarMorte()
{

    if (morreu) return;

    if (GetStat(Stat.VidaAtual) <= 0f)
    {
        morreu = true;
        Morte();
    }
}

    protected virtual void Morte()
    {
        if (this.gameObject.tag != "Player")
        {
            inventarioMenagen.Instance.AddGolde(Mathf.RoundToInt(GetStat(Stat.Ouro)));
            EntityStatus player = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStatus>();
            float bonusExp = player.GetStat(Stat.BonusExperienciaPct);
            int expFinal = Mathf.RoundToInt(GetStat(Stat.Experiencia) * (1f + Mathf.Max(0f, bonusExp)));
            player.AddExp(expFinal);
        }
        if(this.gameObject.tag == "inimigo")
        {
            spamw_Menege.inimigos_live--;
        }
        Destroy(gameObject);
    }
}