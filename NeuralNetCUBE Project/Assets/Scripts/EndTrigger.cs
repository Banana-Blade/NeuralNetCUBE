using UnityEngine;

public class EndTrigger : MonoBehaviour
{

    public GameManager gameManager;

    void OnTriggerEnter()
    {
        // eventually ask which Object it was!
        Debug.Log("END Trigger hit!");
        gameManager.CompleteLevel();
    }

}
