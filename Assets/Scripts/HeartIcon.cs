using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{

    public Sprite fullHeart, emptyHeart;
    Image heartImage;

    // Start is called before the first frame update
    void Awake()
    {
        heartImage = GetComponent<Image>();
    }


    public void SetHeartImage(HeartState status)
    {
        switch(status)
        {
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartState
{
    Empty = 0,
    Full = 1
}