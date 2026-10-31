# Lab_10

**<ins>How to Use the Save/Load System<ins>** <br>
Press **S** to save and **L** to load. Saving stores the score, the rifle's location and the location of every duck on screen. Loading puts them all back. Bullets are not saved.

**<ins>How it was Implemented<ins>** <br>
**ISaveable** is an interface with a 'SaveID', a 'SavedData' property that returns the object's data as JSON, and a 'LoadFromData' method that restores it. **SaveableBehavior** is a base class that implements it and gives each object its own GUID as its SaveID.<br>
**TransformSaver** extends 'SaveableBehavior' and saves an object's position, rotation, and scale. It's attached to the rifles object in the hierarchy. the duck spawner also uses it to save and restore each duck's location.<br>
The ducks are spawned during play, so the spawner saves them itself. It stores each duck's type, lane, and transform; and when the game is loaded it removes the current ducks and rebuilds the saved ones in the same spots.<br>
**SavingService** handles the files:
* **Locations (JSON):** it findes every 'ISaveable' in the scene, collects their data, and writes it to 'Positions.json' using Unity's 'JsonUtility'. Loading reads the file and gives each object its data back by matching the SaveID.
* **Score (binary):** the score is written to 'Score.bin' with a 'BinaryWriter' and read back with a 'BinaryReader'.

Both files are saved in 'Application.persistentDataPath'. Scene saving was left out since the game only has one scene.
