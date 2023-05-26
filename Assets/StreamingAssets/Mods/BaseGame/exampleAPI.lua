--[[

EXAMPLE LUA FILE (Mod logic)

For all lua files of your mod, the game exposes some global API for you to overwrite/modify/create content.

Also, you might come to realize that you cannot access the lua "require" function,
so how do you access your code in other files? that's easy!
The game gives you two global variables: _G["ModName"]:string and _G["ModTable"]:table (literally)
and *those are shared between all of your lua files*. So if you want to access something from another file,
make it accessible from the ModTable global variable.

NOTE: Cards have a size of 420px * 600px (width/height)

Game API:

-- everything game-related must be accessible from the global environment.
_G = {
    ModName:string,
    ModTable:table,

    GetTocInfo(key:string):string, -- access your TOC information (except filesToLoad)

    GetGame():userdata, -- access the full game systems and API for the current game, usable in OnGameAction

    -- the next elements are for you to define, if you want to implement custom logic

    RegisterCards():table,

    OnModEnabled(),
    OnModDisabled(),

    OnGameAction(actionType:string, params:table) -- called each time an action happens in the game, params depends on the actionType
}

actionType:
    - "PhaseChanged" params[1] = ( "DRAW", "FREE", "ATTACK" )

GetGame() API:
    - GetPlayer():player
    - GetOpponent():player
    - GetPhase():string ( "DRAW", "FREE", "ATTACK" )
    - GetTurn():string ( "PLAYER1", "PLAYER2" )
    - GetCurrentRound():number

player API:
    - TakeDamage(amount:number)
    - cardsOnBoard:table (of cards, see below)

card API:
    - Name:string
    - PV:number
    - Attack:number
    - ApplyDamage(count:number)

]]

function OnModEnabled()
    print("Hey from OnModEnabled")
    print(GetTocInfo("notes"))
end

function OnModDisabled()
    print("Hey from OnModDisabled")
end

ModTable.test = "This variable is accessible from all of this mod's scripts!"

print(ModName) -- prints "BaseGame"

-- see the "cards.lua" file to see how new cards are implemented
