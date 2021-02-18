using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    List<Game.CoreBlock> targetsRemainig = new List<Game.CoreBlock>();
    [SerializeField]
    List<Game.CoreBlock> playersRemainig = new List<Game.CoreBlock>();


    public void CheckTargetsRemainig(Game.CoreBlock coreDestroyed)
    {
        targetsRemainig.Remove(coreDestroyed);
        playersRemainig.Remove(coreDestroyed);

        if (playersRemainig.Count <= 0)
        {
            Debug.Log("MISSION FAILED!!");
            Invoke("RestartScene", 3);
        }
        else if (targetsRemainig.Count <= 0)
        {
            Debug.Log("MISSION COMPLETE!!");
        }
    }

    public void RestartScene()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Scene_Name");
    }
}
