using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleModule : Module
{
    public enum State { Free, WithPlayer, WithEnemy }

    public State currentState = State.Free;

    public override void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide)
    {
        Module otherModule = otherModuleSide.ModuleParent;

        if(otherModule is SimpleModule otherSimpleModule)
        {

        }
    }
}
