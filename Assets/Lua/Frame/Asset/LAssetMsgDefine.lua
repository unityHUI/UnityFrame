--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



-- endregion

local MsgStart = LManagerID.LAssetManager;

LAssetMsgDefine = {

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
}