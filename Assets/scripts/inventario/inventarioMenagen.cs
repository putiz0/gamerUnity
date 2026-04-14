using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class inventarioMenagen : MonoBehaviour
{
    public static inventarioMenagen Instance { get; private set; }

    public playerStatus player_status;

    public GameObject inv_Fundo;
    public GameObject inv_slot;
    public int Active_slot;

    public List<weopon> invetario_;

    int selected_slot = 0;

    public float gold_coins;
    public Text gold_text;

    // 🧠 guardar arma atual
    private weopon armaAtual;

     private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        player_status = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();

        if (invetario_.Count > 0)
            Selecweopon(1);
        else
            RefrecheInventario();
    }

    void Update()
    {
        InvetarioSelec();
    }

    void RefrecheInventario()
    {
        gold_text.text = gold_coins.ToString();

        foreach (Transform child in inv_Fundo.transform)
        {
            Destroy(child.gameObject);
        }

        int hotkey_ = 1;

        foreach (weopon w in invetario_)
        {
            GameObject slot_intance = Instantiate(inv_slot, inv_Fundo.transform);

            Image img = slot_intance.GetComponentInChildren<Image>();
            Outline outline = slot_intance.GetComponentInChildren<Outline>();

            if (w == null)
            {
                img.enabled = false;
            }
            else
            {
                img.enabled = true;
                img.sprite = w.weopon_icon;

                outline.enabled = (selected_slot == hotkey_);
            }

            slot_intance.GetComponentInChildren<Text>().text = hotkey_.ToString();
            hotkey_++;
        }
    }

    void Selecweopon(int hotkey_)
    {
        if (hotkey_ - 1 >= invetario_.Count) return;
        Active_slot = hotkey_ -1;

        weopon arma = invetario_[hotkey_ - 1];

        // 🔥 RESETA stats antes (IMPORTANTE)
        ResetStats();

        if (arma != null)
        {
            armaAtual = arma;

            // 💥 DANOS
            player_status.SetStat(Stat.AtaqueFisico, arma.danoFisico);
            player_status.SetStat(Stat.AtaqueDeFogo, arma.danoFogo);
            player_status.SetStat(Stat.AtaqueDeGelo, arma.danoGelo);
            player_status.SetStat(Stat.AtaqueEletrico, arma.danoEletrico);

            // ⚔️ COMBATE
            player_status.SetStat(Stat.VelocidadeDeAtaque, arma.weopon_speed);
            player_status.SetStat(Stat.DistanciaDoAtaque, arma.weopon_distancia);

            // ❄️ STATUS
            player_status.AddStat(Stat.StunChance, arma.bonusStunChance);
            player_status.AddStat(Stat.CongelamentoChance, arma.bonusCongelamentoChance);
        }

        selected_slot = hotkey_;
        RefrecheInventario();
    }

    // 🧠 MUITO IMPORTANTE
    void ResetStats()
    {
        player_status.SetStat(Stat.AtaqueFisico, 0);
        player_status.SetStat(Stat.AtaqueDeFogo, 0);
        player_status.SetStat(Stat.AtaqueDeGelo, 0);
        player_status.SetStat(Stat.AtaqueEletrico, 0);

        player_status.SetStat(Stat.StunChance, 0);
        player_status.SetStat(Stat.CongelamentoChance, 0);
    }

    void InvetarioSelec()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.digit1Key.wasPressedThisFrame) Selecweopon(1);
        if (keyboard.digit2Key.wasPressedThisFrame) Selecweopon(2);
        if (keyboard.digit3Key.wasPressedThisFrame) Selecweopon(3);
        if (keyboard.digit4Key.wasPressedThisFrame) Selecweopon(4);
        if (keyboard.digit5Key.wasPressedThisFrame) Selecweopon(5);
        if (keyboard.digit6Key.wasPressedThisFrame) Selecweopon(6);
        if (keyboard.digit7Key.wasPressedThisFrame) Selecweopon(7);
    }

    public void AddGolde(int g)
    {
        gold_coins += g;
        RefrecheInventario();
    }
    public void DescateItem () {
        if (Active_slot != 0)
        {
            invetario_[Active_slot] = null;
            Selecweopon(1);
            RefrecheInventario();
        }
    }
}