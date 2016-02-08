// Tom Wexler
// Example program to help you get started with your project.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemManager : MonoBehaviour {
	
	GameObject gemFolder;	// This will be an empty game object used for organizing objects in the Hierarchy pane.
	List<Gem> gems;			// This list will hold the gem objects that are created.
    List<Tile>  emptyTiles;
	int gemType; 			// The next gem type to be created.
    private const float gemSpawnInterval = 1.5f;
    float clock = 0;
    BoardManager bm;
	
    public void init(BoardManager boardMan, Tile[,] board) {
        emptyTiles = new List<Tile>();
        bm = boardMan;
        gemFolder = new GameObject();
        gemFolder.name = "Gems";        // The name of a game object is visible in the hHerarchy pane.
        gems = new List<Gem>();
        gemType = 1;
        for (int x=0; x<board.GetLength(0); x++) {
            for (int y=0; y<board.GetLength(1); y++) {
                emptyTiles.Add(board[x, y]);
            }
        }
    }

	// Update is called every frame.
	void Update () {
        clock += Time.deltaTime;
		if (clock >= gemSpawnInterval * gems.Count) {
            print("Adding gem.");
            clock = 0;
            int index = Random.Range(0, emptyTiles.Count);
            Tile targetTile = emptyTiles[index];
            Vector3 coordinates = targetTile.transform.position;
            addGem(coordinates.x, coordinates.y);
            emptyTiles.RemoveAt(index);
		}
	}

	void addGem(float x, float y) {
		GameObject gemObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Gem gem = gemObject.AddComponent<Gem>();			// Add the Gem.cs script to the object.
															// We can now refer to the object via this script.
		gem.transform.parent = gemFolder.transform;			// Set the gem's parent object to be the gem folder.
		gem.transform.position = new Vector3(x,y,BoardManager.GemZ);		// Position the gem at x,y.								
		
		gem.init(gemType, this);							// Initialize the gem script.
		
		gems.Add(gem);										// Add the gem to the Gems list for future access.
		gem.name = "Gem "+gems.Count;						// Give the gem object a name in the Hierarchy pane.
		
		gemType = (gemType%4) + 1;							
	}
}