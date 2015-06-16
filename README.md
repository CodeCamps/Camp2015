# Camp2015

Landry game programming camp for 2015

## Homework [Tuesday]

1. Add attack #2 when player presses `Y` button
2. Add jump attack when player `Location.Z > 0` and player presses `B` (attack) button

Brainstorm some ideas for powerups, characters, and obstacles for the game.

## Features

* **Controller Mappings**
  * `Thumsticks.Left` = move player (`Location.X`, `Location.Y`)
  * `A` = jump
  * `B` = attack #1
  * `Y` = attack #2
  * `X` = charge! (run, then hit opponent)
  * For attack buttons (`B` & `Y`), press and hold for more damage.
* **Player Actions**
  * Idle
  * Walk
  * Run
  * Jump
  * Attack 1
  * Attack 2?
  * Attack 3?
  * Jump & Attack
  * Knocked Out
* **Player Stats**
  * ShadowColor
  * PlayerIndex
  * Location (x,y,z - Z is jump height)
  * Health (< 1 = death)
  * AttackDamage (-N Health from opponent)
  * AttackBonus (from powerups)
  * AttackBonusTime
  * InvincibleTime (after getting hit, or via powerup)
  * AttackBoostTime (press and hold before attacking)
* **Player Effects**
  * Glowing Column (for AttackBoostTime)
  * Sparks (for hits)
  * Flash Transparent (after getting hit)
* **Obstacles**
  * ~~Sink Hole (immediate death)~~
  * Spikes (-N Health)
  * Falling Rocks (-N Health)
* **Screens**
  * Title
  * Game
  * Credits
