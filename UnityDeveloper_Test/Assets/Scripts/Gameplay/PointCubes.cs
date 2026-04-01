using UnityEngine;

public class PointCubes : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerController>())
        {
           
        
                GameManager.instance.OnCubeCollect();
                UIManager.instance.UpdateText();
                gameObject.SetActive(false);
                
             if(GameManager.instance.IsAllCollected()){
                UIManager.instance.OnWin();
                other.gameObject.SetActive(false);
            }
        }
    }
}
