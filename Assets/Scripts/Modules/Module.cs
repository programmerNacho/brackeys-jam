using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    [SerializeField]
    protected List<Module> connectedModules = new List<Module>();
    [SerializeField]
    protected List<ModuleSide> moduleSides = new List<ModuleSide>();

    private new Rigidbody2D rigidbody = null;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public abstract void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide);

    protected void DockOtherModule(ModuleSide myModuleSide, ModuleSide otherModuleSide, Module otherModule)
    {
        ConnectModulesAndModuleSides(myModuleSide, otherModuleSide, otherModule);
        SetOtherModuleParentAndPhysicsBehaviour(otherModule);
        RotateOtherModule(myModuleSide, otherModuleSide, otherModule);
        MoveOtherModule(myModuleSide, otherModuleSide, otherModule);
    }

    private void RotateOtherModule(ModuleSide myModuleSide, ModuleSide otherModuleSide, Module otherModule)
    {
        Vector2 inverseMyModuleSideNormal = -myModuleSide.NormalDirectionGlobal;
        Vector2 otherModuleSideNormal = otherModuleSide.NormalDirectionGlobal;
        float angleRotationZ = Vector2.SignedAngle(otherModuleSideNormal, inverseMyModuleSideNormal);
        otherModule.transform.Rotate(Vector3.forward, angleRotationZ);
    }

    private void MoveOtherModule(ModuleSide myModuleSide, ModuleSide otherModuleSide, Module otherModule)
    {
        Vector2 myDockPoint = myModuleSide.MiddlePointGlobal;
        Vector2 otherDockPoint = otherModuleSide.MiddlePointGlobal;
        Vector2 translationOtherModule = myDockPoint - otherDockPoint;
        otherModule.transform.position = (Vector2)otherModule.transform.position + translationOtherModule;
    }

    private void ConnectModulesAndModuleSides(ModuleSide myModuleSide, ModuleSide otherModuleSide, Module otherModule)
    {
        ConnectNewModule(otherModule, myModuleSide);
        otherModule.ConnectNewModule(this, otherModuleSide);
    }

    private void SetOtherModuleParentAndPhysicsBehaviour(Module otherModule)
    {
        otherModule.SetParentModule(this);
        otherModule.DeActivatePhysics();
    }

    public void ConnectNewModule(Module newModule, ModuleSide sideConnected)
    {
        if(connectedModules.Contains(newModule) == false && moduleSides.Contains(sideConnected))
        {
            connectedModules.Add(newModule);
            sideConnected.connected = true;
        }
    }

    public virtual void SetParentModule(Module parentModule)
    {
        transform.SetParent(parentModule.transform, true);
    }

    public void ActivatePhysics()
    {
        if(rigidbody)
        {
            Destroy(rigidbody);
        }

        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 0f;
    }

    public void DeActivatePhysics()
    {
        Destroy(rigidbody);
    }
}
