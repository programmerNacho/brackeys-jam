using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSideLobo : MonoBehaviour
{
    [SerializeField]
    private ModuleLobo moduleParent = null;

    public bool canDock = true;

    public void SetModule(ModuleLobo module)
    {
        moduleParent = module;
    }
    public ModuleLobo GetModule()
    {
        return moduleParent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDock)
        {
            ModuleSideLobo otherSide = null;
            if (collision.TryGetComponent<ModuleSideLobo>(out otherSide))
            {
                moduleParent.DockCheck(this, otherSide);
            }
        }
    }
}
