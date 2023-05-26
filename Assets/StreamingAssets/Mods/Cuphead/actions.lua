﻿local f = {}

local currentGameData

function OnGameAction(actionType, gameData, params) -- Game API
    currentGameData = gameData

    -- print(actionType, currentGameData.GetTurnNumber(), table.unpack(params))

    if actionType == "PhaseChanged" then
        f.processPhaseChanged(params[1])
    end
end

function f.processPhaseChanged(newPhase)
    if newPhase == "DRAW" then
        f.processDrawPhase()
    end
end

function f.processDrawPhase()
    local player = nil
    if currentGameData.GetTurn() == "PLAYER1" then
        player = currentGameData.GetPlayer()
    elseif currentGameData.GetTurn() == "PLAYER2" then
        player = currentGameData.GetOpponent()
    end

    for _, card in pairs(player.cardsOnBoard) do
        if card.Name == "Devil" then -- the devil damages the player each turn as long as he is on the terrain
            player.takeDamage(1)
            print("hahahahahaha")
        end
    end
end