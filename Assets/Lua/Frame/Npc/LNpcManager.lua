--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LNpcManager = LManagerBase:new();

local  this = LNpcManager;
function  LNpcManager:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

function  LNpcManager.GetInstance()
  if this.Instance == nil then
      this.Instance = this:new()
  end
      return this.Instance;
end

function  LNpcManager.AnalysisMsg(msg)
  if msg:GetManagerID() == LManagerID.LNpcManager then
    this:HandleMsgEvent(msg);
  else
     LMsgCenter.GetInstance().AnalysisMsg(msg);
  end 
 
end