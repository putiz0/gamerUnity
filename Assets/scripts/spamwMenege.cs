using System.Collections.Generic;
using UnityEngine;

public class spamwMenege : MonoBehaviour
{
    public int Ouro_ganho;
    public GameObject porta_;
    public List<GameObject> SpamwPoint;
    public List<GameObject> inimigos;
    public int inimigos_live = 0;
    bool danjoAtiva = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkDanjoEnd();
    }
    private void OnTriggerEnter2D (Collider2D Collision) {
        if(Collision.gameObject.tag == "Player")
        {
            porta_.SetActive(true);
            SpamwInimigo();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            danjoAtiva = true;
        }
        
    }
    void SpamwInimigo()
    {
        foreach(GameObject sp in SpamwPoint) {
            int randoImigigo = Random.Range(0,2);
            GameObject new_inimigo = Instantiate(inimigos[randoImigigo], sp.transform.position, Quaternion.identity);
            new_inimigo.GetComponent<EntityStatus>().spamw_Menege = this;
            inimigos_live++;
        }
    }
    void checkDanjoEnd()
    {
        if(danjoAtiva == true)
        {
            if(inimigos_live == 0)
            {
              FinalizarDanjo();
            }
        }
    }

    void FinalizarDanjo()
    {
        danjoAtiva = false;
        porta_.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        if (inventarioMenagen.Instance != null)
        {
            inventarioMenagen.Instance.AddGolde(Ouro_ganho);
        }
    }
}
