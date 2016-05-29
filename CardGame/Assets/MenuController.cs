using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MenuController : MonoBehaviour {

    public GameObject joinButton;
    public int player;

    ApiController api;

	// Use this for initialization
	IEnumerator Start () {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetPlayerGames(player);
        var allGames = JSON.Parse(api.result);
        int i = 0;
        while(i < allGames.Count)
        {
            GenerateButton(int.Parse(allGames[i]["id"]), allGames[i]["name"]);
            i++;
        }
	}

    void GenerateButton(int game, string name)
    {
        Debug.Log("Generating button for " + name);
        GameObject entity = Instantiate(joinButton) as GameObject;
        entity.GetComponent<ButtonJoinGame>().SetupButton(game, name);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
