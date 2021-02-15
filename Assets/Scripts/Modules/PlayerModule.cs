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
                    DockOtherModule(myModuleSide, otherModuleSide, otherSimpleModule);
                    otherSimpleModule.currentState = SimpleModule.State.WithPlayer;
                    break;
                case SimpleModule.State.WithPlayer:
                    break;
                case SimpleModule.State.WithEnemy:
                    break;
            }
        }
        else if (otherModule is EnemyModule otherEnemyModule)
        {

        }
    }

    private void DockOtherModule(ModuleSide myModuleSide, ModuleSide otherModuleSide, Module otherModule)
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
}
