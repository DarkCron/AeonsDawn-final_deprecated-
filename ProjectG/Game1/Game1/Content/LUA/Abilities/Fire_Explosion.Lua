import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)

	if abiInfo.bCalledForArea then
		AreaHandler(abiInfo)
	end


	print("You cast: ",abiInfo.abiName)
	print("INT: ",abiInfo.caster:getStat("STR"))
	print("Damage was between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
	abiInfo.minDmg = 2 + abiInfo.caster:getStat("INT")/3
	abiInfo.maxDmg = 2 + abiInfo.caster:getStat("INT")/2
	--abiInfo.maxDmg = abiInfo.maxDmg + 2
	print("Damage is now between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
end

function AreaHandler(abiInfo)
	
	areaMod = LuaAbilityModArea()
	areaMod:ModifyArea([[ * ]])
	areaMod:ModifyArea([[***]])
	areaMod:ModifyArea([[ * ]])
	areaMod:ModifyArea([[   ]])
	areaMod:ModifyArea([[   ]])
	areaMod:ModifyArea([[ o ]])
	areaMod.bCenterIncluded = false
	abiInfo:ProcessAreaMod(areaMod)
	
	
end