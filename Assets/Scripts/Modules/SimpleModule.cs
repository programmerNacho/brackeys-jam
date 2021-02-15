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
}
