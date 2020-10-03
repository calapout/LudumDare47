using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bytes;

public class EntityData : Bytes.Data
{
    public Entity entity;

    public EntityData(Entity entity)
    {
        this.entity = entity;
    }
}
