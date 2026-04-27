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
    private Player playerController;
    private GameObject projectilePadrao;

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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player_status = playerObj.GetComponent<playerStatus>();
            playerController = playerObj.GetComponent<Player>();
            if (playerController != null)
            {
                projectilePadrao = playerController.projecaoPrefab;
            }
        }

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
        if (gold_text != null)
        {
            gold_text.text = gold_coins.ToString();
        }

        foreach (Transform child in inv_Fundo.transform)
        {
            Destroy(child.gameObject);
        }

        int hotkey_ = 1;

        foreach (weopon w in invetario_)
        {
            GameObject slot_intance = Instantiate(inv_slot, inv_Fundo.transform);

            Transform fundoTransform = slot_intance.transform.Find("fundo");
            Transform iconTransform = slot_intance.transform.Find("icon");
            Transform hotkeyTransform = slot_intance.transform.Find("icon/Hotkey");

            Image fundoImage = fundoTransform != null ? fundoTransform.GetComponent<Image>() : null;
            Image iconImage = iconTransform != null ? iconTransform.GetComponent<Image>() : null;
            Outline outline = slot_intance.GetComponentInChildren<Outline>();
            bool isSelected = selected_slot == hotkey_;

            if (w == null)
            {
                if (fundoImage != null)
                {
                    fundoImage.color = isSelected ? Color.yellow : Color.white;
                }

                if (iconImage != null)
                {
                    iconImage.enabled = true;
                    iconImage.color = new Color(1f, 1f, 1f, 0f);
                }
            }
            else
            {
                if (fundoImage != null)
                {
                    fundoImage.color = isSelected ? Color.yellow : Color.wheat;
                }

                if (iconImage != null)
                {
                    iconImage.enabled = true;
                    iconImage.sprite = w.weopon_icon;
                    iconImage.color = Color.white;
                }

            }

            if (outline != null)
            {
                outline.enabled = isSelected;
            }

            if (hotkeyTransform != null)
            {
                Text hotkeyText = hotkeyTransform.GetComponent<Text>();
                if (hotkeyText != null)
                {
                    hotkeyText.text = hotkey_.ToString();
                }
            }
            hotkey_++;
        }
    }

    void Selecweopon(int hotkey_)
    {
        if (hotkey_ - 1 >= invetario_.Count) return;
        Active_slot = hotkey_ -1;

        weopon arma = invetario_[hotkey_ - 1];

        // 🔥 RESETA stats antes (IMPORTANTE)
        if (arma != null)
        {
            armaAtual = arma;
            AplicarStatsBasePlayer();
            AplicarStatsArma(arma);
        }
        else
        {
            armaAtual = null;
            AplicarStatsBasePlayer();

            if (playerController != null)
            {
                playerController.projecaoPrefab = projectilePadrao;
            }
        }

        selected_slot = hotkey_;
        RefrecheInventario();
    }

    void AplicarStatsBasePlayer()
    {
        if (player_status == null) return;

        player_status.SetStat(Stat.AtaqueFisico, player_status.ataqueFisico);
        player_status.SetStat(Stat.AtaqueDeFogo, 0f);
        player_status.SetStat(Stat.AtaqueDeGelo, player_status.ataqueDeGelo);
        player_status.SetStat(Stat.AtaqueEletrico, 0f);

        player_status.SetStat(Stat.VelocidadeDeAtaque, player_status.velocidadeDeAtaque);
        player_status.SetStat(Stat.DistanciaDoAtaque, player_status.distanciaDoAtaque);
        player_status.SetStat(Stat.VelocidadeDeMovimento, player_status.velocidadeDeMovimento);
        player_status.SetStat(Stat.RouboDeVida, player_status.RouboDeVida);

        player_status.SetStat(Stat.StunChance, 0f);
        player_status.SetStat(Stat.CongelamentoChance, 0f);
    }

    void AplicarStatsArma(weopon arma)
    {
        if (player_status == null || arma == null) return;

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

        if (playerController != null)
        {
            playerController.projecaoPrefab = arma.weopon_projetion;
        }
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
