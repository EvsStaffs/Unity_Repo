using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 500 * Time.deltaTime);
    }

/*    private void OnTriggerEnter2D()
    {
        Destroy(gameObject);
        Debug.Log("Hit");
    }*/

    //private void OnTriggerEnter(Collider other)
    //{
    //    Destroy(gameObject);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        Destroy(gameObject);
    }


}
