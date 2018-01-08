-- region *.lua
-- Date
-- 此文件由[BabeLua]插件自动生成



-- endregion

-- eventTree 链式存储  以消息ID为单个链式结构 对该消息感兴趣的脚本存储在LEventNode里，并作为一个节点进行挂载

LManagerBase = { eventTree = { } };

function LManagerBase:new(o)
    o = o or {};
    setmetatable(o, self);
    self.__index = self;
    return o;
end

function LManagerBase:FindKey(dict, key)
    for k, v in pairs(dict) do
        if k == key then
            return true
        end
    end
    return false;
end

-- 注册一条消息   节点内容 为 脚本
function LManagerBase:RegisterMsg(id, eventNode)
    if not self:FindKey(self.eventTree, id) then
        self.eventTree[id] = eventNode;
                print(self.kk);
          print("manager Base Register"..id .. " " ..eventNode.value.tag);
    else
        tmpNode = self.eventTree[id];
        while tmpNode.next ~= nil do
            tmpNode = tmpNode.next;
        end
        tmpNode.next = eventNode;
    end
end

-- 注册一个脚本所关注的所有消息
function LManagerBase:RegisterMultMsg(script, msgs)
    
    for i, v in pairs(msgs) do
        node = LEventNode:new(script);

        self:RegisterMsg(v, node);
    end
end

function LManagerBase:RemoveMsg(script, id)
    if self:FindKey(self.eventTree, id) then
      local tmpNode = self.eventTree[id];
        if tmpNode.value == script then
            if tmpNode.next ~= nil then
                self.eventTree[id] = tmpNode.next;
                tmpNode.next = nil;              
            else
                self.eventTree[id] = nil;
            end
        else
          while tmpNode.next ~= nil and tmpNode.next.value ~= script do 
               tmpNode = tmpNode.next;
          end  
          if tmpNode.next.next == nil then
               tmpNode.next = nil
          else 
            local curNode = tmpNode.next;
               tmpNode.next = tmpNode.next.next;
               curNode.next = nil;
          end
        end
    end
end

function  LManagerBase:RemoveMultMsg(script,...)
  if arg == nil then
    return;
   end
  for i in arg do
   self:RemoveMsg(script,i);
  end
end

function  LManagerBase:HandleMsgEvent(msg)
    if not self:FindKey(self.eventTree,msg.msgId) then
      print("Dont Have MsgEvent  ManagerID ="..msg.msgId);
    else
      self:HandOutMsg(msg);

    end

end

function  LManagerBase:HandOutMsg(msg)
    print("handle msgid = "..msg.msgId);
--    print(self.kk);
--    for k,v in pairs(self.eventTree) do
--     print("k = "..k .. " v = "..v.value.tag);
--    end
     local msgId = msg.msgId;
     local tmpNode = self.eventTree[msgId];

    if self.eventTree[msgId] == nil then 
    print("tmpNode is null");
      return
    end

    repeat
       tmpNode.value:ProcessEvent(msg);
       tmpNode = tmpNode.next;
    until (tmpNode == nil);
     
end


function LManagerBase:OnDestroy()
    keys = { }
    keyCount = 0;
    for k, v in pairs(self.eventTree) do
        keys[keyCount] = k;
        keyCount = keyCount + 1;
    end
    for i = 1, keyCount do
        self.eventTree[keys[i]] = nil;
    end
end
 
