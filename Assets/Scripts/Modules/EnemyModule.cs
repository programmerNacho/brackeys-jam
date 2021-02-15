using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModule : Module
{
    public override void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide)
    {
        Module otherModule = otherModuleSide.ModuleParent;

        if (otherModule is SimpleModule otherSimpleModule)
        {

        }
        else if (otherModule is PlayerModule otherPlayerModule)
        {

        }
    }
}
