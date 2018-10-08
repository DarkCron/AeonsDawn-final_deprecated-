import ('The betrayer', 'TBAGW.Utilities') 
import ('The betrayer', 'TBAGW') 
import ('System')

print("Hello world")
testObj = testClass("")


local localTable = { testClass("element1"),testClass("element2"),testClass("element3")}

function MyFunc(val1)
	val1 = val1 + testObj.i
	return val1
end

function publicStaticCall()
	DataProvider.stuff()
end

function internalStaticCall()
	DataProvider.internalStuff()
end

function attempt()
	print("This is from provisionState:")
	print(provisionState.s)
	provisionData:getData().sList:Add("")
	return provisionData:getData().name
end

function returnData()
	return provisionData:getData()
end

function returnTable()
	return localTable
end

local tableOfObjects = {}
local TOOIndex = 0;
local assignedTable = {}

function modifyObject(testObj)
	if(testObj.i >= 10) then
	testObj.s = "True element";
	else
	testObj.s = "Imposter!";
	end
	--tableOfObjects[#tableOfObjects +1] = testObj
end

function doStuff()
	for key,value in pairs(assignedTable) do
	modifyObject(value)	
	end

	return assignedTable;
end

function setStuff(item)
	assignedTable[#assignedTable+1] = item;
end

print("Done reading test.lua")