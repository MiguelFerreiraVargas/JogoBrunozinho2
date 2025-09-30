using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void EscolherUmPlayer()
    {
        PlayerPrefs.SetInt("Players", 1);
    }

    public void EscolherDoisPlayers()
    {
        PlayerPrefs.SetInt("Players", 2);
    }
}