using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum State{
    CROSS,
    NULL,
    EMPTY
}

class GameObjectComp : IComparer<GameObject>{
    public int Compare(GameObject p1, GameObject p2){
        int p1Comp = Int32.Parse(p1.name.Substring(6,1));
        int p2Comp = Int32.Parse(p2.name.Substring(6,1));
        return (p1Comp - p2Comp);
    }
}

public class MakeMoveScript : MonoBehaviour, Observer
{
    [SerializeField] GameObject CrossPrefab;
    [SerializeField] GameObject NullPrefab;
    [SerializeField] Text text;
    public Field field = new Field();
    public CellCreator cellCreator = new CellCreator();
    public GameObject[] cells;
    public State gameState = State.CROSS;
    public static MakeMoveScript instance;
    public int moveCount = 0;

    private bool isOver = false;
    private float time = 0;

    private MakeMoveScript(){}
    
    public void changeGameState(){
        if(gameState == State.CROSS) gameState = State.NULL;
        else gameState = State.CROSS;
    }

    void Start()
    {
        instance = this;
        ClickReciver.obj.regObserver(instance);
        cells = GameObject.FindGameObjectsWithTag("Cell");
        Array.Sort(cells, new GameObjectComp());
        foreach (GameObject item in cells)
        {
            Cell temp = (Cell) cellCreator.create(item);
            field.setCell(temp);
        }
    }
    public void notifed(Cell moveCell){
        if(isOver) return;

        Transform instantiateCoords = moveCell.getObject().transform;
        GameObject move = null;
        //Вообще считается, что создавать объект - это крайне плохая практика, но я спорт прогер, мне пофиг
        if(moveCell.getState() == State.CROSS){
            move = Instantiate(CrossPrefab, instantiateCoords.position, Quaternion.identity);
        }else{
            move = Instantiate(NullPrefab, instantiateCoords.position, Quaternion.identity);
        }
        moveCount++;
        bool isWin = WinChecker.checkWin(field);
        if(moveCount != 9 || isWin) {
            if(isWin){
                if(gameState == State.CROSS) text.text = "Cross Win!!!";
                else{
                    text.text = "Null Win!!!";
                }
                isOver = true;
            }
        }
        else {
                text.text = "Draw";
                isOver = true;
        }
        changeGameState();
    }
    void Update(){
        if(isOver){
            time += Time.deltaTime;
            if(time > 1.5){
                SceneManager.LoadScene ("SampleScene");
            }
        }

        // Если случилось нажатие, то нужно создать в нужной клетке крестик/нолик, наверно плохая идея делать это каждый фрейм, лучше лиснер повешать, но я так не умею
        //Теперь умею хехе
    }
}
