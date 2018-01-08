--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

LUIManager = LManagerBase:new();

local  this = LUIManager;
function  LUIManager:new(o)
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end
LUIManager.kk = "kkkk";

function  LUIManager.GetInstance()
  if this.Instance == nil then
      this.Instance = this:new()
  end
      return this.Instance;
end

function  LUIManager.AnalysisMsg(msg)

  if msg:GetManagerID() == LManagerID.LUIManager then
    print("hanle msg msgid = "..msg:GetManagerID())
    this:HandleMsgEvent(msg);
  else
     LMsgCenter.GetInstance().AnalysisMsg(msg);
  end 
 
end