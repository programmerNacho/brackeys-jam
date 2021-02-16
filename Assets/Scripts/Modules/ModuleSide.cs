using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSide : MonoBehaviour
{
    [SerializeField]
    private Block moduleParent = null;
    [SerializeField]
    private Transform dockTransform = null;

    public Block ModuleParent
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
        ModuleSide otherModuleSide = collision.GetComponent<ModuleSide>();

        if (moduleParent.CanDock())
        {
            if (otherModuleSide && otherModuleSide.moduleParent != moduleParent && otherModuleSide.moduleParent.CanDock())
            {
                moduleParent.InteractionBetweenModulesSides(this, otherModuleSide);
            }
        }
    }
}
