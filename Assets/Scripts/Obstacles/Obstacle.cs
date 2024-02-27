using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Obstacle : MonoBehaviour
{
    [Header("Variables")]
    int removalCost;

    [Header("References")]
    [SerializeField] GameObject _canvas;
    [SerializeField] ShopMenu _shopMenu;
    Collider2D _collider;
    ObstacleCostManager _costManager;
    [SerializeField] TextMeshProUGUI _costText;


    // Start is called before the first frame update
    void Start()
    {

        _collider = GetComponent<Collider2D>();
        _costManager = GetComponentInParent<ObstacleCostManager>();
        _canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        removalCost = _costManager.removalCost;


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // position of mouse click

            if (_collider.OverlapPoint(mousePosition))
            {
                 _costText.text = string.Format("This obstacle will cost {000} gold to remove, are you sure?" ,removalCost);
                 Debug.Log("Clicked on the obstacle");
                 _canvas.SetActive(true);
            }
            


        }
    }

    public void CloseMenu()
    {
        _canvas.SetActive(false);
    }

    public void RemoveObstacle()
    {
        if (_shopMenu.currentGold >= removalCost)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough gold");
        }

    }

    
}
