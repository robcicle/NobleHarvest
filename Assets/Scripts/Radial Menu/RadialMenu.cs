using TMPro;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public GameObject theMenu;

    public Vector2 moveInput;

    public TextMeshProUGUI[] options;

    public Color normalColor, highlightedColor;

    public int selectedOption;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            theMenu.SetActive(true);

        }

        if (theMenu.activeInHierarchy)
        {
            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
            moveInput.Normalize();

            //Debug.Log(moveInput);

            if (moveInput != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                angle *= 180f;
                angle += 90f;
                if (angle < 0)
                {
                    angle += 360;
                }

                //Debug.Log(angle);

                for (int i = 0; i < options.Length; i++)
                {
                    if (angle > i * (360 / options.Length) && angle < (i + 1) * (360 / options.Length))
                    {
                        options[i].color = highlightedColor;
                        selectedOption = i;
                    }
                    else
                    {
                        options[i].color = normalColor;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                switch (selectedOption)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;


                }
                theMenu.SetActive(false);
            }


        }

    }
}
