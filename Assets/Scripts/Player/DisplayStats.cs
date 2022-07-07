using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    public PlayerState playerStateScript;

    public EquippedWeapon equippedWeaponScript;

    public TextMeshProUGUI healthText;

    public float maxHealth;

    public Slider healthBar;

    private void Start()
    {
        maxHealth = playerStateScript.health;
    }

    void Update()
    {
        if (GlobalVars.levelComplete == false)
        {
            healthText.text = "<size=96>" + Mathf.Round(playerStateScript.health * 100f) / 100f + "</size>/" + maxHealth;
            healthBar.value = playerStateScript.health / maxHealth;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
