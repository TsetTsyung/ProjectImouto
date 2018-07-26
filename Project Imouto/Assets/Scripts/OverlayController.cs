﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour {

    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private Slider xpSlider;
    [SerializeField]
    private Text xpValue;
    [SerializeField]
    private Text smallHealthBrewText;
    [SerializeField]
    private Text largeHealthBrewText;
    [SerializeField]
    private Text coinText;
    [SerializeField]
    private GameObject emoteMenuObject;
    [SerializeField]
    private GameObject levelUpMenuObject;
    [SerializeField]
    private GameObject missionInfoObject;
    [SerializeField]
    private GameObject missionCompletedObject;
    [SerializeField]
    private Animator overlayAnimator;
    [SerializeField]
    private float messageTextTimeOut;

    private GameObject interactingObject;
    private GameControllerScript gameController;
    private PlayerAnimationController playerAnimationController;
    private LevelUpScreenController levelUpScreenController;
    private PlayerXPController playerXPController;
    private MissionInfoScreenController missionInfoScreenController;
    private MissionCompletedScreenController missionCompletedScreenController;
    private int currentMaxHealth;
    private int currentHealth;

    private string smallHealthBrewHeader = "Small Health Brew: ";
    private string largeHealthBrewHeader = "Large Health Brew: ";
    private string coinHeader = "Coin : ";

    private void Awake()
    {
        GameObjectDirectory.OverlayController = this;
    }

    // Use this for initialization
    void Start() {
        gameController = GameObjectDirectory.GameController;
        playerAnimationController = GameObjectDirectory.PlayerAnimationController;
        levelUpScreenController = GetComponentInChildren<LevelUpScreenController>();
        playerXPController = GameObjectDirectory.PlayerXPController;
        overlayAnimator = GetComponent<Animator>();
        missionInfoScreenController = GetComponentInChildren<MissionInfoScreenController>();
        missionCompletedScreenController = GetComponentInChildren<MissionCompletedScreenController>();
        HideInteractionText();
        HideEmotePanel();
        HideLevelUpPanel();
        HideMissionInfoPanel();
        HideMissionCompletedPanel();
    }

    private void HideInteractionText()
    {
        interactionText.enabled = false;
    }

    public void HideInteractionText(GameObject newObject)
    {
        if (newObject == interactingObject)
        {
            interactionText.enabled = false;
            interactingObject = null;
        }
    }


    public void DisplayInteractionText(GameObject newObject, string textToDisplay)
    {
        if (newObject == interactingObject)
            return;

        interactionText.enabled = true;
        interactionText.text = textToDisplay;

        interactingObject = newObject;
    }

    public void DisplayEmotePanel()
    {
        gameController.PauseGame();
        emoteMenuObject.SetActive(true);
    }

    public void HideEmotePanel()
    {
        gameController.ResumeGame();
        emoteMenuObject.SetActive(false);
    }

    public void DisplayLevelUpPanel()
    {
        gameController.PauseGame();
        levelUpMenuObject.SetActive(true);
        levelUpScreenController.UpdateLevelUpScreen();
    }

    public void HideLevelUpPanel()
    {
        gameController.ResumeGame();
        levelUpMenuObject.SetActive(false);
    }

    public void DisplayMissionInfoPanel(string missionName, string missionText, MissionType missionType, int xpReward, int coinReward)
    {
        if (missionInfoObject.activeSelf)
            return;
        gameController.PauseGame();
        missionInfoObject.SetActive(true);
        missionInfoScreenController.DisplayMissionInfo(missionName, missionText, missionType, xpReward, coinReward);
    }


    public void HideMissionInfoPanel()
    {
        gameController.ResumeGame();
        missionInfoObject.SetActive(false);
    }

    public void DisplayMissionCompletedPanel(string completedMessage)
    {
        gameController.PauseGame();
        missionCompletedObject.SetActive(true);
        missionCompletedScreenController.SetMessage(completedMessage);
    }

    public void HideMissionCompletedPanel()
    {
        gameController.ResumeGame();
        missionCompletedObject.SetActive(false);
    }

    public void HideMessageText()
    {
        messageText.text = "";
        messageText.enabled = false;
    }

    public void DisplayMessageText(string messageToDisplay)
    {
        StartCoroutine(StartTimedMessageText());
        messageText.text = messageToDisplay;
    }

    private IEnumerator StartTimedMessageText()
    {
        yield return new WaitForSeconds(messageTextTimeOut);

        messageText.enabled = false;
    }

    public void UpdateHealthBar(int newHealth)
    {
        healthSlider.value = newHealth;
    }

    public void UpdateStaminaBar(int newStamina)
    {
        staminaSlider.value = newStamina;
    }

    public void UpdateXPBar(int newXP)
    {
        xpSlider.value = newXP;
        xpValue.text = xpSlider.maxValue.ToString() + "/" + xpSlider.value.ToString();
    }

    public void UpdateSmallHealthBrewAmount(int smallHealthBrewAmount)
    {
        smallHealthBrewText.text = smallHealthBrewHeader + smallHealthBrewAmount.ToString();
    }

    public void UpdateLargeHealthBrewAmount(int largeHealthBrewAmount)
    {
        largeHealthBrewText.text = largeHealthBrewHeader + largeHealthBrewAmount.ToString();
    }

    public void UpdateCoinAmount(int newCoinAmount)
    {
        coinText.text = coinHeader + newCoinAmount.ToString();
    }

    public void SetNewMaxHealth(int newMaxHealth)
    { 
        healthSlider.maxValue = newMaxHealth;
    }

    public void SetNewMaxStamina(int newMaxStamina)
    {
        staminaSlider.maxValue = newMaxStamina;
    }

    public void SetNewMaxXP(int newMaxXP)
    {
        if(newMaxXP > xpSlider.maxValue)
            overlayAnimator.SetBool("LevelUpAvailable", true);

        xpSlider.maxValue = newMaxXP;
        xpValue.text = xpSlider.maxValue.ToString() + "/" + xpSlider.value.ToString();
    }

    public void PlayEmote1()
    {
        HideEmotePanel();
        // Now call the animator controller
        playerAnimationController.PlayEmote1();
    }

    public void PlayEmote2()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote2();
    }

    public void PlayEmote3()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote3();
    }

    public void PlayEmote4()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote4();
    }

    public void PlayEmote5()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote5();
    }

    public void PlayEmote6()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote6();
    }

    public void PlayEmote7()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote7();
    }

    public void PlayEmote8()
    {
        HideEmotePanel();
        playerAnimationController.PlayEmote8();
    }
}
