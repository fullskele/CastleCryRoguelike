using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() {
        //delete duplicate gamemanagers, players, textmanagers
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        //Delete ALL data, make function later?
        //PlayerPrefs.DeleteAll();

        OnHitpointChange();

        instance = this;
        SceneManager.sceneLoaded -= LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

        //SceneManager.sceneLoaded += LoadState;
    }

    //resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //references
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnimator;
    public GameObject hud;
    public GameObject menu;

    //logic
    public int pesos;
    public int experience;
    [HideInInspector]
    public int floorLevel = 1;

    //floating text gen
    public void showText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Upgrade weapon
    public bool tryUpgradeWeapon() {
        //is weapon level maxed?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if (pesos >= weaponPrices[weapon.weaponLevel]) {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }


        return false;
    }

    //Hitpoint bar
    public void OnHitpointChange() {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);

    }

    //Experience system
    public int GetCurrentLevel() {
        int r = 0;
        int add = 0;

        while (experience >= add)    {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) //max level
                return r;
        }
        return r;
    }

    public int GetXPToLevel(int level) {

        int r = 0;
        int xp = 0;

        while (r < level) {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp) {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel()) {
            OnLevelUp();
        }
    }

    public void OnLevelUp () {
        OnHitpointChange();
        player.OnLevelUp();
    }

    // On scene load
    public void OnSceneLoaded(Scene s, LoadSceneMode mode) {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    public void Respawn() {
        deathMenuAnimator.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }


    //SAVE/LOAD

    //INT characterChoice
    //INT pesos
    //INT experience
    //INT weaponlevel
    public void SaveState() {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();
     
        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode) {

        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        //player.transform.position = GameObject.Find("SpawnPoint").transform.position;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //TODO change player skin
        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }
} 
