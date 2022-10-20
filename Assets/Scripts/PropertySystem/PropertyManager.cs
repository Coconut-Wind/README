using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 道具系统管理类 </summary>
public class PropertyManager : MonoBehaviour
{
    public static PropertyManager instance;
    [Header("有关道具")]
    [SerializeField] private List<Property> propertyList; // 所有道具的列表
    [SerializeField] private List<GameObject> propertyPrefabsList; // 按照ID顺序，用于生成道具图标即道具本身
    [SerializeField] private List<Property> playerPropertyList;

    [Header("有关UI")]
    public Canvas propertyCanvas;
    public GameObject propertyPanel;
    public GameObject passivePropertyDetailPanel; // 被动道具详情面板
    public GameObject activePropertyDetailPanel; // 主动道具详情面板
    public GameObject currentClickProperty; // 当前点击的道具图标

    public bool isOpenedPanel = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        //passivePropertyDetailPanel =     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GenerateProperty(0);
        }
    }

    public void AddPlayerOwnedPropertyList(Property _property)
    {
        playerPropertyList.Add(_property);
    }

    public void RemovePlayerOwnedPropertyList(Property _property)
    {
        playerPropertyList.Remove(_property);
    }


    public List<Property> GetPlayerOwnedPropertyList()
    {
        return playerPropertyList;
    }

    // ---------------------------------------------

    /// <summary> 获得道具时应调用该方法，根据道具ID生成道具图标即道具本身</summary>
    public void GenerateProperty(int _id)
    {
        var spawnedProperty = Instantiate(propertyPrefabsList[_id]);
        spawnedProperty.transform.SetParent(propertyPanel.transform);
        spawnedProperty.transform.localScale = new Vector3(1,1,1);
    }

    public void RemoveAllPlayerProperty()
    {
         for (int i = 0; i < propertyPanel.transform.childCount; i++)
         {
            Destroy(propertyPanel.transform.GetChild(i).gameObject);
         }
    }

    public void ClosePropertyDetailPanel(GameObject _panel)
    {
        Debug.Log("A");
        _panel.SetActive(false);

        isOpenedPanel = false;
    }

    public void OnClickUsePropertyButton()
    {
        Property currentProperty = currentClickProperty.GetComponent<Property>();
        GameManager.instance.player.UseProperty(new UsePropertyEventArgs(currentProperty.propertyID));

        // 一次性
        if (currentProperty.isOneOff)
        {
            Destroy(currentClickProperty);
        }
    }

}