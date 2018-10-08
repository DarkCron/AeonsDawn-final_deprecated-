local rand = require [[Content\LUA\utils]]
import ('Aeons Dawn', 'LUA') 

function start()
	battle = LuaScriptBattle()
	battle.enemies:Add(Enemy([[Goblin_Complete]],LuaPoint(10,17)))
	battle.enemies[0].rot = LuaHelp.Random(0,4)
	battle.enemies:Add(Enemy([[Goblin_Complete]],LuaPoint(10,17)))
	battle.enemies:Add(Enemy([[Goblin_Complete]],LuaPoint(10,17)))
	battle.enemies:Add(Enemy([[Goblin_Complete]],LuaPoint(10,17)))
	battle.partySpawn = LuaPoint(11,19)
	battle.regionID = 0
	battle.zoneID = 0
	
	GenerateCombatScript()
	
	LuaScriptBattle.Initialize(battle)
end

function GenerateCombatScript()
	ev1 = LuaBScriptEvent()
	ev1.eventType = LuaBScriptEvent.EventType.startET --function gets called when turns switch from group to group (player to enemy for example)
	ev1.functionName = [[enemySkip]]
	ev1.fileNameLoc = [[LUA\Intro\IntroBattle.lua]]
	
	ev2 = LuaBScriptEvent(ev1)
	ev2.eventType = LuaBScriptEvent.EventType.postCT --function gets called when a character ends it's turn
	ev2.functionName = [[call]]

	ev3 = LuaBScriptEvent(ev1)
	ev3.eventType = LuaBScriptEvent.EventType.updateEV --function gets called when a character ends it's turn
	ev3.functionName = [[goblindance]]
	
	LuaBScriptHandle.AddBScript(ev1)
	LuaBScriptHandle.AddBScript(ev2)
	LuaBScriptHandle.AddBScript(ev3)
end

function enemySkip(turnSet)
	LuaCombatCommands.EndTurn(turnSet)
end

--fix combat controls, enter to end combat
local testint = 1
function call(turnSet)
	print([[Hello! From: ]],turnSet.CTCallFrom.charInfo.name,[[ ]],testint)
	testint = testint + 1
	
	
	if turnSet.bIsPlayer then --prevents the enemy from triggering itself :)
		if  turnSet:CTIsFromCurrentTS(turnSet.CTCallFrom) then
			--dia = LuaDialogue([[FOCUS!]])
			--dia:Initialize([[Knight]],[[-]],0)--0 is Left, 1 is Right
			--LuaExecutionList.Add(dia);
			turnSet:HandleChangeSide(turnSet.otherGroups[0].charTurnInfos[0],turnSet.otherGroups[0],turnSet,LuaTurnSetInfo.SideTurnType.MindControl)
			--LuaCombatCommands.StartAI(turnSet.otherGroups[0].charTurnInfos[0])
		end
	end
end

local timer = 300
local timePassed = 0
function goblindance(turnSet,ms)
	timePassed = timePassed + ms
	
	if timePassed >= timer then
		timePassed = 0
		rotation = LuaHelp.Random(0,4)
		for i = 0, turnSet.otherGroups[0].charTurnInfos.Count-1 do
			turnSet.otherGroups[0].charTurnInfos[i].charInfo:Rotate(rotation)
		end
	end

end