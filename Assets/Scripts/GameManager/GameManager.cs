using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instant { get; set; }         //单例
    public GameObject[] players;           //玩家引用
    public ExternalBehaviorTree tree;               //行为树
     public static int aiNumber = 0;            //AI数量

    [HideInInspector]public Player[] playersScript;           //player脚本数组
    [HideInInspector] public Dictionary<int, GameObject> cellDic;           //存储所有格子的词典
    [HideInInspector] public int playerTurn;        //玩家顺序
    [HideInInspector] public int morePoint;        //事件额外点数
    [HideInInspector] public int round;         //轮数（每投一次骰子为1轮，1回合4轮）
    [HideInInspector] public int resetMove;         //停止后剩余移动点数
    [HideInInspector] public bool pause;            //暂停当前回合，供传送后继续移动使用

    //委托，用于投骰子之后检测是否执行
    public delegate void EventAfterDice(int dicePoint);
    public EventAfterDice eventAfterDice;

    //委托，用于每轮检测效果是否失效
    public delegate void DisactiveEffect();
    public DisactiveEffect disactiveEffect;

    //委托，用于每轮结束后清空一些效果
    public delegate void ClearAfterRound();
    public ClearAfterRound clearAfterRound;

    //组件
    public Button diceButton;
    public ExtraPointText extraPointText;
    public CurRoundPlayer CurRoundPlayer;
    public FirstPlayerText firstPlayerText;
    public Text winnerText;
    private Player player;
    private Text dicePointText;

    Rigidbody2D rb2d;           //当前玩家的Rigidbody2D组件
    Transform player_trf;       //当前玩家的Transfrom组件
    private int dicePoint;        //骰子点数
    private Dictionary<int, Transform> playerTrsfs;         //所有玩家的Transfrom组件
    private Dictionary<int, Rigidbody2D> playerRd2ds;           //所以玩家的Rigidbody2D组件
    private List<Player> playerRank;                  //存储玩家排名的列表
    private bool _GameOver;             //标识游戏结束

    //public int random;              //调试用值，用于控制骰子点数


    private void Awake()
    {
        if (instant == null)
            instant = this;
        else if (instant != this)
            Destroy(gameObject);

        Init();
    }

    private void Start()
    {
        dicePointText = diceButton.GetComponentInChildren<Text>();
        cellDic = cellDic.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

        //保存所有玩家的Transform组件和Rigidbody2D
        foreach (GameObject player in players)
        {
            Player playerScript = player.GetComponent<Player>();
            playerTrsfs.Add(playerScript.turn - 1, player.transform);
            playerRd2ds.Add(playerScript.turn - 1, player.GetComponent<Rigidbody2D>());
        }

        //添加AI行为树
        for (int i = 4 - aiNumber; i < 4; i++)
        {
            var bt = players[i].AddComponent<BehaviorTree>();
            GlobalVariables.Instance.SetVariableValue("PlayerTurn", i);
            bt.ExternalBehavior = tree;
            bt.StartWhenEnabled = false;
            bt.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.Space))
            StartGameLoop();
    }

    private void Init()
    {
        _GameOver = false;
        playersScript = new Player[4];
        cellDic = new Dictionary<int, GameObject>();
        playerTrsfs = new Dictionary<int, Transform>();
        playerRd2ds = new Dictionary<int, Rigidbody2D>();


        playerTurn = 0;
        morePoint = 0;
        round = 1;
        resetMove = 0;
        dicePoint = 0;
        pause = false;

        for (int i = 0; i < players.Length; i++)
            playersScript[i] = players[i].GetComponent<Player>();

        player = playersScript[0];
        CurRoundPlayer.UpdateText(player.playerText.text);
    }


    //注册方法，供格子脚本调用，用于将格子注册到词典中
    public void Register(int instantID, GameObject cellGmobj)
    {
        cellDic.Add(instantID, cellGmobj);
    }

    //点击骰子后调用
    public void StartGameLoop()
    {
        dicePoint = RollTheDice();
        eventAfterDice?.Invoke(dicePoint);
        StartCoroutine(GameLoop(dicePoint));
    }

    //行走指定点数，供其他方法调用
    public void StartGameLoop(int dicePoint)
    {
        StartCoroutine(GameLoop(dicePoint));
    }

    //投骰子
    private int RollTheDice()
    {
        diceButton.interactable = false;
        int random = Random.Range(1, 6);
        dicePointText.text = random.ToString();
        return random;
    }

    //控制游戏进程
    private IEnumerator GameLoop(int dicePoint)
    {
        //等待当前玩家移动完成
        if (!player.skipMove)
            yield return StartCoroutine(MoveThePlane(dicePoint));

        //判断是否游戏结束，或者移动暂停
        if (!_GameOver  && !pause)
        {
            //切换下一轮之后判断是否需要跳过该玩家回合（使用了高级道具）
            NextTurn();
            while (player.skipRound)
            {
                player.skipRound = false;
                NextTurn();
            }
            CurRoundPlayer.UpdateText(player.playerText.text);
        }
    }


    //当前回合结束，切换到下一位玩家
    private void NextTurn()
    {
        //轮数+1
        round++;
        //检查是否有效果失效
        disactiveEffect?.Invoke();
        dicePoint = 0;

        //更新排名第一的玩家
        Player firstPlayer = GetFirstPlayer();
        firstPlayerText.UpdateFirstPlayerText(firstPlayer.playerText.text);

        BehaviorTree behaviorTree;
        behaviorTree = player.GetComponent<BehaviorTree>();
        if (behaviorTree != null)
            behaviorTree.DisableBehavior();

        //清空当前玩家身上可清除的状态位
        player.ResetFlag();
        player.ChangeAllButtonState(false);
        clearAfterRound?.Invoke();

        //切换到下一位玩家
        playerTurn = (playerTurn + 1) % 4;
        player = playersScript[playerTurn];

        //player = players[playerTurn].GetComponent<Player>();
        player.ChangeAllButtonState(true);
        diceButton.interactable = true;
        extraPointText.ClearExtraPointText();

        behaviorTree = player.GetComponent<BehaviorTree>();
        if (behaviorTree != null)
        {
            GlobalVariables.Instance.SetVariableValue("PlayerTurn", playerTurn);
            behaviorTree.enabled = true;
            behaviorTree.EnableBehavior();
        }
    }

    //获取事件的额外点数
    private void GetMorePoint()
    {
        if(morePoint != 0)
        {
            if (player.extraPoint > 0)
                player.extraPoint += morePoint;
            else
                player.extraPoint -= morePoint;
        }
    }

    //综合额外点数，计算需要移动的步数
    private int CalculateMove(int point)
    {
        //显示额外点数
        if (player.extraPoint != 0)
        {
            GetMorePoint();
            //在骰子上显示额外点数
            extraPointText.ShowExtraPointText(player.extraPoint);
            //清空玩家上一轮点数显示
            player.extraPointText.ClearExtraPointText();
        }

        //  根据玩家身上的额外点数决定正向还是反向移动
        int move = point + player.extraPoint;

        //反向移动，修改玩家方向值
        if (move < 0)
        {
            move = Mathf.Abs(move);
            player.ReverseDir(true);
        }

        player.CheckNeedRotate();
        return move;
    }



    //移动棋子，调用Player脚本中的方法
    private IEnumerator MoveThePlane(int random)
    {
        Vector2 moveDir;
        rb2d = playerRd2ds[playerTurn];
        player_trf = playerTrsfs[playerTurn];

        //如果玩家发生了传送，传送之后继续移动时不需要重新计算移动步数
        int move;
        if (!player.isPortal)
            move = CalculateMove(random);
        else
            move = random;

        player.isMove = true;
        //每次移动一格
        for (int i = 0; i < move; i++)
        {
            //有玩家到达终点或遇到停止标识，停止移动
            if (_GameOver || player.stopMove)
            {
                //存储剩余步数，用于传送后继续移动
                resetMove = move - i;
                break;
            }

            if (player.isReverse)
                moveDir = -player_trf.up;
            else
                moveDir = player_trf.up;

            //进入旋转格，需要旋转以改变方向
            if (player.needRotate)
                yield return StartCoroutine(player.RotatehePlane(player.targetRotation));
            //正常行走
            else
            {
                Vector2 targetPos = rb2d.position + moveDir * 1f;
                yield return StartCoroutine(player.MoveThePlane(targetPos));
            }
        }

        player.isMove = false;

        //不需要继续移动
        if(!player.contiueMove)
        {
            //如果之前倒退移动，现在将玩家方向改回原始值
            if (player.isReverse)
                player.ReverseDir(false);

            //移动结束
            //获取颜色格内容
            player.GetCellContent();

        }

    }

    //关闭所有按钮互动状态
    private void TurnOffAllBtn()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Player player = players[i].GetComponent<Player>();
            player.ChangeAllButtonState(false);
        }
    }

    //游戏结束方法，供终点格调用
    public void GameOver()
    {
        _GameOver = true;
        StopCoroutine(GameLoop(0));
        diceButton.gameObject.SetActive(false);
        extraPointText.gameObject.SetActive(false);
        winnerText.gameObject.SetActive(true);
        TurnOffAllBtn();
        winnerText.text = "winner:" + player.playerText.text;
    }

    //返回以当前玩家为起始的Player，参数为偏移量
    public Player GetPlayer(int offset = 0)
    {
        //Player player = players[(playerTurn + offset) % 4].GetComponent<Player>();
        Player player = playersScript[(playerTurn + offset) % 4];
        return player;
    }

    //返回以当前玩家为起始的Player，参数为偏移量
    public Player GetSpecifiedPlayer(int turn)
    {
        //Player player = players[(playerTurn + offset) % 4].GetComponent<Player>();
        Player player = playersScript[turn];
        return player;
    }

    //计算目标玩家对应的行动轮数
    public int GetPlayerRound(int playerTurn)
    {
        int curPlayerTurn = playerTurn + 1;
        int gap = playerTurn - curPlayerTurn;
        return round + gap;
    }

    public Player GetFirstPlayer()
    {
        //对玩家数组进行升序排序
        playerRank = new List<Player>();
        foreach (GameObject item in players)
        {
            //如果玩家在起点，不计入排序
            Player player = item.GetComponent<Player>();
            if (player.curCellIndex == 0)
                continue;

            playerRank.Add(player);
        }

        playerRank.Sort((x, y) => x.CompareTo(y));
        return playerRank[0];
    }

    //查找是否有距离终点小于27且领先自身的玩家
    public bool HasPlayerCloseToFinal()
    {
        foreach (Player temPlayer in playersScript)
            if (temPlayer != player && temPlayer.distanceFromFinal < 27
                && temPlayer.distanceFromFinal < player.distanceFromFinal)
                return true;

        return false;
    }
}
