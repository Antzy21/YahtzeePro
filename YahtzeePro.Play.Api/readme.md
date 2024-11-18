## Yahtzee Pro Play API

- `/newgame`
    - POST
    - <i>Creates a new Game that can be sent moves to play on.</i>
    - Parameters:
        - GameConfiguration
    - Returns
        - Guid id of the Game

- `/games`
    - GET
    - Returns
        - All guids of existing games

- `/games/{guid}`
    - GET
    - Parameters:
        - Guid gameId
    - Returns
        - Game state for game

- `/move`
    - POST
    - Parameters:
        - Guid gameId
        - String move, either "bank" or "roll"
    - Returns
        - New game state for
