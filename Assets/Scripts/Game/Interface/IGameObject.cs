using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameObjectType
{
    None,
    Character,
    Vehicle
}

public interface IGameObject
{
    GameObjectType gameObjectType { get; set; }
    float speed { get; set; }
}
