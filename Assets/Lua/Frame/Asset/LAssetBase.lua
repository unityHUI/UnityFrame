--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetBase = {msgIds = {}};

function  LAssetBase:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

function  LAssetBase:RegisterSelfMsg(script,msgs)

    LAssetManager.GetInstance():RegisterMultMsg(script,msgs);
end

function LAssetBase:RemoveSelfMsg(script,msgs)
   LAssetManager.GetInstance():RemoveMultMsg(script,msgs);
end

--点语法默认不接受self
function LAssetBase.AnalysisMsg(msg)
    LAssetManager.GetInstance().AnalysisMsg(msg);
end

function  LAssetBase:OnDestroy()
    LAssetBase:RemoveSelfMsg(self,self.msgIds);
end