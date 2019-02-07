using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Transform player;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = (player.position.z / 10f).ToString("0") + "%";
        if(player.position.z>=1000)
        {
            scoreText.text = "100%";
        }
    }
}
