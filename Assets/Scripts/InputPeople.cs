using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputPeople : MonoBehaviour
{
    public InputField Vegetarians;
    public InputField Cannibals;
    public static int inputV;
    public static int inputC;
    private bool isNumericV, isNumericC;

    public GameObject popUp;
    public Text popUpMessage;

    public void StartMain()
    {
        isNumericV = int.TryParse(Vegetarians.text, out inputV);
        isNumericC = int.TryParse(Cannibals.text, out inputC);
        if (!isNumericC || !isNumericV || inputV > 50 || inputC > 50 || inputC < 0 || inputV < 0)
        {
            PopUp("Számadatot adjon meg 0 - 50 között!");
        }
        else
        {
            if ((inputC == 0 && inputV == 0) || (inputC == 1 && inputV == 0) || (inputC == 0 && inputV == 1))
            {
                PopUp("Nincs értelme a feladatnak!");
            }
            else if ((inputC > inputV) || ((inputV == inputC) && (inputC > 3) && (inputV >3)))
            {
                PopUp("Nincs megoldás!");
            }
            else
            {
                SceneManager.LoadScene("Main");
            }          
        }
    }
    private void PopUp(string message)
    {
        popUpMessage.text = message;
        popUp.SetActive(true);
        Vegetarians.text = "";
        Cannibals.text = "";
        Vegetarians.enabled = false;
        Cannibals.enabled = false;
    }
}
