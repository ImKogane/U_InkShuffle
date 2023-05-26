local f = {}

function OnGameAction(actionType, params) -- Game API
    -- print(actionType, GetGame().GetTurnNumber(), table.unpack(params))

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

    local currentGame = GetGame()
    if currentGame.GetTurn() == "PLAYER1" then
        player = currentGame.GetPlayer()
    elseif currentGame.GetTurn() == "PLAYER2" then
        player = currentGame.GetOpponent()
    end

    for _, card in pairs(player.cardsOnBoard) do
        if card.Name == "Devil" then -- the devil damages the player each turn as long as he is on the terrain
            player.TakeDamage(2)
            print("hahahahahaha")
        end
    end
end
