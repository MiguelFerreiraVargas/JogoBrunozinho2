using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    void Start()
    {
        int players = PlayerPrefs.GetInt("Players", 1);


        if (players == 2)
        {
            Destroy(player2Prefab);
        }
    }
}
