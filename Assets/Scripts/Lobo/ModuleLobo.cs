using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleLobo : MonoBehaviour
{
    [SerializeField]
    private ModuleSideLobo[] sides = new ModuleSideLobo[0];

    public List<ModuleLobo> childrenList = new List<ModuleLobo>();

    public ModuleLobo oldParent = null;

    private void Start()
    {
        SetModuleInSides();
    }

    private void SetModuleInSides()
    {
        foreach (var side in sides)
        {
            side.SetModule(this);
        }
    }

    public void SetSideCanDock(bool canDock)
    {
        foreach (var side in sides)
        {
            side.canDock = canDock;
        }
    }

    public void DockCheck(ModuleSideLobo mySide, ModuleSideLobo otherSide)
    {
        InsertNewModule(mySide, otherSide);
    }
    private void InsertNewModule(ModuleSideLobo mySide, ModuleSideLobo otherSide)
    {
        ModuleLobo otherModule = otherSide.GetModule();
        otherModule.SetSideCanDock(false);
        NewHierarchy(otherSide.GetModule());
    }

    private void NewHierarchy(ModuleLobo otherModule)
    {
        SaveParents(otherModule);

        otherModule.transform.parent = this.transform;

        RecalculateHierarchy(otherModule);

        childrenList.Add(otherModule);
    }

    private void RecalculateHierarchy(ModuleLobo module)
    {
        if (module != null)
        {
            ModuleLobo parent = module.transform.parent.GetComponent<ModuleLobo>();

            module.childrenList.Remove(parent);

            Debug.Log(module.oldParent);
            if (module.oldParent != null)
            {
                module.childrenList.Add(module.oldParent);
                module.oldParent.transform.parent = module.transform;
                RecalculateHierarchy(module.oldParent);
            }
        }
    }

    private void SaveParents(ModuleLobo module)
    {
        module.SetOldParent();
        foreach (var item in module.GetComponentsInChildren<ModuleLobo>())
        {
            item.SetOldParent();
        }
        foreach (var item in module.GetComponentsInParent<ModuleLobo>())
        {
            item.SetOldParent();
        }
    }

    public void SetOldParent()
    {
        oldParent = null;

        if (transform.parent != null)
        {
            oldParent = transform.parent.GetComponent<ModuleLobo>();
        }
    }


}
