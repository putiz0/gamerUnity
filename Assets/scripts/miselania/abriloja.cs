using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 👈 IMPORTANTE

public class abriloja : MonoBehaviour
{
    public GameObject Ui_da_loja;
    public GameObject Texto_pra_abriAloja;

    GameObject Play_obj;

    public List<weopon> weopons_vendidos;

    public GameObject loja_bg;
    public GameObject loja_item;

    void Start()
    {
        RandonItems();
        Ui_da_loja.SetActive(false);

        Play_obj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float dis = Vector2.Distance(transform.position, Play_obj.transform.position);

        if (dis < 2f)
        {
            Texto_pra_abriAloja.SetActive(true);

            // ✅ NOVO INPUT SYSTEM
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Ui_da_loja.SetActive(true);
            }
        }
        else
        {
            Texto_pra_abriAloja.SetActive(false);
            Ui_da_loja.SetActive(false);
        }
    }

    void RandonItems()
    {
        // 🔥 limpa itens antigos (evita duplicação)
        foreach (Transform child in loja_bg.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            int randon_numero = Random.Range(0, weopons_vendidos.Count);

            GameObject new_loja_item = Instantiate(loja_item, loja_bg.transform);

            new_loja_item.GetComponent<LojaIten>().W_ = weopons_vendidos[randon_numero];
            new_loja_item.GetComponent<LojaIten>()
                .Setup(weopons_vendidos[randon_numero]);

            Debug.Log(randon_numero);
        }
    }
}