--region *.lua
--Date
--此文件由[BabeLua]插件自动生成
 



--endregion

-- 消息的基类 

LMsgBase =  {msgId = 0};

LMsgBase.__index = LMsgBase;


function LMsgBase:new(msgId)
  
  local  self = {};

  setmetatable(self,LMsgBase);

  self.msgId = msgId;

  return self;
  
end

function  LMsgBase:GetManagerID()
 
   msgId =  math.floor(self.msgId / MsgSpan) * MsgSpan;
  
  return math.ceil(msgId);
 
end