--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetMsg =  LMsgBase:new ();


function  LAssetMsg:new (msgId,scenceName,bundleName,resName,isSingle,luaFunc)
    o = o or {};
    setmetatable(o,self);
    self.__index = self;
    o.msgId  = msgId;
    o.scenceName = scenceName;
    o.bundleName = bundleName;
    o.resName = resName;
    o.isSingle = isSingle;
    o.luaFunc = luaFunc
    return o

end