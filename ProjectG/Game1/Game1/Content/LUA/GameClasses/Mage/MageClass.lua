import ('Aeons Dawn', 'LUA') 

function LevelUp(ClassInfo)
	level = ClassInfo.level
	
	extraHPplus = 1 + LuaHelp.Random(math.floor(level/3),2+level + math.floor(level/3))
	ClassInfo.statAddition:AddModStat("MaxHP",extraHPplus)
	
	chance = LuaHelp.Random(0,100)
	extraINT = LuaHelp.Random(0,extraHPplus)
	if chance < 70 then
		extraINT = extraINT + 1
		extraINT = extraINT + LuaHelp.Random(0,math.floor(level/3))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("INT",extraINT)
	
	chance = LuaHelp.Random(0,100)
	extraDEF = 0
	if chance < 40 then
		extraDEF = extraDEF + 1
		extraDEF = extraDEF + LuaHelp.Random(0,math.floor(level/4))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("Def",extraDEF)
	
	chance = LuaHelp.Random(0,100)
	extraMANA = LuaHelp.Random(0,4)
	if chance < 70 then
		extraMANA = extraMANA + 1
		extraMANA = extraMANA + LuaHelp.Random(0,math.floor(level/2))
		ClassInfo.quality = ClassInfo.quality+1;
	end
	ClassInfo.statAddition:AddModStat("MAXMP",extraMANA)
	
	
	if level % 5 == 0 then
		print(level % 5)
		print(level)
		ClassInfo.statAddition:AddModStat("Crit",2)
		ClassInfo.statAddition:AddModStat("Hit",2)
		ClassInfo.statAddition:AddModStat("INT",1)
		ClassInfo.statAddition:AddModStat("STR",1)
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