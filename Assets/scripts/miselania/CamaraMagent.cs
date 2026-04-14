using UnityEngine;

public class CamaraMagent : MonoBehaviour
{
    public GameObject Camara_Magent_posiseon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        { 
                Camera.main.GetComponent<CamaraBereive>().Tagter_Obg = Camara_Magent_posiseon;
        }
    }
     private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        { 
                Camera.main.GetComponent<CamaraBereive>().Tagter_Obg = collision.gameObject;
        }
    }
}
