import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)

	if abiInfo.bCalledForArea then
		AreaHandler(abiInfo)
	end


	print("You cast: ",abiInfo.abiName)
	print("STR: ",abiInfo.caster:getStat("STR"))
	print("Damage was between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
	abiInfo.minDmg =  abiInfo.caster:getStat("STR") - (abiInfo.target:getStat("Def") +2)
	if abiInfo.minDmg < 0 then
		abiInfo.minDmg = 0
	end
	abiInfo.minDmg = abiInfo.minDmg + 2
	
	--abiInfo.maxDmg = abiInfo.maxDmg + 2
	--print("Damage is now between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
end

function AreaHandler(abiInfo)
	
	areaMod = LuaAbilityModArea()
	areaMod:ModifyArea([[***]])
	areaMod:ModifyArea([[ * ]])
	areaMod:ModifyArea([[ o ]])
	areaMod.bCenterIncluded = false
	abiInfo:ProcessAreaMod(areaMod)
	
	
end

function abiMod(abiInfo,stuff2)
	print("Ola señor")
	abiModColl = LuaAbilityCastModCollection(LuaAbilityCastMod(abiInfo.target))
	abiModColl.abiModList[0].statMod:AddModStat("DEF",-2)
	abiModColl.abiModList[0].statMod:AddModStat("STR",-2)
	return abiModColl	
end