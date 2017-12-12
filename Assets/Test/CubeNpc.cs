using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCEvent:ushort {
    Left = ManagerID.NpcManager + 1,
    Right,
    Forward,
    Back,
    MaxVlaue,
}
public class CubeNpc : NPCBase {

	// Use this for initialization
	void Start () {
        msgIds = new ushort[] {
          (ushort)NPCEvent.Left,
          (ushort)NPCEvent.Right,
          (ushort)NPCEvent.Forward,
          (ushort)NPCEvent.Back,           
        };
        RegisterSelfMsg(this, msgIds);
	}
    public override void HandleMsgEvent(MsgBase msg)
    {
        base.HandleMsgEvent(msg);
        switch (msg.MsgID) {
            case (ushort)NPCEvent.Left:
                transform.position = transform.position + Vector3.left;
                break;
            case (ushort)NPCEvent.Right:
                transform.position = transform.position + Vector3.right;
                break;
            case (ushort)NPCEvent.Forward:
                transform.position = transform.position + Vector3.forward;
                break;
            case (ushort)NPCEvent.Back:
                transform.position = transform.position + Vector3.back;
                break;
            default:
                break;
        }
    }

}
