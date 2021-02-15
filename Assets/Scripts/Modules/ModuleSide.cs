using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSide : MonoBehaviour
{
    [SerializeField]
    private Module moduleParent = null;
    [SerializeField]
    private Transform dockTransform = null;

    public bool connected = false;

    public Module ModuleParent
    {
        get
        {
            return moduleParent;
        }
    }

    public Vector2 MiddlePointGlobal
    {
        get
        {
            return dockTransform.position;
        }
    }

    public Vector2 NormalDirectionGlobal
    {
        get
        {
            return dockTransform.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(connected == false)
        {
            ModuleSide otherModuleSide = collision.GetComponent<ModuleSide>();

            if (otherModuleSide)
            {
                moduleParent.InteractionBetweenModulesSides(this, otherModuleSide);
            }
        }
    }
}
