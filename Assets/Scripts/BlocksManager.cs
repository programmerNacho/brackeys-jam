using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlocksManager : MonoBehaviour
{
    private List<Game.Block> blocksInLevel = new List<Game.Block>();
    public int currentGroup = 0;

    public UnityEvent OnBlockConnect = new UnityEvent();
    public UnityEvent OnBlockDisconnect = new UnityEvent();
    public UnityEvent OnBlockDestroy = new UnityEvent();
    public UnityEvent OnBlockCreate = new UnityEvent();

    public int AddBlockAndGetGroup(Game.Block block)
    {
        blocksInLevel.Add(block);
        currentGroup++;
        return currentGroup;
    }

    public void RemoveBlock(Game.Block block)
    {
        blocksInLevel.Remove(block);
    }
}
