using System;
using UnityEngine;

public class Alarm : MonoBehaviour
{
   public EnnemyBehavior [] ennemies;
   public GameManager manager;

   void Update(){

        if(manager.treasureBoxesCollected == 3)
        {
           foreach (EnnemyBehavior enemy in ennemies)
            {
                if (enemy != null)
                {
                    enemy.Alarmed = false;
                    enemy.isFleeing = true;
                }
            }
          
        }

   }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnnemyBehavior enemy in ennemies)
            {
                if (enemy != null)
                {
                    enemy.Alarmed = true;
                }
            }
            Debug.Log("Player entered Treasure Box trigger area.");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnnemyBehavior enemy in ennemies)
            {
                if (enemy != null)
                {
                    enemy.Alarmed = false;

                }
            }
            Debug.Log("Player exited Treasure Box trigger area.");
        }
    }

}
