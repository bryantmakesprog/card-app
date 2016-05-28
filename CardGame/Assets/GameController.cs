using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GameController : MonoBehaviour {

    ApiController api;

    public GameObject statePrefab;

    public string json;
    public int gameId;

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
        int i = 0;
        while(i < gameInfo["players"].Count)
        {
            Debug.Log("generate a player");
            yield return GeneratePlayer(int.Parse(gameInfo["players"][i]));
            i++;
        }
    }

    IEnumerator GeneratePlayer(int id)
    {
        GameObject entity = GameObject.Instantiate(statePrefab) as GameObject;
        StateSetup entityState = entity.GetComponent<StateSetup>();
        entityState.game = gameId;
        entityState.player = id;
        yield return entityState.GenerateHand();
    }
}
