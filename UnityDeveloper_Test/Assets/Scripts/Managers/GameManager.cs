using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int cubesToCollect= 5;
    private int collected=0; 
    void Awake()
    {
        if(instance==null)instance=this;
        else Destroy(this);
    }

    

    public void OnCubeCollect()=>collected+=1;

    public bool IsAllCollected()=>collected==cubesToCollect;

    public int GetCollectedCount()=>collected;
    public int GetTotalCubeCound()=>cubesToCollect;
    public void StartGame()=>collected =0;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
