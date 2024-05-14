using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    private RectTransform _rectTransform;

    public float percentageWidth, percentageHeight;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = Screen.width;
        float height = Screen.height;

        _rectTransform.sizeDelta = new Vector2(width * percentageWidth, height * percentageHeight);
        _rectTransform.transform.position = new Vector2(width / 2, height / 2);
    }
}
