using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    [SerializeField]
    protected List<Module> connectedModules = new List<Module>();
    [SerializeField]
    protected List<ModuleSide> moduleSides = new List<ModuleSide>();
    [SerializeField]
    private float cooldownToCanDock = 2f;

    protected new Rigidbody2D rigidbody = null;

    private float timeToCanDock = 0f;

    public bool CanDock()
    {
        return timeToCanDock <= 0f;
    }

    public void BeginCanDockCooldown()
    {
        timeToCanDock = cooldownToCanDock;
        foreach (Module m in connectedModules)
        {
            if (m.transform.parent == transform)
            {
                m.BeginCanDockCooldown();
            }
        }
    }

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

    protected void DisconnectFromParentModule()
    {
        List<Module> disconnect = new List<Module>();

        foreach (Module m in connectedModules)
        {
            if(m.transform == transform.parent)
            {
                disconnect.Add(m);
            }
        }

        foreach (Module m in disconnect)
        {
            m.connectedModules.Remove(this);
            connectedModules.Remove(m);
        }
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
        }
    }

    public virtual void SetParentModule(Module parentModule)
    {
        if(parentModule == null)
        {
            transform.SetParent(null, true);
        }
        else
        {
            transform.SetParent(parentModule.transform, true);
        }
    }

    public abstract void ModuleImpacted(Vector2 globalImpactPoint, Vector2 globalImpactDirection);

    public void ActivatePhysics()
    {
        if(rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        rigidbody.useFullKinematicContacts = true;
        rigidbody.isKinematic = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 0f;
    }

    public void DeActivatePhysics()
    {
        Destroy(rigidbody);
    }

    private void Update()
    {
        if(timeToCanDock > 0f)
        {
            timeToCanDock = Mathf.Clamp(timeToCanDock - Time.deltaTime, 0f, float.MaxValue);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Meteorite")
        {
            CalculateAndExecuteImpact(collision);
            Destroy(collision.gameObject);
        }
    }

    protected void CalculateAndExecuteImpact(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.GetContact(0);

        Vector2 contactPosition = contactPoint.point;
        Vector2 impulseDirection = contactPoint.rigidbody.velocity.normalized;

        ModuleImpacted(contactPosition, impulseDirection);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
