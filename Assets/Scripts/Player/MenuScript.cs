/****
 * Created by: Bridget Kurr
 * Date Created: Nov 05, 2022
 * 
 * Last Edited:
 * Last Edited by:
 ****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuScript : MonoBehaviour
{

    [Header("Text Boxes")]
    public TMP_Text titleTextbox; //textbox for the title
    public TMP_Text creditsTextbox; //textbox for the credits
    public TMP_Text copyrightTextbox; //textbox for the copyright
    public TMP_Text endMessageTextbox; //textbox for the end message


    // Start is called before the first frame update
    void Start()
    {
       //set the value for the textboxes 
    }

   public void OnGameStart()
    {
        Debug.Log("Game Started");
    } //OnGameStart()

    public void OnGameExit()
    {
        Debug.Log("Game Exited");
    } //OnGameExit()
}
