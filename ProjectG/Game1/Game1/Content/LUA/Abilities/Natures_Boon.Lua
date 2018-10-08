import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)

	print("You cast: ",abiInfo.abiName)
	print("INT: ",abiInfo.caster:getStat("INT"))
	print("Damage was between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
	abiInfo.minDmg = 0
	abiInfo.maxDmg = 0
	abiInfo.hitChance = 100
	--abiInfo.maxDmg = abiInfo.maxDmg + 2
	print("Damage is now between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
end

function abiMod()
	print("test from abiMod")
end

function abiMod(abiInfo,stuff2)
	--print("Ola señor")
	temp = LuaAbilityCastMod(abiInfo.target)
	abiModColl = LuaAbilityCastModCollection(temp)

	abiModColl.abiModList[0].length = 3
	amount = abiInfo.caster:getStat("INT")/3
	if amount < 2 then
		amount = 2
	end
	
	abiModColl.abiModList[0].statMod:AddModStat("HP",amount)
	return abiModColl	
end