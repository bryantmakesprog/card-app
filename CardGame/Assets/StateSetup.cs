using UnityEngine;
using System.Collections;
using SimpleJSON;

public class StateSetup : MonoBehaviour {

    public int game, player, points;
    public GameObject cardPrefab;

    public Transform purchasablePosition, landmarkPosition, inventoryPosition;
    public float cardSpacing;

    private ApiController api;
    public string json;

    IEnumerator GetHandInfo()
    {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetHandInfo(game, player);
        json = api.result;
    }
	
	public IEnumerator GenerateHand()
    {
        yield return GetHandInfo();
        Debug.Log("generating hand for player " + player);
        var handInfo = JSON.Parse(json);
        points = int.Parse(handInfo["points"]);
        bool isOwned = player == GameObject.FindObjectOfType<InputController>().player;
        yield return GenerateInventory(handInfo["inventory"], isOwned);
        yield return GeneratePurchasable(handInfo["purchasable"], isOwned);
        yield return GenerateLandmarks(handInfo["landmarks"], isOwned);
    }

    IEnumerator GenerateInventory(JSONNode inventory, bool isOwned)
    {
        int i = 0;
        bool isPurchasable = false;
        while(i < inventory.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(inventory[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = inventoryPosition.localPosition + (Vector3.right * i * cardSpacing);
            entity.transform.parent = gameObject.transform;
            i++;
        }
    }

    IEnumerator GeneratePurchasable(JSONNode purchasable, bool isOwned)
    {
        int i = 0;
        bool isPurchasable = true;
        while (i < purchasable.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(purchasable[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = purchasablePosition.localPosition + (Vector3.right * i * cardSpacing);
            entity.transform.parent = gameObject.transform;
            i++;
        }
    }

    IEnumerator GenerateLandmarks(JSONNode landmarks, bool isOwned)
    {
        int i = 0;
        bool isPurchasable = true;
        while (i < landmarks.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            isPurchasable = int.Parse(landmarks[i]) > 0;
            entity.GetComponent<CardSetup>().SetCardId(Mathf.Abs(int.Parse(landmarks[i])));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = landmarkPosition.localPosition + (Vector3.right * i * cardSpacing);
            entity.transform.parent = gameObject.transform;
            i++;
        }
    }
}
