import ('Aeons Dawn', 'LUA')  

function modifier(abiInfo)



	
	abiInfo.minDmg = 0 + (abiInfo.caster:getStat("AGI") - abiInfo.target:getStat("DEF"))/3
	
	if abiInfo.minDmg < 2 then
		abiInfo.minDmg = 2
	end
	
	abiInfo.maxDmg = 1 + (abiInfo.caster:getStat("AGI")- abiInfo.target:getStat("DEF"))/2

	
end
