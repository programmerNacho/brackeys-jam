using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : Block
{
    public override void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide)
    {
        Block otherModule = otherModuleSide.ModuleParent;

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

    public override void ModuleImpacted(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Muerto");
    }
}
