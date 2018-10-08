import ('Aeons Dawn', 'LUA') 


function getLevel(level)
	base = 220
	EXP = 0
	
	
	if level <= 10 then
		EXP = base * 1.5 * level
	end
	
	if level > 10 and level < 21 then
		EXP = base * 1.7 * level
	end
	
	if level >= 21  then
		EXP = base * 2.0 * level
	end
	
	return EXP
end