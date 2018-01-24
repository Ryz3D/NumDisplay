local NumDisplay={}

function NumDisplay:Start()
	self.targets = {} --displays are gonna get saved in here
	self.targetNum = 0 --amount of displays in self.targets
	self.inSeatLast = false --HBU.InSeat() of last frame
	
	self.ctrl = HBU.GetKey("Control")
	self.alt = HBU.GetKey("Alt")
	
	self.path = "C:/Program Files (x86)/Steam/steamapps/common/Homebrew - Vehicle Sandbox/hb146_Data/NumDisplay.txt"
	
	self.resend = 0.1
	self.lastSend = 0.0
	
	self.display = 1
	
	string2file("", self.path, nil)
end

function NumDisplay:Update()
	--updates when entering/exiting a vehicle
	if (HBU.InSeat() ~= self.inSeatLast) then 
		self.inSeatLast = HBU.InSeat()
		if (HBU.InSeat()) then
			--search displays (PartSearch saves them in self.targets)
			
			local parentComponent = Camera.main.gameObject:GetComponentInParent("VehicleRoot")
			local parent = parentComponent.transform
			self.targetNum = 0
			self:PartSearch(parent, "^Number display")
		else
			self.targets = {} --clear
		end
	end
	
	if (HBU.InSeat()) then
		if (self.ctrl:GetKey() > 0.5 and self.alt:GetKeyDown()) then
			self.display = self.display + 1
			if (self.display > self.targetNum) then
				self.display = 1
			end
		end
		
		if (Time.time - self.lastSend >= self.resend) then
			self:Send()
			self.lastSend = Time.time
		end
	end
end

function NumDisplay:Send()
    local c = self.targets[self.display]
    local component = c:GetComponent("HBNumberDisplay")
    local text = component.aditionalText
    local value = round(component.numberInput.value * math.pow(10, component.decimalOffset), 1)
    local data = tostring(value)..text
    string2file(data, self.path, "wt", nil)
end

function NumDisplay:OnDestroy()
	string2file("", self.path, nil)
end

function round(num, decimals)
	--rounds num to the given decimal places
	return string.format("%." .. (decimals or 0) .. "f", num)
end

function NumDisplay:PartSearch(parent, part)
	--search with parts with "part" in the name
	--that are also children of "parent"
	
    for i = 0, parent.transform.childCount - 1 do 
        local t = parent.transform:GetChild(i)
        if (not Slua.IsNull(t)) then
            if (t.name:find(part) ~= nil) then --if it has "part" in the name
				--add target
				table.insert(self.targets, t)
				self.targetNum = self.targetNum + 1
            end
			
            if (t.childCount > 0) then
				--call on all objects with children (recursion)
                self:PartSearch(t, part)
            end
        end
    end
end

return NumDisplay