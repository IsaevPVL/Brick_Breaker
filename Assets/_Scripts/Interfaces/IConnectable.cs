using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IConnectable
{
    void Connect(Stack<Vector3Int> i);
}