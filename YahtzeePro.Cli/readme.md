# CLI structure

## Status
`status`

Check if the servers are running.
There are three:
- Play Server
- Strategies Server
- Optimum Server

``` bash
> dotnet run -- status
Connected to YatzeePro Optimum API server!
Connected to YatzeePro Play API server!
```

## Config

### Config List
`config list`

View the configuration variables set for the CLI. They are used as defaults for expected arguments or options in other commands.

### Config Set
`config set <variable> <value>`

Set up a configuration value. It must be a valid variable for the CLI.
- GAMEID (guid)
- GAMECONFIG_WINNINGVALUE (int)
- GAMECONFIG_TOTALDICE (int)
- AUTO_UPDATE_CONFIG (bool)

## Strategies

### Strategies List
`strategies list`

View the strategies saved in the player server.

### Strategies Get
`strategies get <name>`

View details (TBC) of a strategy saved in the player server.

### Strategies Simulate
`strategies simulate <strategy1> <strategy2>`

Simulate a game or sets of games between two strategies.
