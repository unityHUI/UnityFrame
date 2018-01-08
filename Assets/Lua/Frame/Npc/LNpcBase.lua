--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

LNpcBase = {msgIds = {}};

function  LNpcBase:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

function  LNpcBase:RegisterSelfMsg(script,msgs)

    LNpcManager.GetInstance():RegisterMultMsg(script,msgs);
end

function LNpcBase:RemoveSelfMsg(script,msgs)
   LNpcManager.GetInstance():RemoveMultMsg(script,msgs);
end

function LNpcBase:AnalysisMsg (msg)
    LNpcManager.GetInstance():AnalysisMsg(msg);
end

function  LNpcBase:OnDestroy()
    LNpcBase:RemoveSelfMsg(self,self.msgIds);
end