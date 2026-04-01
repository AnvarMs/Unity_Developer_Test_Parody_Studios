using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
public static UIManager instance;

    void Awake()
    {
        if(instance!=null)Destroy(this);
        else instance = this;
    }
    public Text countText;
    public Text timerText;

    public GameObject winPanel;
    public GameObject losePanel;
    public void UpdateText()
    {
       int n =  GameManager.instance.GetCollectedCount();
        int x =GameManager.instance.GetTotalCubeCound();
       countText.text = $"{n}/{x}"; 
    }
    public void UpdateTimerText(string value)
    {
        timerText.text = value;
    }
    
    public void OnWin()
    {
        winPanel.SetActive(true);
    }
    public void OnLoose()
    {
        losePanel.SetActive(true);
    }
}
