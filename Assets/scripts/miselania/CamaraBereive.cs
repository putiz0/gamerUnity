using UnityEngine;

public class CamaraBereive : MonoBehaviour
{
    public GameObject Tagter_Obg;
    public GameObject player_Obg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_Obg = GameObject.FindGameObjectWithTag("Player");
        Tagter_Obg = player_Obg;
    }

    // Update is called once per frame
     void FixedUpdate()  
    {
        transform.position = Vector3.Lerp(this.transform.position, new Vector3(Tagter_Obg.transform.position.x, Tagter_Obg.transform.position.y, -10), 2f * Time.fixedDeltaTime);
    }
}
