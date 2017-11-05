# PanteonProject

This is a demo project of a basic strategy game. In this project there are 3 main features.

Building Production:
Buildings can be selected from production menu and dropped on the grid.
Even though buildings have the desired size, placement area needs more than that for units to move around.

Unit Production:
When a Barracks building selected, you can select a unit to spawn from the information menu.
Spawn Point of a Barracks becomes active when the user left clicks on a Barracks building. 
After it becomes active the user can right click to place on the grid to set the spawn location of that Barracks.

Unit Movement:
Selecting a unit with left click on the grid makes that unit selected.
Right clicking to a place on the grid makes that point the destination point and unit moves.
Right clicking to a location which is marked as unwalkable gets a new random point near that location.
Right clicking to a location which is out of the grid gets the nearest X coordinate on the grid and keeps the Y coordinate makes it the new destination point.
Unit movements are being kept in a queue so that the user can give more than one movement instruction and the unit would do those one by one.

Good to know details:
There are two bool variables on the tiles.
One is making it for unwalkable. eg. When a building is placed or when a unit stops at a location those tiles becomes unwalkable. 
Unwalkable tiles' colors are being changed to recognize them easily.
The other one is for marking it to check if a unit is on it or not.
unit placement mark is being set before the unit could reach that position to prevent the units being stack on of each other when the unit production button is being mashed continiously.

Known Issues:
When a walkable tile is surrounded with unwalkable tiles and the user wants a unit to move there it gives a there is no path to that location error and stays in its place.
If a unit's spawn location is being set to an unreachable tile, that unit spawns and stops in front of the Barracks. This is the only way to stack units on top of each other.