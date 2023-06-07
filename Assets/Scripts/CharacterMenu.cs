using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour {
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    //Logic fields
    private int currentCharacterSelect = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    //Char Selection
    public void OnArrowClick(bool right) {
        if (right) {
            currentCharacterSelect++;

            //if too far from array
            if (currentCharacterSelect == GameManager.instance.playerSprites.Count)
                currentCharacterSelect = 0;
            OnSelectionChanged();
        } else {
            currentCharacterSelect--;

            //if too far from array
            if (currentCharacterSelect < 0)
                currentCharacterSelect = GameManager.instance.playerSprites.Count - 1;
            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged() {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelect];
        GameManager.instance.player.SwapSprite(currentCharacterSelect);
    }

    //weapon upgrading
    public void OnUpgradeClick() {
        if (GameManager.instance.tryUpgradeWeapon())
            updateMenu();
    }

    //update character info (on menu button click)
    public void updateMenu() {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count) {
            upgradeCostText.text = "MAX";
        } else {
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }


        //Meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //Xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();

        if (GameManager.instance.GetCurrentLevel() == GameManager.instance.xpTable.Count) {
            xpText.text = GameManager.instance.experience.ToString() + "total experience"; //Display total xp
            xpBar.localScale = Vector3.one;
        } else {
            int prevLevelXp = GameManager.instance.GetXPToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXPToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpToLevel = GameManager.instance.experience - prevLevelXp;
            //put it all together
            float completionRatio = (float)currXpToLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpToLevel.ToString() + " / " + diff;
        }
    }

}
