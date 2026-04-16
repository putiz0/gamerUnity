using System.Collections.Generic;
using UnityEngine;

public class CamaraBereive : MonoBehaviour
{
    public GameObject Tagter_Obg;
    Vector3 Tagter_transform;
    public GameObject player_Obg;
    public List<Transform> limites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_Obg = GameObject.FindGameObjectWithTag("Player");
        Tagter_Obg = player_Obg;
    }

    // Update is called once per frame
     void FixedUpdate()  
    {
        if ((player_Obg.transform.position.x > limites[0].position.x || player_Obg.transform.position.x < limites[1].position.x) && (player_Obg.transform.position.y > limites[2].position.y || player_Obg.transform.position.y < limites[3].position.y))
        {
            Tagter_transform = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, -10);
        }
        else if(player_Obg.transform.position.x > limites[0].position.x || player_Obg.transform.position.x < limites[1].position.x)
        {
            Tagter_transform = new Vector3 (gameObject.transform.position.x, Tagter_Obg.transform.position.y, -10);
        }
        else if(player_Obg.transform.position.y > limites[2].position.y || player_Obg.transform.position.y < limites[3].position.y) {
            Tagter_transform = new Vector3 (Tagter_Obg.transform.position.x, gameObject.transform.position.y, -10);
        } 
        else {
            Tagter_transform = new Vector3 (Tagter_Obg.transform.position.x, Tagter_Obg.transform.position.y, -10);
        }
        transform.position = Vector3.Lerp(this.transform.position, new Vector3(Tagter_transform.x, Tagter_transform.y, -10), 2f * Time.fixedDeltaTime);
    }
}
