using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
  public PlayerMovement player;
  public GameObject[] ennemiesToTrack;
  public GameObject [] treasureBoxesToTrack;
  public int destroyedCount = 0;
  public int treasureBoxesCollected = 0;

  public TextMeshProUGUI killText;
  public TextMeshProUGUI treasureText;
    void Start()
    {

      if(ennemiesToTrack == null || ennemiesToTrack.Length == 0)
      {
         Debug.LogWarning("No Ennemies assigned to track!");
      }
      foreach(GameObject obj in ennemiesToTrack)
        {
            if(obj != null)
            {
                var tracker = obj.GetComponent<EnnemyBehavior>();
                if(tracker != null)
                {
                    tracker.manager = this; // give object reference to manager
                }
            }
        }

      if(treasureBoxesToTrack == null || treasureBoxesToTrack.Length == 0)
      {
         Debug.LogWarning("No Treasure Boxes assigned to track!");
      }
      foreach(GameObject box in treasureBoxesToTrack)
        {
            if(box != null)
            {
                var tracker = box.GetComponent<TreasureBox>();
                if(tracker != null)
                {
                   tracker.manager = this; // give object reference to manager
                }
            }
        }
    }

       
    

    void Update()
    {
       if(player.currentHealth <= 0)
       {
        SceneManager.LoadScene("EndGame");
       }
        if (treasureBoxesCollected >= 3 && destroyedCount >= ennemiesToTrack.Length)
        {
            Debug.Log("You collected all treasure boxes! You win!");
            SceneManager.LoadScene("Win");
        }
        killText.text = "Enemies Killed: " + destroyedCount + " / " + ennemiesToTrack.Length;
        treasureText.text = "Treasure Boxes: " + treasureBoxesCollected + " / " + "3";
    }

    public void ObjectDestroyed()
    {
        destroyedCount++;
        Debug.Log("Destroyed objects: " + destroyedCount);
    }
    public void TreasureBoxCollected()
    {
        treasureBoxesCollected++;
        Debug.Log("Treasure Boxes collected: " + treasureBoxesCollected);
    }


}
