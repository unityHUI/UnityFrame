--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


LMsgCenter = {}
local this = LMsgCenter;
function LMsgCenter:new (o)

  o = o or {};
 
  setmetatable(o,self);

  self.__index = self;

  return o; 
 
end

function  LMsgCenter.GetInstance()
  if this.Instance == nil then
    this.Instance = this:new()
  end
      return this.Instance;
end

function  LMsgCenter:HandleMsg(msg)
        self.AnalysisMsg(msg);
end

function  LMsgCenter.AnalysisMsg(msg)
    managerId = msg:GetManagerID();
    print("center msgid = "..managerId);
   if managerId == LManagerID.LUIManager then 
        LUIManager.GetInstance().AnalysisMsg(msg);
  elseif managerId == LManagerID.LNpcManager then
        LNpcManager.GetInstance().AnalysisMsg(msg);
  elseif managerId == LManagerID.LAssetManager then
   
  elseif managerId == LManagerID.LNetManager then 

  elseif managerId == LManagerID.LAudioManager then
  
  else 

  end
end