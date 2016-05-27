using UnityEngine;
using System.Collections;
using SimpleJSON;

public class StateSetup : MonoBehaviour {

    public int game, player;
    public GameObject cardPrefab;

    public Transform purchasablePosition, landmarkPosition, inventoryPosition;
    public float cardSpacing;

    private ApiController api;
    public string json;

	// Use this for initialization
	IEnumerator Start () {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return GenerateHand();
    }

    IEnumerator GetHandInfo()
    {
        yield return api.GetHandInfo(game, player);
        json = api.result;
    }
	
	public IEnumerator GenerateHand()
    {
        yield return GetHandInfo();
        Debug.Log("generating hand");
        var handInfo = JSON.Parse(json);
        yield return GenerateInventory(handInfo["inventory"]);
        yield return GeneratePurchasable(handInfo["purchasable"]);
        yield return GenerateLandmarks(handInfo["landmarks"]);
    }

    IEnumerator GenerateInventory(JSONNode inventory)
    {
        int i = 0;
        while(i < inventory.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(inventory[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard();
            entity.transform.localPosition = inventoryPosition.localPosition + (Vector3.right * i * cardSpacing);
            i++;
        }
    }

    IEnumerator GeneratePurchasable(JSONNode purchasable)
    {
        int i = 0;
        while (i < purchasable.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(purchasable[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard();
            entity.transform.localPosition = purchasablePosition.localPosition + (Vector3.right * i * cardSpacing);
            i++;
        }
    }

    IEnumerator GenerateLandmarks(JSONNode landmarks)
    {
        int i = 0;
        while (i < landmarks.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//TODO Do something with flipped cards.
            entity.GetComponent<CardSetup>().SetCardId(Mathf.Abs(int.Parse(landmarks[i])));
            yield return entity.GetComponent<CardSetup>().GenerateCard();
            entity.transform.localPosition = landmarkPosition.localPosition + (Vector3.right * i * cardSpacing);
            i++;
        }
    }
}
