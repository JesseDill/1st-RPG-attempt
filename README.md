# 1st-RPG-attempt
Inspired by torchlight
Developed during summer of 2020

So far this game has the following features:

 Movement

    -Based on where the mouse moves and clicks, player moves to that location on the birds-eye-view like camera angle
    
    -mouse clicks pass through objects and entities covering the camera as a ray until it bounces off the floor and 
     only then does the player proceed to move
     
 Combat
    
    -Click based combat in which if click ray bounces off an attackable entity, player initiates attacking animation
    -Based on equiped item, player attacks with different range/melee weapon along with their associated animations
    -Enemies (based on player entity) patrol throughout map with their actions 
  
 Enemies
 
    -Based on player entity with actions governed by a finite-state machine consisting of attacking, chasing, dead, and idle states
    -Have different patrolling routes and attack types (different melee/range items)
 
 Items
  
    -All weapons used by player can be dropped in environment and can be picked up when player mouse clicks on item
    -Associated stats and abilities delegated to the item
 
 Saving
 
    -All entities and items in world environment are stored on save and restored on load to the correct position with their correct stats and state
    -Player items and abilities and transfered from scene to scen in the game world, along with the presence of enemies in pursuit of the player
    -World state of items and entities is restored on player death to last checkpoint, along with the last scene being restored
  
