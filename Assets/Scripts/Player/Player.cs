using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour,IComparable<Player>
{
    public int moveDir;                                 //玩家移动方向    1为上 2 为下 3为左 4为右
    public static float moveTime;
    public static float rotateTime; 

    public GameObject player_PropPanel;         //玩家对应道具栏
    public GameObject final;            //玩家对应终点
    public Text playerText;                 //玩家名称
    public ExtraPointText extraPointText;           //额外点数Text

    public int turn;
    public int curCellIndex;                               //所在格子ID

    [HideInInspector]public bool needRotate;         //标识是否需要旋转
    [HideInInspector] public bool isClockWide;         //标识旋转方向是否为顺时针
    [HideInInspector] public bool isReverse;            //标识是否倒退移动
    [HideInInspector] public bool isTurnAround;            //标识朝向是否为反向

    [HideInInspector] public bool skipRound;            //标识是否跳过当前回合
    [HideInInspector] public bool skipMove;            //标识是否跳过移动环节
    [HideInInspector] public bool isMove ;            //标识是否正在移动
    [HideInInspector] public bool stopMove;            //标识是否需要终止移动
    [HideInInspector] public bool contiueMove;            //标识是否需要继续移动
    [HideInInspector] public bool backToFinal ;            //标识是否倒退到终点
    [HideInInspector] public bool passFinal;          //标识是否反向越过终点

    [HideInInspector] public Quaternion targetRotation;       //旋转角度
    [HideInInspector] public int extraPoint;                        //由棋盘格获得的额外点数
    [HideInInspector] public int distanceFromFinal;                   //离终点距离

    //道具词典，记录对应道具数量
    [HideInInspector] public Dictionary<string, int> props;

    private int finalIndex;     //终点ID
    private int[] dirction;         //以顺时针方向定义的方向数组，顺时针旋转时索引+1，逆时针-1则为新方向值
    private float inverseMoveTime;          //moveTime的倒数，方便计算
    private float inverseRotateTime;


    //需要用到的组件
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //初始数据，供返回起点使用
    private int originIndex;
    private int originMoveDir;
    private Vector3 originPosition;
    private Quaternion originRotation;


    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        finalIndex = final.GetComponent<FinalCell>().index;
        inverseMoveTime = 1 / moveTime;
        inverseRotateTime = 1 / rotateTime;

        //记录初始数据，供返回起点使用
        originIndex = curCellIndex;
        originMoveDir = moveDir;
        originPosition = transform.position;
        originRotation = transform.rotation;
    }


    private void Init()
    {
        props = new Dictionary<string, int>()
        {
            ["EffectPass"] = 10,
            ["StopMove"] = 10,
            ["CheatDice"] = 10,
            ["Portal"] = 10,
            ["TurnAround"] = 10,
        };
        dirction = new int[4] { 1, 4, 2, 3 };

        needRotate = false;         
        isClockWide = false;         
        isReverse = false;            
        isTurnAround = false;            
        skipRound = false;            
        skipMove = false;          
        isMove = false;           
        stopMove = false;            
        contiueMove = false;            
        backToFinal = false;            
        passFinal = false;

        extraPoint = 0;
        moveTime = 0.1f;
        rotateTime = 0.01f;
}

    //移动棋子方法
    public IEnumerator MoveThePlane(Vector2 targetPos)
    {
        float sqrRemainingDistance = (rb2d.position - targetPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            //使用MoveTowards进行平滑移动
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, targetPos, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (rb2d.position - targetPos).sqrMagnitude;
            yield return null;
        }
    }

    //旋转棋子方法
    public IEnumerator RotatehePlane(Quaternion targetRotation)
    {
        Vector2 moveDirection;
        if (isReverse)
            moveDirection = -transform.up;
        else
            moveDirection = transform.up;

        //先移动0.5单位
        Vector2 offset = rb2d.position + moveDirection * 0.5f;
        yield return StartCoroutine(MoveThePlane(offset));

        //利用Slerp插值让物体进行平滑旋转
        float angle = Quaternion.Angle(targetRotation, transform.rotation);
        while (angle > float.Epsilon)
        {
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * inverseRotateTime);
            transform.rotation = newRotation;
            angle = Quaternion.Angle(targetRotation, transform.rotation);
            yield return null;
        }

        //旋转后，改变棋子方向
        ChangeDirAfterRotate(isClockWide);

        //是否反向移动
        if (isReverse)
            moveDirection = -transform.up;
        else
            moveDirection = transform.up;

        //转向后需再移动一个偏移值offsetDistance
        //如果下一个格子为三角形，offsetDistance=0.5f
        //否则offsetDistance=1.5f
        RaycastHit2D hit = DoLinecast(8, moveDirection,false);
        float offsetDistance = 0f;
        if (hit.transform != null)
        {
            GameObject cellObj = hit.transform.gameObject;
            Cell tempCell = Utility.GetCellScriptByTag(cellObj);

            //检测当前格子是否为随机格，如果是，则下一个不会是三角格
            if (tempCell.tag == "RandomCell")
                offsetDistance = 1.5f;
            else
                offsetDistance = 0.5f;
        }
        offset = rb2d.position + moveDirection * offsetDistance;
        yield return StartCoroutine(MoveThePlane(offset));
        needRotate = false;
    }


    //获取颜色格内容方法，在移动结束后调用
    public void GetCellContent()
    {
        extraPoint = 0;
        //做射线检测（忽略Player层），获取当前格子引用
        RaycastHit2D hit  = DoLinecast(11, transform.up,true);

        if (hit.transform != null)
        {
            //获取当前格子ID和tag
            GameObject currentCell = hit.transform.gameObject;
            string cellTag = currentCell.tag;
            Cell cell = Utility.GetCellScriptByTag(currentCell);
            curCellIndex = cell.index;

            //计算到终点距离
            CalculateDistacnFromFinal();

            //停留点不是终点格，取消backToFinal
            if (!Utility.IsTypeOf<FinalCell>(cell))
                backToFinal = false;
            //否则，与终点距离+52
            else
                distanceFromFinal += 52;

            //停留在普通格，获取点数
            if (Utility.IsTypeOf<NormalCell>(cell))
            {
                NormalCell normalCell = currentCell.GetComponent<NormalCell>();

                if (normalCell != null)
                    UpdateExtraPoint(normalCell.extraPoint);
            }
            //停留在道具格，获取道具
            else if (Utility.IsTypeOf<Cell>(cell))
            {
                extraPointText.ClearExtraPointText();
                UpdatePropAmount(cellTag, 1);
            }
            //停留在三角格
            else if(Utility.IsTypeOf<TriCell>(cell))
            {
                //如果当前方向不是三角格旋转方向，取消needRotate
                //用于处理倒退恰好停在三角格情况
                TriCell triCell = currentCell.GetComponent<TriCell>();
                if (moveDir != triCell.clockwideDir &&
                    moveDir != triCell.antiClockwideDir)
                    needRotate = false;

                //执行三角格上对应的脚本
                DoTriCellFunc(currentCell);
            }

            if (currentCell.transform.childCount >0)
                StartCoroutine(GetCellEffect(currentCell.transform.GetChild(0).gameObject));
        }
    }

    //执行三角格上对应的脚本
    private void DoTriCellFunc(GameObject triCell)
    {
        if (triCell.tag == "RandomCell")
        {
            RandomCell randomCell = triCell.GetComponent<RandomCell>();
            randomCell.GetRandom(this);
        }
        else if (triCell.tag == "PropCell")
        {
            PropCell proCell = triCell.GetComponent<PropCell>();
            proCell.GetProp(this);
        }
        else if (triCell.tag == "EventCell")
        {
            EventCell eventCell = triCell.GetComponent<EventCell>();
            eventCell.ExecuteEvent();
        }
    }

    //返回玩家当前所在格子
    public Cell GetCurCell()
    {
        RaycastHit2D hit2D = DoLinecast(11, transform.up, true);
        Cell cell = null ;
        if (hit2D.transform != null)
            cell =  Utility.GetCellScriptByTag(hit2D.transform.gameObject);
        return cell;

    }

    //更新额外点数及其对应Text
    public void UpdateExtraPoint(int point)
    {
        extraPoint = point;
        extraPointText.ShowExtraPointText(extraPoint);
    }

    //做射线检测，可设置检测或忽略某层，获取当前格子引用
    private RaycastHit2D DoLinecast(int layer, Vector3 dir, bool ignore)
    {
        Vector2 start = transform.position;
        Vector2 end = transform.position + dir * 0.5f;
        boxCollider2d.enabled = false;

        int layerMask;
        if (!ignore)
            layerMask = 1 << layer;
        else
            layerMask = ~(1 << layer);

        RaycastHit2D hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider2d.enabled = true;

        return hit;
    }

    //旋转后，根据旋转方向更新玩家当前方向值
    private void ChangeDirAfterRotate(bool clockwide)
    {
        int i;
        for (i = 0; i < dirction.Length; i++)
            if (dirction[i] == moveDir)
                break;

        //为实现循环取方向，索引值+4再取余
        if (clockwide)
            moveDir = dirction[(i + 5) % 4];
        else
            moveDir = dirction[(i + 3) % 4];
    }

    //使棋子移动方向变为反向
    public void ReverseDir(bool needReverse)
    {
        int i;
        for (i = 0; i < dirction.Length; i++)
            if (dirction[i] == moveDir)
                break;

        //当前方向+2%4即为反向
        moveDir = dirction[(i + 2) % 4];
        isReverse = needReverse;
    }

    //获取某个方向的反向
    public int GetReverseDir(int originDir)
    {
        int i;
        for (i = 0; i < dirction.Length; i++)
            if (dirction[i] == originDir)
                break;
        originDir = dirction[(i + 2) % 4];
        return originDir;
    }

    //移动前判断是否在三角格内，如果是判断是否需要旋转
    public void CheckNeedRotate()
    {
        Cell curCell = GetCurCell();
        if (Utility.IsTypeOf<TriCell>(curCell))
            curCell.GetComponent<TriCell>().GetRotation(this,transform);
    }

    //计算到终点的距离
    public void CalculateDistacnFromFinal()
    {
        distanceFromFinal = finalIndex - curCellIndex;
        if (distanceFromFinal < 0)
            distanceFromFinal += 52;

        //如果越过终点，距离需要+52
        Cell cell = GetCurCell();
        if(passFinal)
            distanceFromFinal += 52;
    }

    //传送方法，供传送道具使用
    public IEnumerator Portal(bool needDice)
    {
        //等待玩家停止移动
        needRotate = false;
        stopMove = true;
        contiueMove = true;
        GameManager.instant.pause = true;
        while (isMove)
            yield return null;

        //获取剩余步数
        int resetMove = GameManager.instant.resetMove;

        //播放缩小动画
        animator.SetTrigger("StartProtal");
        yield return new WaitForSeconds(1f);

        //获取除终点外的随机格子
        Dictionary<int, GameObject> cells = GameManager.instant.cellDic;
        GameObject cell;
       while (true)
        {
            int random = UnityEngine.Random.Range(1, cells.Count + 1);
            //int random = 48;
            cell = cells[random];
            FinalCell finalCell = cells[random].GetComponent<FinalCell>();
            if (finalCell == null || finalCell.playerPlane != gameObject)
                break;
        }
        Cell cellScript = Utility.GetCellScriptByTag(cell);

        //传送，并修改朝向
        transform.position = cell.transform.position;
        ChangeDirAfterPortal(cellScript);

        //修改玩家所在的ID
        curCellIndex = cellScript.index;

        //如果是三角格，需要修正位置
        if (Utility.IsTypeOf<TriCell>(cellScript))
        {
            //后一个为三角格，前一个为普通格，向前补0.5
            //否则后补0.5
            bool need = Utility.IsSpcTri(curCellIndex);
            if (need)
                transform.position += transform.up * 0.5f;
            else
                transform.position -= transform.up * 0.5f;
        }

        //播放放大动画
        animator.SetTrigger("FinishProtal");
        yield return new WaitForSeconds(1f);

        //继续移动
        stopMove = false;
        GameManager.instant.pause = false;
        contiueMove = false;

        //传送后不需要再投骰子
        if(!needDice)
            GameManager.instant.StartGameLoop(resetMove);
    }

    //传送后改变朝向
    private void ChangeDirAfterPortal(Cell curCell)
    {
        moveDir = curCell.moveDir;
        //如果反向进入传送门，则移动方向取目标格子的反向
        if (isReverse)
            ReverseDir(true);
        else if (isTurnAround)
            ReverseDir(false);

        //如果不是反向移动进入传送道具，根据目标格子移动方向调整朝向
        if (!isTurnAround)
            ChangeOrientation(curCell.moveDir);
        //否则，取目标格子移动方向的反向为新的朝向
        else
        {
            int reverseMoveDir = GetReverseDir(curCell.moveDir);
            ChangeOrientation(reverseMoveDir);
        }
    }


    //改变朝向方法
    public void ChangeOrientation(int dir)
    {
        switch (dir)
        {
            case 1:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    //回到起点，供炸弹功能使用
    public void ReturnToOrigin()
    {
        //transform.position = originPosition;
        StartCoroutine(MoveThePlane(originPosition));
        transform.rotation = originRotation;
        moveDir = originMoveDir;
        curCellIndex = originIndex;

        needRotate = false;
        passFinal = false;

        extraPoint = 0;
        extraPointText.ClearExtraPointText();
    }


    //获取格子内容，并飞入对应的UI框中
    private IEnumerator GetCellEffect(GameObject cellContent)
    {
        //将生成的格子内容转换为屏幕坐标
        GameObject content =  Instantiate(cellContent, transform.position, Quaternion.identity);
        Transform contentTrsf = content.transform;
        Vector3 contentScreenPos = Camera.main.WorldToScreenPoint(contentTrsf.position);

        //玩家道具框transform.position即为屏幕坐标？？？
        Vector3 targetPos = player_PropPanel.transform.position;

        float sqrRemainingDistance = (contentScreenPos - targetPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            //使用MoveTowards进行平滑移动
            Vector3 newPosition = Vector3.MoveTowards(contentScreenPos, targetPos,10f );
            contentScreenPos = newPosition;
            //修改生成的格子内容世界坐标，从屏幕坐标转换
            contentTrsf.position = Camera.main.ScreenToWorldPoint(newPosition);
            sqrRemainingDistance = (newPosition - targetPos).sqrMagnitude;
            yield return null;
        }
        //飞行完成，销毁
        Destroy(content);
    }

    //修改道具数量，同时修改对应UI
    public void UpdatePropAmount(string propName,int amount)
    {
        props[propName] += amount;
        PropsPanel propsPanel = player_PropPanel.GetComponent<PropsPanel>();
        propsPanel.UpdatePropAmount(propName, props[propName]);
    }


    //改变按钮可互动状态方法
    public void ChangeButtonState(bool state)
    {
        Button[] buttons = player_PropPanel.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = state;
    }

    //重置可清除状态位方法
    public void ResetFlag()
    {
        skipMove = false;
        isReverse = false;
        stopMove = false;
    }

    //排序接口，以distanceFromFinal为标准
    public int CompareTo(Player other)
    {
        return distanceFromFinal.CompareTo(other.distanceFromFinal);
    }
}
