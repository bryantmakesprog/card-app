//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script that shows how to download a remote texture and assign it to be used by a UITexture.
/// </summary>

[RequireComponent(typeof(UITexture))]
public class CardTexture : MonoBehaviour
{
    public bool pixelPerfect = true;

    Texture2D mTex;

    public IEnumerator DownloadTexture(string textureUrl)
    {
        string url = textureUrl;
        WWW www = new WWW(url);
        yield return www;
        mTex = www.texture;

        if (mTex != null)
        {
            UITexture ut = GetComponent<UITexture>();
            ut.mainTexture = mTex;
            if (pixelPerfect) ut.MakePixelPerfect();
        }
        www.Dispose();
    }

    void OnDestroy()
    {
        if (mTex != null) Destroy(mTex);
    }
}
