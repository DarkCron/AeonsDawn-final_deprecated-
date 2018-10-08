import ('Aeons Dawn', 'LUA') 

function LevelUp(ClassInfo)
	level = ClassInfo.level
	
	extraHPplus = 2 + LuaHelp.Random(math.floor(level/3),2+level)
	ClassInfo.statAddition:AddModStat("MaxHP",extraHPplus)
	
	chance = LuaHelp.Random(0,100)
	extraSTR = LuaHelp.Random(0,extraHPplus)
	if chance < 50 then
		extraSTR = extraSTR + 1
		extraSTR = extraSTR + LuaHelp.Random(0,math.floor(level/3))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("STR",extraSTR)
	
	chance = LuaHelp.Random(0,100)
	extraDEF = LuaHelp.Random(0,2)
	if chance < 25 then
		extraDEF = extraDEF + 1
		extraDEF = extraDEF + LuaHelp.Random(0,math.floor(level/6))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("Def",extraDEF)
	
	chance = LuaHelp.Random(0,100)
	extraMANA = LuaHelp.Random(0,2)
	if chance < 25 then
		extraMANA = extraMANA + 1
		extraMANA = extraMANA + LuaHelp.Random(0,math.floor(level/4))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("MAXMP",extraMANA)
	
	
	if level % 5 == 0 then
		print(level % 5)
		print(level)
		ClassInfo.statAddition:AddModStat("Crit",1)
		ClassInfo.statAddition:AddModStat("Hit",1)
		ClassInfo.statAddition:AddModStat("AGI",2)
		ClassInfo.quality = ClassInfo.quality+1;
	end
	
	if level % 10 == 0 then
		ClassInfo.statAddition:AddModStat("Mas",1)
		ClassInfo.quality = ClassInfo.quality+1;
	end
	
	extraPoints = 1
	extraPoints = extraPoints + math.floor(level/5)
	
	ClassInfo.classPoints = LuaClassPoints(ClassInfo,extraPoints)
end