# Yahtzee Pro Play API

## `/newgame` POST
_Creates a new Game that can be sent moves to play on._

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
_Lists the ids of all games stored._

#### Returns
_All guids of existing games._
```json
[
    "some-guid-of-a-game",
    "another-guid-of-a-game"
]
```

## `/games/{guid}` GET
_Return details of a game, including the current game state, the active player and last dice roll_
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

_Makes a given move on the game of the provided game id._

#### Input parameter
`MoveRequest`

_The move choice must be either "safe" or "risky"_

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
_Simulates games between two auto players. This can be one game, many games, or sets of games _

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
_List of simulation results for each set_

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