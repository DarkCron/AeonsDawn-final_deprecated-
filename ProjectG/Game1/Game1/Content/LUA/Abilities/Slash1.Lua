import ('Aeons Dawn', 'LUA') 

function modifier(abiInfo)

	print("You cast: ",abiInfo.abiName)
	print("STR: ",abiInfo.caster:getStat("STR"))
	print("Damage was between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
	abiInfo.minDmg =  abiInfo.caster:getStat("STR")/2 -abiInfo.target:getStat("Def") 
	if abiInfo.minDmg < 0 then
		abiInfo.minDmg = 0
	end
	abiInfo.minDmg = abiInfo.minDmg + 2
	
	--abiInfo.maxDmg = abiInfo.maxDmg + 2
	--print("Damage is now between", abiInfo.minDmg, [[-]],abiInfo.maxDmg)
	
end