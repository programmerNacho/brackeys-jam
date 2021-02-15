using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSide : MonoBehaviour
{
    [SerializeField]
    private Module moduleParent = null;
    [SerializeField]
    private Transform dockTransform = null;

    private Vector2 middlePointGlobal = Vector2.zero;
    private Vector2 normalDirectionGlobal = Vector2.zero;

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
            return middlePointGlobal;
        }
    }

    public Vector2 NormalDirectionGlobal
    {
        get
        {
            return normalDirectionGlobal;
        }
    }

    private void Start()
    {
        middlePointGlobal = dockTransform.position;
        normalDirectionGlobal = dockTransform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ModuleSide otherModuleSide = collision.GetComponent<ModuleSide>();

        if(otherModuleSide)
        {
            moduleParent.InteractionBetweenModulesSides(this, otherModuleSide);
        }
    }
}
