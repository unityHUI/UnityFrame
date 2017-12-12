using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadEvent
{
    LoadOne = ManagerID.UIManager + 1,
    LoadTwo,
    MaxValue,
}
public enum Register
{
    RegisterOne = LoadEvent.MaxValue + 1,
    RegisterTwo,
    MaxValue,
}