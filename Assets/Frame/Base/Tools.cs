﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerID:ushort
{

    GameManager = 0,

    UIManager = Tools.MsgSpan,

    NpcManager = Tools.MsgSpan * 2,

    AssetManager = Tools.MsgSpan * 3,

    AudioManager = Tools.MsgSpan * 4,

    NetManager = Tools.MsgSpan * 5,

}

public class Tools
{

    public const ushort MsgSpan = 1000;

}
