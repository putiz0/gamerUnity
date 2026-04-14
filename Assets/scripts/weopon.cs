using UnityEngine;

[CreateAssetMenu(fileName = "weopon")]
public class weopon : ScriptableObject
{
    public string weopon_nome;
    public int weopon_preco;

    [Header("Danos")]
    public float danoFisico;
    public float danoFogo;
    public float danoGelo;
    public float danoEletrico;

    [Header("Combate")]
    public float weopon_speed;
    public float weopon_distancia;

    [Header("Status (Opcional)")]
    public float bonusStunChance;
    public float bonusCongelamentoChance;

    [Header("Visual")]
    public Sprite weopon_icon;
}