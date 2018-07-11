using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour {

    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private GameObject emoteMenuObject;

    private GameObject interactingObject;
    private int currentMaxHealth;
    private int currentHealth;


    private void Awake()
    {
        GameObjectDirectory.OverlayController = this;
    }

    // Use this for initialization
    void Start() {
        HideInteractionText();
        HideEmotePanel();
    }

    // Update is called once per frame
    void Update() {

    }

    private void HideInteractionText()
    {
        interactionText.enabled = false;
    }

    public void HideInteractionText(GameObject newObject)
    {
        if (newObject == interactingObject)
            interactionText.enabled = false;
    }

    public void DisplayInteractionText(GameObject newObject, string textToDisplay)
    {
        interactionText.enabled = true;
        interactionText.text = textToDisplay;

        interactingObject = newObject;
    }

    public void DisplayEmotePanel()
    {
        Time.timeScale = 0;
        emoteMenuObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;  // TODO: select the player's chosen lockmode
    }

    public void HideEmotePanel()
    {
        Time.timeScale = 1;
        emoteMenuObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateHealthBar(int newHealth)
    {
        healthSlider.value = newHealth;
    }

    public void UpdateStaminaBar(int newStamina)
    {
        staminaSlider.value = newStamina;
    }

    public void SetNewMaxHealth(int newMaxHealth)
    { 
        healthSlider.maxValue = newMaxHealth;
    }

    public void SetNewMaxStamina(int newMaxStamina)
    {
        staminaSlider.maxValue = newMaxStamina;
    }
}
