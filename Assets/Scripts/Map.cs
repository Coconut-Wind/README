using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [Header("---- Prefabs ----")]
    [SerializeField] private GameObject nullCellPrefab; // 空点位的预制体
    [SerializeField] private GameObject normalCellPrefab; // 普通点位的预制体
    // TODO:还有增益点位和减益点位的预制体没创建
    [SerializeField] private GameObject edgePrefab; // 边的预制体


    [Header("---- cellMap参数 ----")]
    private GameObject[,] cellMap; // 整张点位地图，二维数组
    [SerializeField] private int cellMapRow;
    [SerializeField] private int cellMapColumn;


    [Header("---- 渲染地图的参数 ----")]
    [SerializeField] private float cellPadding = 1.0f; // 每个点位之间的间隔
    private float cellWidth; // 点位的宽
    private float cellHeight; // 点位的高
    private float offsetX; // 将地图挪到世界中心的X偏移量
    private float offsetY; // 将地图挪到世界中心的Y偏移量


    private CellNode[] cellAdjList; // 邻接表
    private string[] cellType = new string[4] { "NullCell", "NormalCell", "PosiCell", "NegaCell" }; // 点位的四种类型
    public TextAsset mapInfo; // 用于读取地图信息，即Data文件夹中的mapInfo，目前为手动挂载


    private void Awake()
    {
        ReadMapInfo();

        cellWidth = normalCellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        cellHeight = normalCellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        offsetX = (cellMapColumn - 1) * (cellWidth + cellPadding);
        offsetY = (cellMapRow - 1) * (cellHeight + cellPadding);
        cellMap = new GameObject[cellMapRow, cellMapColumn];
    }

    private void Start()
    {
        GenerateMap();
        GenerateEdge();

        test();
    }

    // 读取地图信息
    private void ReadMapInfo()
    {
        string[] infos = mapInfo.text.Split('\n'); // 将多行数据用换行符分隔
        for (int i = 0; i < infos.Length; i++)
        {
            // Debug.Log(infos[i]);
            string[] lineInfo = infos[i].Split(','); // 一行中的数据用逗号分隔
            if (i == 0) // 数据中的第一行记录地图的行和列
            {
                // string to int
                cellMapRow = int.Parse(lineInfo[0]);
                cellMapColumn = int.Parse(lineInfo[1]);

                cellAdjList = new CellNode[cellMapRow * cellMapColumn];
            }
            else // 第二行后的数据记录点位的类型和邻接的点位；第一个为点位类型，剩下的为与该点位邻接的点位在邻接表的下标
            {
                cellAdjList[i - 1] = new CellNode();
                cellAdjList[i - 1].cellType = lineInfo[0];
                if (cellAdjList[i - 1].cellType == "NullCell") // 如果点位类型是空点位
                {
                    cellAdjList[i - 1].firstEdge = null;
                }
                else // 如果点位类型是有效的点位
                {
                    // 初始化边表
                    for (int j = 1; j < lineInfo.Length; j++)
                    {
                        EdgeNode newEdge = new EdgeNode();
                        newEdge.adjCell = int.Parse(lineInfo[j]);
                        newEdge.nextEdge = cellAdjList[i - 1].firstEdge;
                        cellAdjList[i - 1].firstEdge = newEdge;
                    }
                }
            }
        }

    }

    // 生成地图
    private void GenerateMap()
    {
        for (int i = 0; i < cellMapRow; i++)
        {
            for (int j = 0; j < cellMapColumn; j++)
            {
                GameObject spawnedCell = null;

                // 调整点位生成的位置，坐标原点在左上角
                float positionX = 0; // 点位最终渲染的位置x分量
                float positionY = 0; // 点位最终渲染的位置y分量
                if (i != 0 && j != 0)
                {
                    positionX = j + j * cellPadding;
                    positionY = -(i + i * cellPadding);
                }
                else if (i == 0 && j != 0)
                {
                    positionX = j + j * cellPadding;
                    positionY = i;
                }
                else if (i != 0 && j == 0)
                {
                    positionX = j;
                    positionY = -(i + i * cellPadding);
                }
                else
                {
                    positionX = j;
                    positionY = i;
                }

                // 根据点位类型生成点位
                if (cellAdjList[i * cellMapColumn + j].cellType == "NullCell")
                {
                    spawnedCell = Instantiate(nullCellPrefab, new Vector2(positionX - offsetX / 2.0f, positionY + offsetY / 2.0f), Quaternion.identity);
                }
                else if (cellAdjList[i * cellMapColumn + j].cellType == "NormalCell")
                {
                    spawnedCell = Instantiate(normalCellPrefab, new Vector2(positionX - offsetX / 2.0f, positionY + offsetY / 2.0f), Quaternion.identity);
                }
                else if (cellAdjList[i * cellMapColumn + j].cellType == "PosiCell")
                {
                    spawnedCell = Instantiate(normalCellPrefab, new Vector2(positionX - offsetX / 2.0f, positionY + offsetY / 2.0f), Quaternion.identity);
                }
                else if (cellAdjList[i * cellMapColumn + j].cellType == "NegaCell")
                {
                    spawnedCell = Instantiate(normalCellPrefab, new Vector2(positionX - offsetX / 2.0f, positionY + offsetY / 2.0f), Quaternion.identity);
                }

                cellMap[i, j] = spawnedCell; // 将生成的点位存放在cellMap内

                // 以下对spawnedCell的属性进行初始化
                spawnedCell.transform.SetParent(transform); // 将生成的点位作为Map的子物体
                spawnedCell.name = $"cell {i} {j}";
                if (spawnedCell.GetComponent<Cell>()) // 如果生成的点位有Cell这个组件，即生成的点位是有效点位
                {
                    Cell cell = spawnedCell.GetComponent<Cell>(); // 获取到新生成有效点位的Cell脚本组件
                    cell.SetIndex(i, j);
                    cell.SetCellType(cellAdjList[i * cellMapColumn + j].cellType);
                }
            }
        }
    }

    // 生成整个地图点位的边
    private void GenerateEdge()
    {
        // 遍历cellMap
        for (int i = 0; i < cellMapRow; i++)
        {
            for (int j = 0; j < cellMapColumn; j++)
            {
                if (cellMap[i, j].GetComponent<Cell>()) // 如果cellMap[i, j]存储的是有效点位
                {
                    List<Cell> adjCellList = new List<Cell>(); // 该点位的邻接点数组
                    GameObject startCell = cellMap[i, j]; // 该点位，作为边的出发位置

                    EdgeNode pointer = cellAdjList[i * cellMapColumn + j].firstEdge; // 用于遍历
                    while (pointer != null)
                    {
                        int adjCellIndex = pointer.adjCell;
                        GameObject endCell = cellMap[adjCellIndex / cellMapColumn, adjCellIndex % cellMapColumn]; // 邻接点位，作为边的终点位置

                        var edge = Instantiate(edgePrefab, (startCell.transform.position + endCell.transform.position) / 2.0f, Quaternion.identity);
                        edge.name = $"edge {i * cellMapColumn + j} {adjCellIndex}";
                        edge.transform.SetParent(transform);

                        edge.GetComponent<Edge>().DrawLine(startCell.transform, endCell.transform); // 调用边的DrawLine函数使用LineRenderer画出一条线
                        adjCellList.Add(endCell.GetComponent<Cell>()); // 将邻接点位加入邻接点数组

                        pointer = pointer.nextEdge;
                    }
                    cellMap[i, j].GetComponent<Cell>().SetAdjCellList(adjCellList); // 设置该点位的邻接点数组
                }
            }

        }
    }

    // test
    private void test()
    {
        for (int i = 0; i < cellMapRow; i++)
        {
            for (int j = 0; j < cellMapColumn; j++)
            {
                if (cellMap[i, j].GetComponent<Cell>())
                {
                    Cell cell = cellMap[i, j].GetComponent<Cell>();
                    for (int k = 0; k < cell.GetAdjCellList().Count; k++)
                    {
                        Debug.Log(cell.GetAdjCellList()[k].name);
                    }

                    Debug.Log(cellMap[i, j].GetComponent<Cell>().GetCellType());
                    Debug.Log(cellMap[i, j].GetComponent<Cell>().GetIndex());
                }
            }
        }
    }

}

// 顶点表
public class CellNode
{
    // TODO：有想过使用Cell，后续看看需不需要改，改的话整个逻辑可能要大改
    public string cellType;
    public EdgeNode firstEdge;
}

// 边表
public class EdgeNode
{
    public int adjCell; // 邻接的点位在顶点表的下标
    public EdgeNode nextEdge;
}