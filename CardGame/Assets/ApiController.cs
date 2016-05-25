using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class ApiController : MonoBehaviour {

    public string baseUrl;
    public string cardUrl;

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
