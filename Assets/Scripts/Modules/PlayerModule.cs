using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : Module
{
    public override void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide)
    {
        Module otherModule = otherModuleSide.ModuleParent;

        if (otherModule is SimpleModule otherSimpleModule)
        {
            switch (otherSimpleModule.currentState)
            {
                case SimpleModule.State.Free:
                case SimpleModule.State.WithSimple:
                    DockOtherModule(myModuleSide, otherModuleSide, otherSimpleModule);
                    otherSimpleModule.SetState(SimpleModule.State.WithPlayer);
                    break;

                case SimpleModule.State.WithEnemy:
                    // Me muero.
                    break;
            }
        }
        else if (otherModule is EnemyModule otherEnemyModule)
        {
            // Hemos chocado nexos el enemigo y yo. Muero.
        }
    }
}
