using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy this object on any collision
        if(collision.gameObject.tag != "EnnemyProjectile"){
        Destroy(this.gameObject);
        }
    }
}
