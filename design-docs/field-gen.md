## Plan
The basic design for a defense. A plan consists of yard-stamps and the enemies that will be dropping at those yard stamps. Each yard stamp has a **Move**, which consists of the enemies at that yard.

```json
{
    "50": [
        "CraftyBoi",
        "CraftyBoi",
        "CraftyBoi",
        "MenacingBoi"
    ],
    "100": [
        "MenacingBoi",
        "MenacingBoi"
    ],
    ...
}
```

This plan should be evaluated by a standardized function that determines its difficulty. A plan is the total difficulty score for all of its moves, which in turn are determined by the total difficulty score for all of its enemies in that move. Earlier moves are given more weight to them, as they will have a higher chance of defeating the player too early.

To determine the next plan, randomly generate a set of plans and evaluate them against a desired difficulty score, choosing the one that is closest without going over. The desired score should keep going up as the player progresses to harder and harder fields.

### Move Function
```ts
function generateMove(knownEnemies: Enemy[], potentialEnemies: Enemy[], move: number, totalMoves: number) {
    const move = [];
    const newEnemyPercent = move / totalMoves;
    const numberOfEnemies

    for (let e = 0; e < )
}
```