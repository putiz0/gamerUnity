using UnityEngine;
using UnityEngine.UI;

public class inimigoHPBar : MonoBehaviour
{
    [SerializeField] private Slider HP_Bar;
    private EntityStatus target;

    void Awake()
    {
        if (HP_Bar == null)
            HP_Bar = GetComponentInChildren<Slider>(true);

        target = GetComponentInParent<EntityStatus>();
    }

    void Update()
    {
        AtualizarVida();
    }

    void AtualizarVida()
    {
        if (target == null || HP_Bar == null) return;

        float atual = target.GetStat(Stat.VidaAtual);
        float max = target.GetStat(Stat.VidaMaxima);

        if (max <= 0f)
        {
            HP_Bar.value = 0f;
            return;
        }

        HP_Bar.value = Mathf.Clamp01(atual / max);
    }
}
