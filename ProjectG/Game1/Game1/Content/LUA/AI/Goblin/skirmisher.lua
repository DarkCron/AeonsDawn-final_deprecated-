local rand = require [[Content\LUA\utils]]
import ('Aeons Dawn', 'LUA') 

function mainCreation(c)
	--rand.setRandomSeed(os.time()/3.1415);
	level = LuaCombatInfo.partyAverageLevel;

	stats = LuaStatEdit()
	stats:AddModStat("HP",LuaHelp.Random(-10,-5))
	stats:AddModStat("MAXHP",LuaHelp.Random(0,4)*level)
	stats:AddModStat("MP",LuaHelp.Random(0,5))
	stats:AddModStat("STR",LuaHelp.Random(0,2))
	stats:AddModStat("str",LuaHelp.Random(0,level)) -- Equal to STR
	stats:AddModStat("strength",1*LuaHelp.Random(0,level)) -- Also equal to STR
	--stats:AddModStat("HP_Regen",1)
	stats:AddModStat("HiT_ChAnCe",-5)
	stats:AddModStat("DEF",LuaHelp.Random(0,2))
	stats:AddModStat("STR",LuaHelp.Random(0,2))
	
	print([[Average level; ]],level)
	c.dialogueName = "Modified Goblin"
	c:LuaUpdateStats(stats)
end