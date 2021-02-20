using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Menus menus = null;
    [SerializeField]
    private float waitToOpenMenu = 3;

    [Header("Cores in game")]
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
            Invoke("ShowMenuFail", waitToOpenMenu);
        }
        else if (targetsRemainig.Count <= 0)
        {
            Invoke("ShowMenuWin", waitToOpenMenu);
        }
    }

    public void ShowMenuFail()
    {
        menus.SetCurrentMenu(Menus.Menu.fail);
    }
    public void ShowMenuWin()
    {
        menus.SetCurrentMenu(Menus.Menu.win);
    }
}
