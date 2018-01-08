--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetManager = LManagerBase:new();

local  this = LAssetManager;
function  LAssetManager:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

function  LAssetManager.GetInstance()
  if this.Instance == nil then
      this.Instance = this:new()
  end
      return this.Instance;
end

function  LAssetManager.AnalysisMsg(msg)
  if msg:GetManagerID() == LManagerID.LAssetManager then
    this:HandleMsgEvent(msg);
  else
     LMsgCenter.GetInstance().AnalysisMsg(msg);
  end 
 
end