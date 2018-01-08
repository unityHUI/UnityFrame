--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetLoad = LAssetBase:new();

function  LAssetLoad:new()
   o = o or {}
   setmetatable(o,self)
  self.__index = self;
  return o
end

--[[
    ReleseAsset = MsgStart + 1,
    ReleseBundleAsset = MsgStart + 2,
    ReleseScenceAllBundleAsset = MsgStart + 3,
    ReleseAppBundleAsset = MsgStart + 4,

    ReleseBundle = MsgStart + 5,
    ReleseBundleAndAsset = MsgStart + 6,
    ReleseScenceAllBundle = MsgStart + 7,
    ReleseAppBundle = MsgStart + 8,

    LoadAsset = MsgStart + 9,
    MaxValue = MsgStart + 10,
]]--
function  LAssetLoad : ProcessEvent(msg)
   if msg.msgId == LAssetMsgDefine.LoadAsset then
     LuaAssetManager.Instance:LoadAsset(msg.scenceName,msg.bundleName,msg.resName,msg.isSingle,msg.luaFunc);
   elseif msg.msgId == LAssetMsgDefine.ReleseAsset then
      LuaAssetManager.Instance:LoadAsset(msg.scenceName,msg.bundleName,msg.resName,msg.isSingle,msg.luaFunc);
    end
end