import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)



	
	abiInfo.minDmg = 0 + (abiInfo.caster:getStat("AGI") - abiInfo.target:getStat("DEF"))/4
	
	if abiInfo.minDmg < 2 then
		abiInfo.minDmg = 1
	end
	
	abiInfo.maxDmg = 1 + (abiInfo.caster:getStat("AGI")- abiInfo.target:getStat("DEF"))/3

	
end

function abiMod(abiInfo,stuff2)
	print("Ola señor")
	abiModColl = LuaAbilityCastModCollection(LuaAbilityCastMod(abiInfo.target))
	abiModColl.abiModList[0].statMod:AddModStat("HP",-abiInfo.minDmg)
	return abiModColl	
end
