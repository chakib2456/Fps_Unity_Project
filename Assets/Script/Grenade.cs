using UnityEngine;

public class Grenade : MonoBehaviour
{
   
    public float projectileSpeed = 10f;
    private Rigidbody rb;
    public GameObject explosionSFX;
    private int counter = 0;
    private float explosionDelay = 3f; // Time in seconds before the grenade explodes
    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * projectileSpeed;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(explosionDelay > 0)
        {
            explosionDelay -= Time.deltaTime;
        }
        else
        {
            // Explode the grenade
            Explode();
            
        }
    }
    
     public void Explode()
    {
      
            
           
           
            if(counter == 0)
            {
            GameObject instantiatedExplosionEffect = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(instantiatedExplosionEffect, .5f); // Destroy the explosion effect after 2
            GameObject instiatedExplosion = Instantiate(explosionSFX, transform.position, transform.rotation);
            Destroy(instiatedExplosion, 2f);
            counter++;
            }
             Destroy(this.gameObject); // Destroy the grenade object after a short delay
            
        
    }
}
