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

public abstract class ICell{
    protected State state = State.EMPTY;
    protected int number;
}

//Фабричный метод, честно хз зачем он, просто повыпендриваться, что я типо паттерны знаю
public abstract class Creator{
    public abstract ICell create(GameObject obj);
}

public class CellCreator : Creator{
    public override ICell create(GameObject obj){return new Cell(obj);}
}

public class Cell : ICell{
    private GameObject gameObject; // gameObject wich associate with cell

    public Cell(GameObject obj){
        gameObject = obj;
    }

    public GameObject getObject(){
        return gameObject;
    }
    public State getState(){
        return state;
    }

    private Cell setState(State state_){
        state = state_;
        return this;
    }

    public Cell changeState(Cell cell, State state){
        if(cell.getState() != State.EMPTY) return cell;
        else return cell.setState(state);
    }

    public void setNumber(int number_){
        number = number_; // пока предпологаю, что ячейки можно перенумеровать.
    }
}

public class Field{
    private Cell[,] cells;// 3x3 matrix
    private int size_ = 3; 
    private int counterRow = 0;
    private int counterCol = 0;
    // next you can see magic ))
    public int size{
        get{
            return size_;
        }
        set{}
    }

    public Field(){
        cells = new Cell[size_ , size_];
    }

    public void setCell(Cell cell){
        if(counterRow == size_){
            counterRow = 0;
            counterCol++;
        }
        cells[counterCol, counterRow] = cell;
        cell.setNumber(counterRow);
        counterRow++;
    }

    public Cell this[int i, int j]{
        get{return cells[i, j];}
        set{cells[i, j] = value;}
    }
}

public class WinChecker{
    public static bool checkWin(Field field){
        int n = field.size;
        //Проверяем строки
        int counter = 0;
        for(int row = 0;row < n;row++){
            counter = 0;
            for(int i = 0;i < n;i++){
                if(field[row, i].getState() == State.CROSS) counter++;
                if(field[row, i].getState() == State.NULL) counter--;
                if(field[row, i].getState() == State.EMPTY) break;
            }
            if(Math.Abs(counter) == n) return true;
        }

        //Проверяем столбцы
        counter = 0;
        for(int col = 0;col < n;col++){
            counter = 0;
            for(int i = 0;i < n;i++){
                if(field[i, col].getState() == State.CROSS) counter++;
                if(field[i, col].getState() == State.NULL) counter--;
                if(field[i, col].getState() == State.EMPTY) break;
            }
            if(Math.Abs(counter) == n) return true;
        }

        //Проверяем главную диагональ
        counter = 0;
        for(int i = 0;i < n;i++){
            if(field[i, i].getState() == State.CROSS) counter++;
            if(field[i, i].getState() == State.NULL) counter--;
            if(field[i, i].getState() == State.EMPTY) break;
        }
        if(Math.Abs(counter) == n) return true;

        //Проверяем побочную диагональ
        counter = 0;
        for(int i = n - 1, j = 0;i >= 0;i--, j++){
            if(field[j, i].getState() == State.CROSS) counter++;
            if(field[j, i].getState() == State.NULL) counter--;
            if(field[j, i].getState() == State.EMPTY) break;
        }
        if(Math.Abs(counter) == n) return true;

        return false;
    }
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
