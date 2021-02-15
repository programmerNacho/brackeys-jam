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

    public void ConnectNewModule(Module newModule, ModuleSide sideConnected)
    {
        if(connectedModules.Contains(newModule) == false && moduleSides.Contains(sideConnected))
        {
            connectedModules.Add(newModule);
            sideConnected.connected = true;
        }
    }

    public void SetParentModule(Module parentModule)
    {
        transform.SetParent(parentModule.transform);
    }

    public void ActivatePhysics()
    {
        rigidbody.isKinematic = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DeActivatePhysics()
    {
        rigidbody.isKinematic = true;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
    }
}
