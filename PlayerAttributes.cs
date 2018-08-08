using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*-------------------------- PlayerAttributes: MonoBehaviour -------------------
   |  Purpose:  Keeps track of all player related attributes    
   |
   |  Author: Jordan Pitner
   |  Date: August 8th, 2018
   *-------------------------------------------------------------------*/
public class PlayerAttributes : MonoBehaviour
{

    // Global Variables
    #region Global Variables

    public int Health { get; private set; }
    public int Stamina { get; private set; }

    [SerializeField]
    private float StaminaRechargeRate = 1;

    private IEnumerator decreaseStamina;

    public bool Grounded { get; set; }
    public bool Crouched { get; set; }
    public bool Sprinting { get; set; }

    #endregion

    /*-------------------------- Start() -------------------
   |  Function Start
   |
   |  Purpose: Used for setup of variables
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    void Start()
    {
        Health = 100;
        Stamina = 100;

        // Start the Stamina recharge
        InvokeRepeating("RechargeStamina", 1, 1);
    }

    /*-------------------------- Action(string action) -------------------
    |  Function Action
    |
    |  Purpose: Process the character action the player is performing
    |
    |  Parameters: Action - string to determine which action to take
    |	      
    |  Returns:  Nothing
    *-------------------------------------------------------------------*/
    public void Action(string action)
    {
        switch (action)
        {
            case "jump":
                Stamina -= 10;
                Debug.Log("Total Stamina: " + Stamina);
                break;
            case "pounce":
                Stamina -= 25;
                Debug.Log("Total Stamina: " + Stamina);
                break;
            case "cling":
                // Make sure coroutines don't stack
                if (decreaseStamina == null)
                {
                    decreaseStamina = DecreaseStamina(2);
                    StartCoroutine(decreaseStamina);
                }
;
                break;
            case "sprint":
                // Make sure coroutines don't stack
                if (decreaseStamina == null)
                {
                    decreaseStamina = DecreaseStamina(2);
                    StartCoroutine(decreaseStamina);
                }

                break;
            case "stop":
                // Stop decreasing Stamina
                StopCoroutine(decreaseStamina);
                decreaseStamina = null;
                break;
            default:
                break;
        }
    }


    /*-------------------------- RechargeStamina() -------------------
    |  Function RechargeStamina
    |
    |  Purpose:  Will get called with InvokeRepeating, continuously increase Stamina variable by rate
    |
    |  Parameters: None
    |	      
    |  Returns:  Nothing
    *-------------------------------------------------------------------*/
    private void RechargeStamina()
    {
        if (!Sprinting && Stamina < 100)
        {
            Stamina += Mathf.RoundToInt(1 * StaminaRechargeRate);
        }

        Debug.Log("Total Stamina: " + Stamina);
    }

    /*-------------------------- DecreaseStamina(int rate) -------------------
   |  Function RechargeStamina
   |
   |  Purpose:  Coroutine to get Stamina to decrease at a set rate every second
   |
   |  Parameters: Rate - the rate at which Stamina should be decreased
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    IEnumerator DecreaseStamina(int rate)
    {
        while (true)
        {
            Stamina -= rate;
            Debug.Log("Total Stamina: " + Stamina);
            yield return new WaitForSeconds(1f);
        }
    }
}
