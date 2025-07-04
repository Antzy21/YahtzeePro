# Yahtzee Pro Play API

## `/newgame` POST
Creates a new Game that can be sent moves to play on.

#### Input parameter
`GameConfiguration`

```json
{
    "winningValue": 5000,
    "totalDice": 5
}
```

#### Returns
Guid of the newly created game.

## `/games` GET
Lists the ids of all games stored.

#### Returns
All guids of existing games.
```json
[
    "some-guid-of-a-game",
    "another-guid-of-a-game"
]
```

## `/games/{guid}` GET
Return details of a game, including the current game state, the active player and last dice roll
#### Input parameter:
`Guid` of the game to be retrieved.
#### Returns
`GameResponse`

```json
{
    "gameState": {
        "playerScore": 0,
        "opponentScore": 0,
        "cachedScore": 0,
        "diceToRoll": 5,
        "isStartOfTurn": true,
        "gameConfiguration": {
            "winningValue": 5000,
            "totalDice": 5
        }
    },
    "ActivePlayerName": "You",
    "LastDiceRoll": null
}
```

## `/move` POST

Makes a given move on the game of the provided game id.

#### Input parameter
`MoveRequest`

The move choice must be either "safe" or "risky"

```json
{
    "gameId": "some-game-guid",
    "moveChoice": "risky",
}
```

#### Returns
`GameResponse`

```json
{
    "gameState": {
        "playerScore": 1200,
        "opponentScore": 850,
        "cachedScore": 200,
        "diceToRoll": 2,
        "isStartOfTurn": false,
        "gameConfiguration": {
            "winningValue": 5000,
            "totalDice": 5
        }
    },
    "ActivePlayerName": "You",
    "LastDiceRoll": {
        "diceCount": [
            {"1": 1},
            {"2": 1},
            {"3": 0},
            {"4": 1},
            {"5": 2},
            {"6": 0},
        ],
        "score": 200,
        "numberOfDiceScoring": 3,
        "allDiceAreScoring": false
    }
}
```    

## `/simulate` POST
Simulates games between two auto players. This can be one game, many games, or sets of games

#### Input parameter
`SimulateGamesRequest`
```json
{
    "player1Name": "strategy1",
    "player2Name": "strategy2",
    "totalGames": 1,
    "totalSets": 1,
    "gameConfiguration": {
        "winningValue": 5000,
        "totalDice": 5
    }
}
```
#### Returns:
List of simulation results for each set

`GameSetResult`
```json
{
    [
        {
            "winnerScore": 5000,
            "loserScore": 3950,
            "winningPlayer": "strategy1"
        }
    ],
    "playerOneWinCount": 1,
    "playerTwoWinCount": 0
}
```