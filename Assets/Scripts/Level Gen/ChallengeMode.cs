using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dice.Player{

    public class ChallengeMode : MonoBehaviour
    {
        
        public PlayerCharacterController pcController;
        private bool TimerInProgress = true;
        public float timeRemaining = 10;
        private bool textEnabled = true;
        public Text timerText;
        public Text challengeText;

        void Start()
        {
            TimerInProgress = true;
        }
        
        void Update()
        {
            if (timeRemaining > 0 && TimerInProgress)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = Mathf.Round(timeRemaining).ToString();
            }
            else{ // win condition
                TimerInProgress = false;
                timeRemaining = 0;
                timerText.text = timeRemaining.ToString();
                if(pcController.currentHealth > 0){
                    challengeText.text = "SUCCESS!";
                    // if(textEnabled == true){
                    //     StartCoroutine(DisableText());
                    // }
                    //make money!
                }
            }
            // lose condition
            if(TimerInProgress && pcController.currentHealth <= 0){
                TimerInProgress = false;
                // if(textEnabled == true){
                //     StartCoroutine(DisableText());
                // }
            }
        }

        // private IEnumerator DisableText()
        // {
        //     yield return new WaitForSeconds(2f);
        //     challengeText.text = "";
        //     timerText.text = "";
        //     textEnabled = false;
        // }
    }
}