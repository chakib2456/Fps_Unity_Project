using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public GameManager manager;
    public EnnemyBehavior [] ennemies;
    private void OnDestroy()
    {
        GameManager gameManager = manager.GetComponent<GameManager>();
        if(manager != null)
        {
            foreach (EnnemyBehavior enemy in ennemies)
            {
                if (enemy != null && enemy.isFleeing == false)
                {
                    enemy.Alarmed = true;
                }
                else if (manager.treasureBoxesCollected >= 3)
                {
                    enemy.Alarmed = false;
                }
            }
            manager.TreasureBoxCollected();
        }
    }
}
