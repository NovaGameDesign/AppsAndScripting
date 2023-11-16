using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        if(save.items.Count > 0)
        {
            foreach (var item in save.items)
            {
                XmlElement savedItem = xmlDoc.CreateElement("Item");
                savedItem.InnerText = item.name;
                root.AppendChild(savedItem);
            }
        }
        
        if(save.quickItems.Count > 0)
        {
            foreach (var quickItem in save.quickItems)
            {
                XmlElement savedQuickItem = xmlDoc.CreateElement("QuickItem");
                if (quickItem != null)
                {
                    savedQuickItem.InnerText = quickItem.name;
                }
                else
                    savedQuickItem.InnerText = "Null"; 
                
                root.AppendChild(savedQuickItem);
            }
        }

        XmlElement savedEnemy, enemyIsAlive, savedEnemyPositionX, savedEnemyPositionY, savedEnemyPositionZ, EnemyID;
        for (int i = 0; i < save.enemyStillAlive.Count; i++)
        {
            savedEnemy = xmlDoc.CreateElement("Enemy");
            EnemyID = xmlDoc.CreateElement("EnemyID");
            enemyIsAlive = xmlDoc.CreateElement("EnemyIsAlive");
            savedEnemyPositionX = xmlDoc.CreateElement("EnemyPositionX");
            savedEnemyPositionY = xmlDoc.CreateElement("EnemyPositionY");
            savedEnemyPositionZ = xmlDoc.CreateElement("EnemyPositionZ");

            EnemyID.InnerText = save.enemyId[i].ToString();
            savedEnemyPositionX.InnerText = save.enemyPositionX[i].ToString();
            savedEnemyPositionY.InnerText = save.enemyPositionY[i].ToString();
            savedEnemyPositionZ.InnerText = save.enemyPositionZ[i].ToString();
            enemyIsAlive.InnerText = save.enemyStillAlive[i].ToString();

            savedEnemy.AppendChild(savedEnemyPositionX);
            savedEnemy.AppendChild(savedEnemyPositionY);
            savedEnemy.AppendChild(savedEnemyPositionZ);
            savedEnemy.AppendChild(enemyIsAlive);
            savedEnemy.AppendChild(EnemyID);

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

    /// <summary>
    /// I"m just putting this comment here for later... 
    /// I'm not sure if I need to save whether the enemy is alive given I'm now usng an ID system. So I might be able to remove the bool check if the player is alive or not. 
    /// </summary>
    /// <returns></returns>
    public bool LoadSaveData()
    {
        if(File.Exists(Application.dataPath + "/DataXML.text"))
        {
            //Load the game. 
            Save save = new Save();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/DataXML.text");

            //Get the save file data from our previous save. 
            XmlNodeList health = xmlDocument.GetElementsByTagName("PlayerHealth");
            float playerHealth = float.Parse(health[0].InnerText);
            save.playerHealth = playerHealth;

            XmlNodeList playerPositionX = xmlDocument.GetElementsByTagName("PlayerPositionX");
            float playerPositionX1= float.Parse(playerPositionX[0].InnerText);
            save.playerPositonX = playerPositionX1;

            XmlNodeList playerPositionY = xmlDocument.GetElementsByTagName("PlayerPositionY");
            float playerPositionY1 = float.Parse(playerPositionY[0].InnerText);
            save.playerPositonY = playerPositionY1;

            XmlNodeList playerPositionZ = xmlDocument.GetElementsByTagName("PlayerPositionZ");
            float playerPositionZ1 = float.Parse(playerPositionZ[0].InnerText);
            save.playerPositonZ = playerPositionZ1;

            /// Functionally the quick item and item for loops are the same. The biggest reason we do quick items first is because both of them use .Find() which requires the gameoject to be active. 
            /// and because of that, we need to set the actual item itself inactive after.
          
            XmlNodeList playerQuickItems = xmlDocument.GetElementsByTagName("QuickItem");
            for (int i = 0; i < 4; i++)
            {
                string objectName = playerQuickItems[i].InnerText;
                //Debug.Log("Attempted to find an object with the name: "+objectName);
                GameObject obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    save.quickItems.AddLast(obj.GetComponent<ItemParent>());
                    //Debug.Log("Added a new item to our temp save inventory: " + obj.GetComponent<ItemParent>().name);
                }
                else
                {
                    obj = GameObject.Find("Player/Player Ui");
                    //Debug.Log(obj);
                    ItemParent temp = obj.GetComponent<ItemParent>();
                    Debug.Log(temp.name);
                    save.quickItems.AddLast(temp);
                }
            }

            XmlNodeList playerItems = xmlDocument.GetElementsByTagName("Item");              
            for(int i = 0; i < playerItems.Count; i++)
            {
                string objectName = playerItems[i].InnerText; 
                //Debug.Log("Attempted to find an object with the name: "+objectName);
                GameObject obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    save.items.AddLast(obj.GetComponent<ItemParent>());
                    //Debug.Log("Added a new item to our temp save inventory: " + obj.GetComponent<ItemParent>().name);
                    obj.SetActive(false);

                }
            }
            

            player.transform.position = new Vector3(save.playerPositonX, save.playerPositonY, save.playerPositonZ);
            stats.SetHealth(save.playerHealth);
            inventory.items = save.items;
            inventory.quickItems = save.quickItems;
            inventory.RefreshDisplayedItems(0);            
            inventory.UpdateItemDisplay(0);

            XmlNodeList enemy = xmlDocument.GetElementsByTagName("Enemy");   
            if(enemy.Count > 0)
            {
                for (int i = 0; i < enemy.Count; i++)
                {
                    XmlNodeList enemyID = xmlDocument.GetElementsByTagName("EnemyID");
                    int ID = int.Parse(enemyID[i].InnerText);
                    save.enemyId.Add(ID);

                    XmlNodeList enemyIsAlive = xmlDocument.GetElementsByTagName("EnemyIsAlive");
                    bool EnemyIsAlive = enemyIsAlive[i].InnerText == "true" ? true : false;
                    save.enemyStillAlive.Add(EnemyIsAlive);

                    XmlNodeList enemyPositionX = xmlDocument.GetElementsByTagName("EnemyPositionX");
                    float EnemyPositionX = float.Parse(enemyPositionX[i].InnerText);
                    save.enemyPositionX.Add(EnemyPositionX);

                    XmlNodeList enemyPositionY = xmlDocument.GetElementsByTagName("EnemyPositionY");
                    float EnemyPositionY = float.Parse(enemyPositionY[i].InnerText);
                    save.enemyPositionY.Add(EnemyPositionY);

                    XmlNodeList enemyPositionZ = xmlDocument.GetElementsByTagName("EnemyPositionZ");
                    float EnemyPositionZ = float.Parse(enemyPositionZ[i].InnerText);
                    save.enemyPositionZ.Add(EnemyPositionZ);
                }


            }
            S_EnemyBase[] enemiesScript = enemies.GetComponentsInChildren<S_EnemyBase>();
            for(int i = 0; i < enemiesScript.Length; i++)
            {
                bool enemyInSaveFile = false;
                Transform tempTransform = gameObject.transform;



                for (int j = 0; j < save.enemyId.Count; j++)
                {
                    if(enemiesScript[i].ReferenceId == save.enemyId[j])
                    {
                        enemyInSaveFile = true;
                        tempTransform.position = new Vector3(save.enemyPositionX[j], save.enemyPositionY[j], save.enemyPositionZ[j]);
                        break;
                    }
                }

                if (enemyInSaveFile)
                {
                    //If the ID is in the save file that means it is still alive. 
                    //Because we confirmed the enemy is alive, we can just update their position to the last saved location. 
                    enemiesScript[i].gameObject.transform.position = tempTransform.position;
                }
                else
                {
                    //Enemy is not in the save file which means they were destroyed/died at some point.
                    //We can just destroy the gameoject. 
                    Destroy(enemiesScript[i].gameObject);
                }
            }

            return true;
        }
        return false;
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
            save.items.AddLast(item);
        }
        foreach (var quickItem in inventory.quickItems)
        {
            save.quickItems.AddLast(quickItem);
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
            save.enemyId.Add(enemiesScript[i].ReferenceId);
            save.enemyStillAlive.Add(enemiesG.activeSelf);
        }

        return save;
    }
}
