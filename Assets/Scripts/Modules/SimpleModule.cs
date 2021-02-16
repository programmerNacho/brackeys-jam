using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleModule : Module
{
    public enum State { Free, WithSimple, WithPlayer, WithEnemy }

    public State currentState = State.Free;

    public override void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide)
    {
        Module otherModule = otherModuleSide.ModuleParent;

        if (otherModule is SimpleModule otherSimpleModule)
        {
            switch (otherSimpleModule.currentState)
            {
                case State.Free:
                case State.WithSimple:
                    if (connectedModules.Contains(otherSimpleModule) == false)
                    {
                        DockOtherModule(myModuleSide, otherModuleSide, otherModule);
                        switch (currentState)
                        {
                            case State.Free:
                            case State.WithSimple:
                                SetState(State.WithSimple);
                                break;
                            case State.WithPlayer:
                                SetState(State.WithPlayer);
                                break;
                            case State.WithEnemy:
                                SetState(State.WithEnemy);
                                break;
                        }
                    }
                    break;
            }
        }
        else if(otherModule is PlayerModule otherPlayerModule)
        {
            if(currentState == State.WithEnemy)
            {
                // Me desacoplo.
            }
        }
        else if(otherModule is EnemyModule otherEnemyModule)
        {
            if(currentState == State.WithPlayer)
            {
                // Me desacoplo.
            }
        }
    }

    public void SetState(State state)
    {
        currentState = state;
        gameObject.layer = LayerMask.NameToLayer(currentState.ToString());

        foreach (Module m in connectedModules)
        {
            if(m is SimpleModule s && m.transform.parent == transform)
            {
                s.SetState(currentState);
            }
        }
    }

    public override void SetParentModule(Module parentModule)
    {
        if(transform.parent == null)
        {
            base.SetParentModule(parentModule);
        }
        else
        {
            Module currentParentModule = transform.parent.GetComponent<Module>();
            base.SetParentModule(parentModule);
            currentParentModule.DeActivatePhysics();
            currentParentModule.SetParentModule(this);
        }
    }

    public override void ModuleImpacted(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
    {
        BeginCanDockCooldown();
        DisconnectFromParentModule();
        base.ActivatePhysics();
        transform.parent = null;

        if (connectedModules.Count > 0)
        {
            SetState(State.WithSimple);
        }
        else
        {
            SetState(State.Free);
        }


        rigidbody.AddForce(globalImpactDirection * 5f, ForceMode2D.Impulse);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Module otherModule = collision.gameObject.GetComponent<Module>();

        if(otherModule)
        {
            if(otherModule is SimpleModule simpleModule)
            {
                bool playerAgainstEnemy = currentState == State.WithPlayer && simpleModule.currentState == State.WithEnemy;
                bool enemyAgainstPlayer = currentState == State.WithEnemy && simpleModule.currentState == State.WithPlayer;

                if (playerAgainstEnemy || enemyAgainstPlayer)
                {
                    CalculateAndExecuteImpact(collision);
                }
            }
        }
        
    }
}
