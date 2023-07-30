## Yahtzee Pro analysis

Assume 1v1

Variables
- **Si** = Score of current player
- **So** = Score of opponent
- **C** = current cached score
- **D** = Dice count available to roll

These variables make up a **game state**
Assume that winning is just getting above 5000,
instead of getting exactly 5000.

Pi(Si, So, C, D) = Probability of player winning at game state
Po(Si, So, C, D) = Probability of opponent winning at game state

On a turn, the player can roll or bank

Probability of winning is the max of:
the probability of winning if you roll
the probability of winnning if you bank

Probability if chosing to bank
	Sb(Si, So, C, D) = P(So, Si+C, 0, 0) | P(So, Si+C, C, D)
Note that a player can choose to start from 5 dice, or keep going with the cache and reduced dice score

Assume that three+ of a kind does not count

Probability of losing cache score
Not rolling any 1s or 5s
	FailP(D) = (4/6)^D
Probability of "roll over"
Only rolling 1s or 5s
	RlOvP(D) = (2/6)^D
Probability of continuing to score (but not rolling over)
Rolling some 1s or 5s, but also some non 1s and 5s
	ContP(D) = 1 - FailP(D) - RlOvP(D)
	ContP(D) = 1 - 4^D/6^D - 2^D/6^D
	ContP(D) = 1 - (4^D + 2^D)/6^D

Probability of winning if chosing to roll
	Pr(Si, So, C, D) =
	FailP(D)*P(So, Si, 0, 5) + 
	RlOvP(D)*P(Si, So, C + RlOvScore, 5) +
	ContP(D)*P(Si, So, C + ContScore, D - DiceUsed)
where DiceUsed < D
