using UnityEngine;
using UnityEngine.InputSystem;

public class Anda : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AWSDmove();
    }

   void AWSDmove()
{
    if (Input.GetKey(KeyCode.W))
    {
        transform.position += new Vector3(0, 0.02f, 0);
    }
    
    if (Input.GetKey(KeyCode.S))
    {
        transform.position += new Vector3(0, -0.02f, 0);
    }
    
    if (Input.GetKey(KeyCode.A))
    {
        transform.position += new Vector3(-0.02f, 0, 0);
    }
    
    if (Input.GetKey(KeyCode.D))
    {
        transform.position += new Vector3(0.02f, 0, 0);
    }
}


}
