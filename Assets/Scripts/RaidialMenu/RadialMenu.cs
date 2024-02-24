using TMPro;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    //Should be the background
    public GameObject theMenu;

    public Vector2 moveInput;
    //Allows you to drag in the text boxes, may require you to do in specific order IE clockwise
    public TextMeshProUGUI[] options;
    //What colour you want it to turn when hovering
    public Color normalColor, highlightedColor;
    //Which options hovered
    public int selectedOption;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Sets the menu as active when tab is held
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            theMenu.SetActive(true);

        }

        if (theMenu.activeInHierarchy)
        {
            //mouse pos
            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
            moveInput.Normalize();


            //If mouse isnt at middle of screen
            if (moveInput != Vector2.zero)
            {
                //Turning mouse position into an angle
                float angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                angle *= 180f;
                angle += 90f;
                if (angle < 0)
                {
                    angle += 360;
                }


                //Dpending on ammount of options in the options array creates zones of the correct size evenly
                for (int i = 0; i < options.Length; i++)
                {
                    if (angle > i * (360 / options.Length) && angle < (i + 1) * (360 / options.Length))
                    {
                        //just makes it change colour
                        options[i].color = highlightedColor;
                        //Set selected option to whatever option is currently hovered
                        selectedOption = i;
                    }
                    else
                    {
                        options[i].color = normalColor;
                    }
                }
            }


            //Use this to call functions, Simply add/remove cases or change this to work to call functions however you like
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
