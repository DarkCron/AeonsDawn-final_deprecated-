import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)



	abiInfo.minDmg = 0
	abiInfo.maxDmg = 0

	baseHitChance = 80
	baseHitChance = baseHitChance + (abiInfo.caster:getStat("INT")-abiInfo.target:getStat("INT"))*5
	abiInfo.hitChance = baseHitChance
	
end

function abiMod(abiInfo,stuff2)
	abiModColl = LuaAbilityCastModCollection(LuaMindControl(abiInfo.target))
	return abiModColl
end