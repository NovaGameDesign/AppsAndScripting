using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [Header("Saving System")]
    [SerializeField] GameObject player;
    [SerializeField] playerInventory inventory;
    [SerializeField] PlayerStats stats;

    [SerializeField] GameObject enemies;
    [SerializeField] GameObject items;

    public void SaveGameData()
    {
        Save save = CreateSaveData();
        
        XmlDocument xmlDoc = new XmlDocument();

        #region CreateXML Elements
        XmlElement root = xmlDoc.CreateElement("saved");
        

        XmlElement playerHealth = xmlDoc.CreateElement("PlayerHealth");
        playerHealth.InnerText = save.playerHealth.ToString();
        root.AppendChild(playerHealth);
        //Player Position 
        XmlElement PlayerPositionX = xmlDoc.CreateElement("PlayerPositionX");
        PlayerPositionX.InnerText = save.playerPositonX.ToString();
        root.AppendChild(PlayerPositionX);

        XmlElement PlayerPositionY = xmlDoc.CreateElement("PlayerPositionY");
        PlayerPositionY.InnerText = save.playerPositonY.ToString();
        root.AppendChild(PlayerPositionY);

        XmlElement PlayerPositionZ = xmlDoc.CreateElement("PlayerPositionZ");
        PlayerPositionZ.InnerText = save.playerPositonZ.ToString();
        root.AppendChild(PlayerPositionZ);
        //Player Rotation 
        /*
        XmlElement PlayerRotationX = xmlDoc.CreateElement("PlayerRotationX");
        PlayerRotationX.InnerText = save.playerRotationX.ToString();
        root.AppendChild(PlayerRotationX);*/


        foreach(var item in save.items)
        {
            XmlElement savedItem = xmlDoc.CreateElement("Item");
            savedItem.InnerText = item.name;
            root.AppendChild(savedItem);
        }

        /*foreach (var quickItem in save.quickItems)
        {
            XmlElement savedItem = xmlDoc.CreateElement("QuickItem");
            savedItem.InnerText = quickItem.name;
            root.AppendChild(savedItem);
        }*/
        XmlElement savedEnemy, enemyIsAlive, savedEnemyPositionX, savedEnemyPositionY, savedEnemyPositionZ;
        for (int i = 0; i < save.enemyStillAlive.Count; i++)
        {
            savedEnemy = xmlDoc.CreateElement("Enemy");
            enemyIsAlive = xmlDoc.CreateElement("EnemyIsAlive");
            savedEnemyPositionX = xmlDoc.CreateElement("EnemyPositionX");
            savedEnemyPositionY = xmlDoc.CreateElement("EnemyPositionY");
            savedEnemyPositionZ = xmlDoc.CreateElement("EnemyPositionZ");

            savedEnemyPositionX.InnerText = save.enemyPositionX[i].ToString();
            savedEnemyPositionY.InnerText = save.enemyPositionY[i].ToString();
            savedEnemyPositionZ.InnerText = save.enemyPositionZ[i].ToString();
            enemyIsAlive.InnerText = save.enemyStillAlive[i].ToString();

            savedEnemy.AppendChild(savedEnemyPositionX);
            savedEnemy.AppendChild(savedEnemyPositionY);
            savedEnemy.AppendChild(savedEnemyPositionZ);
            savedEnemy.AppendChild(enemyIsAlive);

            root.AppendChild(savedEnemy);
        }

        #endregion

        xmlDoc.AppendChild(root);

        xmlDoc.Save(Application.dataPath + "/DataXML.text");
        if(File.Exists(Application.dataPath + "DataXML.text"))
        {
            Debug.Log("XML FILE SAVED");
        }

    }
    public void LoadSaveData()
    {

    }

    /// <summary>
    /// Create Data to save. 
    /// </summary>
    /// <returns></returns>
    Save CreateSaveData()
    {
        Save save = new Save();
        //Save position and rotation of the player 
        save.playerPositonX = player.transform.position.x;
        save.playerPositonY = player.transform.position.y;
        save.playerPositonZ = player.transform.position.z;
        save.playerRotationX = player.transform.rotation.x;
        save.playerRotationY = player.transform.rotation.y;
        save.playerRotationZ = player.transform.rotation.z;
        //Save Health
        save.playerHealth = stats.health;
        //Save Inventory Items
        foreach(var item in inventory.items)
        {
            save.items.Add(item);
        }
        foreach (var quickItem in inventory.quickItems)
        {
            save.quickItems.Add(quickItem);
        }

        //Save Enemy Information. 
        S_EnemyBase [] enemiesScript = enemies.GetComponentsInChildren<S_EnemyBase>();
        for(int i = 0; i < enemiesScript.Length;i++)
        {
            GameObject enemiesG = enemiesScript[i].gameObject;
            //Saving the rotation of each enemy. 
            save.enemyPositionX.Add(enemiesG.transform.position.x);
            save.enemyPositionY.Add(enemiesG.transform.position.y);
            save.enemyPositionZ.Add(enemiesG.transform.position.z);
            save.enemyStillAlive.Add(enemiesG.activeSelf);
        }

        return save;
    }
}
