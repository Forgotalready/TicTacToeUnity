using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
    private Cell[,] cells;
    private int size_ = 3; // 3x3 matrix
    private int counterRow = 0;
    private int counterCol = 0;

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

public class MakeMoveScript : MonoBehaviour
{
    [SerializeField] GameObject CrossPrefab;
    [SerializeField] GameObject NullPrefab;
    public Field field = new Field();
    public CellCreator cellCreator = new CellCreator();
    public GameObject[] cells;
    public State gameState = State.CROSS;
    public static MakeMoveScript instance;
    public int moveCount = 0;
    public void changeGameState(){
        if(gameState == State.CROSS) gameState = State.NULL;
        else gameState = State.CROSS;
    }
    
    void Start()
    {
        instance = this;
        cells = GameObject.FindGameObjectsWithTag("Cell");
        Array.Sort(cells, new GameObjectComp());
        foreach (GameObject item in cells)
        {
            Cell temp = (Cell) cellCreator.create(item);
            field.setCell(temp);
        }
    }
    //Function that called every frame
    void Update(){
        // Если случилось нажатие, то нужно создать в нужной клетке крестик/нолик, наверно плохая идея делать это каждый фрейм, лучше лиснер повещать, но я так не умею
        if(ClickReciver.obj != null)
        {
            Cell moveCell = ClickReciver.obj.clickedObject;
            Transform instantiateCoords = moveCell.getObject().transform;
            GameObject move = null;
            //Вообще считается, что создавать объект - это крайне плохая практика, но я спорт прогер, мне пофиг
            if(moveCell.getState() == State.CROSS){
                move = Instantiate(CrossPrefab, instantiateCoords.position, Quaternion.identity);
            }else{
                move = Instantiate(NullPrefab, instantiateCoords.position, Quaternion.identity);
            }

            //move.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            MakeMoveScript.instance.changeGameState();
            ClickReciver.obj = null;
            moveCount++;
            if(moveCount == 9) Pause.point.isOver = true;
            else Pause.point.isOver = WinChecker.checkWin(field);
        }
    }
}
