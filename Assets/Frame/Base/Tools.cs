using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerID:ushort
{

    GameManager = 0,

    UIManager = Tools.MsgSpan,

    NpcManager = Tools.MsgSpan * 2,

}

public class Tools
{

    public const ushort MsgSpan = 1000;

}
