--[[

EXAMPLE TABLE OF CONTENTS FILE
This file is only used to initialize the Mod's information.

To create a new Mod, follow these 4 steps:
    - Find a name for your mod (ex: MyMod)
    - Create a new folder in StreamingAssets/Mods with your mod's name (ex: StreamingAssets/Mods/MyMod)
    - In that folder, create a lua script with your mod's name (ex: StreamingAssets/Mods/MyMod/MyMod.lua)
    - Fill in the necessary fields in the "toc" global variable to give information about your mod (see toc definition just below)

toc is a table (more precisely a userdata) containing specific fields that you can overwrite:
toc = {
    title:string, -- the display name of your mod
    notes:string, -- a short note to display more information to the end user
    version:string,
    author:string,
    
    -- the path to the lua files containing the mod's logic (ex: filesToLoad = { "core", "effects", "Data/cards", ... })
    -- the game will load these files in the same order! if there is none, your mod won't do anything.
    filesToLoad:table,
}

]]

toc.title = "Base Game"

toc.notes = "The base game data and features"

toc.version = "0.1"

toc.author = "Groupe3"

toc.filesToLoad = {
    "exampleAPI",
    "exampleAPI2",
}
