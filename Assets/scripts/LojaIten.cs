using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class LojaIten : MonoBehaviour
{
    public weopon W_;

    public Text nome_intem;
    public Text preco_intem;
    public Image icon_intem;
    public Text inf_intem;
    public Button loja_button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        loja_button.interactable = inventarioMenagen.Instance.gold_coins >= W_.weopon_preco;
    }
    public void Setup(weopon w)
    {
        nome_intem.text = w.weopon_nome;
        preco_intem.text = w.weopon_preco.ToString();
        icon_intem.sprite = w.weopon_icon;
        inf_intem.text = "ATK DANO: " + w.danoFisico.ToString() + "\n\nATK VELOCIDADE: " + w.weopon_speed.ToString() + "\n\nATK DISTANCIA: " + w.weopon_distancia.ToString();
        Debug.Log("Item recebido: " + w.weopon_nome);
        if (inventarioMenagen.Instance.gold_coins < w.weopon_preco)
        {
            loja_button.interactable = false;
        }
        else
        {
            loja_button.interactable = true;
        }
    }
    public void CompraArma()
    {
        if(inventarioMenagen.Instance.invetario_[6] != null)
        {

        }
        else
        {
            for(int i = 0; i < 6; i++) {
                if (inventarioMenagen.Instance.invetario_[i] == null)
                {
                    inventarioMenagen.Instance.invetario_[i] = W_;
                    break;
                }
            }
            inventarioMenagen.Instance.AddGolde(W_.weopon_preco * -1);
            Destroy(this.gameObject);
        }
        
    }
    void RefresfLoja()
    {
        GameObject[] loja_buttons = GameObject.FindGameObjectsWithTag("itenDaLoja");
        foreach(GameObject go in loja_buttons) {
            go.GetComponent<LojaIten>().Setup(GetComponent<LojaIten>().W_);
        }
    }
}
