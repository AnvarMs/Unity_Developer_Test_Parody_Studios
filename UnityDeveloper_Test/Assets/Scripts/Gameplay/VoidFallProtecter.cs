using UnityEngine;

public class VoidFallProtecter : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {

            UIManager.instance.OnLoose("You fell into the void!");
        
    }
}
