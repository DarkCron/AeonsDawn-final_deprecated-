import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)

	print("You cast: ",abiInfo.abiName)
	print("INT: ",abiInfo.caster:getStat("INT"))
	print("Damage was between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
	abiInfo.minDmg = abiInfo.caster:getStat("INT") - abiInfo.target:getStat("INT")
	
	if abiInfo.minDmg < 3 then
		abiInfo.minDmg = 3
	end
	
	abiInfo.maxDmg = 3 + abiInfo.minDmg
	--abiInfo.maxDmg = abiInfo.maxDmg + 2
	print("Damage is now between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
end