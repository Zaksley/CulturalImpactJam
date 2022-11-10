/****
 * Created by: Bridget Kurr
 * Date Created: Nov 05, 2022
 * 
 * Last Edited:
 * Last Edited by:
 ****/

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [Header("Text Boxes")]
    public TMP_Text titleTextbox; //textbox for the title
    public TMP_Text creditsTextbox; //textbox for the credits
    public TMP_Text copyrightTextbox; //textbox for the copyright
    public TMP_Text endMessageTextbox; //textbox for the end message
    
    void Start()
    {
       //set the value for the textboxes 
    }

   public void OnGameStart()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
   } 

    public void OnGameExit()
    {
        Application.Quit();
    } 
}
