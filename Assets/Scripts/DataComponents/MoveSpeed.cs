using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Shooter.ECS
{
    [Serializable]
    public class MoveSpeed : IComponentData
    {
        public float Value;
    }

    public class MoveSpeedComponent : ComponentDataWrapper<MoveSpeed>
    {

    }

}
