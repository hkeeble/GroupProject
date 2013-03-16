// -- DR LAZARUS DEV CONSOLE README -- \\

Player	Creature	ID	[id]			// Generates a new creature for the player with the given ID
			Breed	[id]			// Breeds player's creature with creature of given ID
			GetID				// Not functional
	Item		Add	[id]	[amount]	// Adds given number of items of given ID (amount defaults to 1 if not given)
			Remove	[id]	[amount]	// Removes given number of items of given ID (amount defaults to 1 if not given)
	Boss		GetID				// Not Functional
			ID	[id]			// Sets boss creature to creature of given ID
	LoadLevel	[levelNumber]			// Loads level


Examples:
- Player Creature ID 1234
- Player GetID
- Boss ID 1334
- Item Add 1 3

Commands are NOT case sensitive.