using System;
using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    public static VidaUI Instance;

    public EntityStatus target;
    public Slider slider_Hp;
    public Slider slider_Exp;

    [Header("Nivel Up")]
    public GameObject Nivel_up_tela;
    public Text[] Info_up;
    public GameObject Popup;

    [Header("Bonus")]
    public float aumentoHpMaximo = 10f;
    public float aumentoAtkPct = 0.10f;
    public float aumentoVelAtkPct = 0.10f;
    public float aumentoDistAtkPct = 0.10f;
    public int aumentoDef = 1;
    public float aumentoVelMovPct = 0.10f;
    public float aumentoRouHpPct = 0.10f;
    public float aumentoChaCongPct = 0.10f;
    public float aumentoAumCuraPct = 0.10f;
    public float aumentoExpPct = 0.10f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (target == null)
            target = FindFirstObjectByType<playerStatus>();

        if (target == null)
            target = GetComponentInParent<EntityStatus>();

        Slider[] sliders = GetComponentsInChildren<Slider>(true);
        if (slider_Hp == null && sliders.Length > 0)
            slider_Hp = sliders[0];
        if (slider_Exp == null && sliders.Length > 1)
            slider_Exp = sliders[1];

        if (Info_up == null || Info_up.Length == 0)
            Info_up = GetComponentsInChildren<Text>(true);
    }

    void Update()
    {
        AtualizarVida();
        AtualizarExp();
    }

    void AtualizarVida()
    {
        if (target == null || slider_Hp == null) return;

        float atual = target.GetStat(Stat.VidaAtual);
        float max = target.GetStat(Stat.VidaMaxima);

        if (max <= 0f)
        {
            slider_Hp.value = 0f;
            return;
        }

        slider_Hp.value = Mathf.Clamp01(atual / max);
    }

    void AtualizarExp()
    {
        if (target == null || slider_Exp == null) return;

        float nivel = Mathf.Max(1f, target.GetStat(Stat.Nivel));
        float expMax = nivel * 100f;
        float expAtual = target.GetStat(Stat.Experiencia);

        slider_Exp.maxValue = expMax;
        slider_Exp.value = Mathf.Clamp(expAtual, 0f, expMax);
    }

    public void Info_up_Text()
    {
        if (Nivel_up_tela != null)
            Nivel_up_tela.SetActive(true);

        if (Info_up == null || Info_up.Length < 10)
            return;

        Info_up[0].text = "+" + aumentoHpMaximo.ToString("0");
        Info_up[1].text = "+" + (aumentoAtkPct * 100f).ToString("0") + "%";
        Info_up[2].text = "+" + (aumentoVelAtkPct * 100f).ToString("0") + "%";
        Info_up[3].text = "+" + (aumentoDistAtkPct * 100f).ToString("0") + "%";
        Info_up[4].text = "+" + aumentoDef.ToString();
        Info_up[5].text = "+" + (aumentoVelMovPct * 100f).ToString("0") + "%";
        Info_up[6].text = "+" + (aumentoRouHpPct * 100f).ToString("0") + "%";
        Info_up[7].text = "+" + (aumentoChaCongPct * 100f).ToString("0") + "%";
        Info_up[8].text = "+" + (aumentoAumCuraPct * 100f).ToString("0") + "%";
        Info_up[9].text = "+" + (aumentoExpPct * 100f).ToString("0") + "%";
    }

    public void FecharNivelUp()
    {
        if (Nivel_up_tela != null)
            Nivel_up_tela.SetActive(false);
    }

    public void SelectStat(string statu)
    {
        if (target == null || string.IsNullOrWhiteSpace(statu))
            return;

        switch (statu.ToLowerInvariant())
        {
            case "hp":
                target.AddStat(Stat.VidaMaxima, aumentoHpMaximo);
                target.SetStat(Stat.VidaAtual, target.GetStat(Stat.VidaMaxima));
                break;
            case "atk":
                target.AddStat(Stat.AtaqueFisico, target.GetStat(Stat.AtaqueFisico) * aumentoAtkPct);
                break;
            case "velatk":
                target.AddStat(Stat.VelocidadeDeAtaque, target.GetStat(Stat.VelocidadeDeAtaque) * aumentoVelAtkPct);
                break;
            case "disatk":
                target.AddStat(Stat.DistanciaDoAtaque, target.GetStat(Stat.DistanciaDoAtaque) * aumentoDistAtkPct);
                break;
            case "def":
                target.AddStat(Stat.DefesaFisica, aumentoDef);
                break;
            case "velm":
                target.AddStat(Stat.VelocidadeDeMovimento, target.GetStat(Stat.VelocidadeDeMovimento) * aumentoVelMovPct);
                break;
            case "rouhp":
                target.AddStat(Stat.RouboDeVida, aumentoRouHpPct);
                break;
            case "chacong":
                target.AddStat(Stat.CongelamentoChance, aumentoChaCongPct);
                break;
            case "aumcura":
                target.AddStat(Stat.BonusCuraPct, aumentoAumCuraPct);
                break;
            case "exp":
                target.AddStat(Stat.BonusExperienciaPct, aumentoExpPct);
                break;
        }

        FecharNivelUp();
    }

    public void SelectStat(string statu, float valor)
    {
        if (target == null || string.IsNullOrWhiteSpace(statu))
            return;

        switch (statu.ToLowerInvariant())
        {
            case "hp":
                target.AddStat(Stat.VidaMaxima, valor);
                target.SetStat(Stat.VidaAtual, target.GetStat(Stat.VidaMaxima));
                break;
            case "atk":
                target.AddStat(Stat.AtaqueFisico, valor);
                break;
            case "velatk":
                target.AddStat(Stat.VelocidadeDeAtaque, valor);
                break;
            case "disatk":
                target.AddStat(Stat.DistanciaDoAtaque, valor);
                break;
            case "def":
                target.AddStat(Stat.DefesaFisica, valor);
                break;
            case "velm":
                target.AddStat(Stat.VelocidadeDeMovimento, valor);
                break;
            case "rouhp":
                target.AddStat(Stat.RouboDeVida, valor);
                break;
            case "chacong":
                target.AddStat(Stat.CongelamentoChance, valor);
                break;
            case "aumcura":
                target.AddStat(Stat.BonusCuraPct, valor);
                break;
            case "exp":
                target.AddStat(Stat.BonusExperienciaPct, valor);
                break;
        }

        FecharNivelUp();
    }

    public void AumentarHpMaximo() => SelectStat("hp");
    public void AumentarHpMaximo10() => SelectStat("hp");
    public void AumentarAtk() => SelectStat("atk");
    public void AumentarVelAtk() => SelectStat("velatk");
    public void AumentarDistAtk() => SelectStat("disatk");
    public void AumentarDef() => SelectStat("def");
    public void AumentarVelMov() => SelectStat("velm");
    public void AumentarRouHp() => SelectStat("rouhp");
    public void AumentarChaCong() => SelectStat("chacong");
    public void AumentarAumCura() => SelectStat("aumcura");
    public void AumentarExp() => SelectStat("exp");

    internal static VidaUI GetActive()
    {
        return Instance;
    }

}
