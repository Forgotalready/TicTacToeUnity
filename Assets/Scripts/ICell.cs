using UnityEngine;

public abstract class ICell{
    protected State state = State.EMPTY;
    protected int number_;

    public int number{
        get{return number_;}
        set{number_ = value;}
    }
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
}