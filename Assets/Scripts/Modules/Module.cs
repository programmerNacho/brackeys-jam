using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    public abstract void InteractionBetweenModulesSides(ModuleSide myModuleSide, ModuleSide otherModuleSide);
}
