--[[

EXAMPLE LUA FILE (Mod logic)

For all lua files of your mod, the game exposes some global API for you to overwrite/modify/create content.

Also, you might come to realize that you cannot access the lua "require" function,
so how do you access your code in other files? that's easy!
The game gives you two global variables: _G["ModName"]:string and _G["ModTable"]:table (literally)
and those are shared between all of your lua files. So if you want to access something from another file,
make it accessible from the ModTable global variable.

Game API:

-- everything game-related must be accessible from the global environment.
_G = {
    ModName:string,
    ModTable:table,
    
    GetModInstanceInfo(key:string):any, -- access your TOC information
    
    GetCurrentRound():number, -- ex: print(GetCurrentRound())
    GetPlayer():userdata
    GetOpponent():userdata
    
    -- the next elements are for you to define, if you want to implement custom logic
    OnModEnabled(),
    OnModDisabled(),
    
    OnStartRound(),
    OnEndRound(),
    ...
}

]]

function OnModEnabled()
    print("Hey from OnModEnabled")
    print(table.unpack(ModTable.test2))
    ModTable.test3 = (ModTable.test3 or 0) + 3
end

function OnModDisabled()
    print("Hey from OnModDisabled")
    print(ModTable.test3)
end

print(ModName)
ModTable.test = (ModTable.test or 0) + 1

HelloTest = 2