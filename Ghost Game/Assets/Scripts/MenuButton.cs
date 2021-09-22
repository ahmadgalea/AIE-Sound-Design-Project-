using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = Color.white;
    }
}
