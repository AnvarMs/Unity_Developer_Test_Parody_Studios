using UnityEngine;

public class PointCubes : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerController>())
        {
            if(GameManager.instance.IsAllCollected()){
                UIManager.instance.OnWin();
                other.gameObject.SetActive(false);
            }
            else
            {
                GameManager.instance.OnCubeCollect();
            }
        }
    }
}
