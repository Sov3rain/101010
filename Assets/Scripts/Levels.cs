using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour
{
    static public Levels instance;

    public Object[] blocsPrefab;
    public int currentLevel;    

    // Hashtable
    Dictionary<Blocs.BLOC_TYPE, Object> blocsHash;
    Dictionary<char, Blocs.BLOC_TYPE> blocsTypeHash;

    [HideInInspector]
    public bool loadingInProgress = false;

    void Awake()
    {
        instance = this;
        InitHashtable();
    }

    void InitHashtable()
    {
        blocsHash = new Dictionary<Blocs.BLOC_TYPE, Object>();
        blocsTypeHash = new Dictionary<char, Blocs.BLOC_TYPE>();

        foreach (Object blocPrefab in blocsPrefab)
        {
            if (blocPrefab is GameObject)
            {
                Blocs blocScript = ((GameObject)blocPrefab).GetComponent<Blocs>();
                if (blocScript != null)
                {
                    blocsHash.Add(blocScript.type, blocPrefab);
                    blocsTypeHash.Add(blocScript.blocChar[0], blocScript.type);
                }
            }
        }
    }

    public void LoadLevel(int numLevel)
    {
        Time.timeScale = 1.0f;
        currentLevel = numLevel;
        loadingInProgress = true;

        TextAsset txt = (TextAsset)Resources.Load("level" + numLevel.ToString("00"), typeof(TextAsset));
        CreateLevel(txt.text);
    }

    void CreateLevel(string levelString)
    {
        int x = 0;
        int y = 0;

        foreach (char c in levelString)
        {
            if (c == '\n')
            {
                y--;
                x = 0;
                continue;
            }

            // intancier tete
            if (c == 'T')
            {
                //GameObject head = Instantiate(Manager.instance.headPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                GameObject head = Instantiate(Manager.instance.headPrefab, new Vector3(x, y, 0), Manager.instance.headPrefab.transform.rotation) as GameObject;

                head.transform.parent = this.transform;                
            }

            // instancier corps
            if (c == 'C')
            {
                //GameObject body = Instantiate(Manager.instance.bodyPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                GameObject body = Instantiate(Manager.instance.bodyPrefab, new Vector3(x, y, 0), Manager.instance.bodyPrefab.transform.rotation) as GameObject;

                body.transform.parent = this.transform;
            }

            if (blocsTypeHash.ContainsKey(c))
            {
                Blocs.BLOC_TYPE blocType = blocsTypeHash[c];
                GameObject blocPrefab = blocsHash[blocType] as GameObject;

                GameObject newBloc = Instantiate(blocPrefab, new Vector3(x, y, 0), blocPrefab.transform.rotation) as GameObject;
                newBloc.transform.parent = this.transform;
            }
            x++;
        }
        loadingInProgress = false;
        Manager.instance.state = Manager.STATES.PLAY;
    }
}
