--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

-- 脚本节点类 存储当前脚本 以及 下一个脚本
LEventNode = { }

function LEventNode:new(event)
	local o = { };

	setmetatable(o, self);

	self.__index = self;

	o.value = event;

	o.next = nil;

	return o;

end

