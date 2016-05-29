using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GameController : MonoBehaviour {

    ApiController api;

    public GameObject statePrefab;

    public string json;
    public int gameId;
    public float offsetFromCenter;
    public int player;

    public int currentPlayerTurn;

    public GameObject diceRoller;

	// Use this for initialization
	void Start () {
        gameObject.SendMessage("GenerateGame");
	}
	
    IEnumerator GenerateGame()
    {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetGameInfo(gameId);
        json = api.result;
        var gameInfo = JSON.Parse(json);
        currentPlayerTurn = int.Parse(gameInfo["turn"]);
        int i = 0;
        while(i < gameInfo["players"].Count)
        {
            Debug.Log("generate a player");
            yield return GeneratePlayer(int.Parse(gameInfo["players"][i]), i, gameInfo["players"].Count);
            i++;
        }
    }

    IEnumerator GeneratePlayer(int id, int playerId, int numPlayers)
    {
        bool myTurn = (Mathf.Abs(currentPlayerTurn) == player) && (player == id);
        if (myTurn && (currentPlayerTurn < 0))
            DisplayDiceWidget();
        float rotation = (360 / numPlayers) * playerId;
        GameObject entity = GameObject.Instantiate(statePrefab) as GameObject;
        StateSetup entityState = entity.GetComponent<StateSetup>();
        entityState.game = gameId;
        entityState.player = id;
        yield return entityState.GenerateHand(myTurn);
        //entity.transform.localPosition = entity.transform.localPosition + (Vector3.down * offsetFromCenter);
        Vector3 newPosition = RotateVector2D(Vector3.down * offsetFromCenter, rotation);
        entity.transform.localPosition = entity.transform.localPosition + newPosition;
        entity.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.forward * rotation));
    }

    private void DisplayDiceWidget()
    {
        Debug.Log("Display dice widget.");
        diceRoller.SetActive(true);
    }

    private Vector3 RotateVector2D(Vector3 oldDirection, float angle)
    {
        float newX = Mathf.Cos(angle * Mathf.Deg2Rad) * (oldDirection.x) - Mathf.Sin(angle * Mathf.Deg2Rad) * (oldDirection.y);
        float newY = Mathf.Sin(angle * Mathf.Deg2Rad) * (oldDirection.x) + Mathf.Cos(angle * Mathf.Deg2Rad) * (oldDirection.y);
        float newZ = oldDirection.z;
        return new Vector3(newX, newY, newZ);
    }
}
