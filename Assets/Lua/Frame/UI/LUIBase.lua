--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

LUIBase = {msgIds = {}};

function  LUIBase:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

function  LUIBase:RegisterSelfMsg(script,msgs)

    LUIManager.GetInstance():RegisterMultMsg(script,msgs);
end

function LUIBase:RemoveSelfMsg(script,msgs)
   LUIManager.GetInstance():RemoveMultMsg(script,msgs);
end

--点语言默认不接受self
function LUIBase.AnalysisMsg(msg)
    LUIManager.GetInstance().AnalysisMsg(msg);
end

function  LUIBase:OnDestroy()
    LUIBase:RemoveSelfMsg(self,self.msgIds);
end