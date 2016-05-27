using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class ApiController : MonoBehaviour {

    public string baseUrl;
    public string cardUrl;
    public string stateUrl;
    public string purchaseUrl;

    private bool hasResult;
    public string result;

	// Use this for initialization
	void Start () {
        hasResult = false;
	}
	
    public IEnumerator GetCardInfo(int id)
    {
        string call = cardUrl + id.ToString();
        yield return GetApiJSONResults(call);
    }

    public IEnumerator GetHandInfo(int game, int player)
    {
        string call = stateUrl.Replace("{{GAME}}", game.ToString());
        call = call.Replace("{{PLAYER}}", player.ToString());
        yield return GetApiJSONResults(call);
    }

    public IEnumerator PostPurchaseInfo(int game, int player, int card)
    {
        Debug.Log("purchasing card");
        string call = purchaseUrl.Replace("{{GAME}}", game.ToString());
        call = call.Replace("{{PLAYER}}", player.ToString());
        call = call.Replace("{{CARD}}", card.ToString());
        yield return GetApiJSONResults(call);
    }

    IEnumerator GetApiJSONResults(string call)
    {
        hasResult = false;
        string url = baseUrl + call;
        WWW web = new WWW(url);
        yield return web;
        if (web.error == null)
        {
            hasResult = true;
            result = web.text;
        }
        else
        {
            Debug.Log("ApiController.GetApiJSONResults: Could not read from " + url + "!");
            hasResult = true;
            result = "";
        }
    }
}
