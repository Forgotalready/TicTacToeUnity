using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class ClickReciver : MonoBehaviour, IPointerClickHandler, Subject
{
    private static List<Observer> obsList = new List<Observer> {};
    public Cell clickedObject = null;
    public static ClickReciver obj = null;

    public void regObserver(Observer o){
        obsList.Add(o);
    }
    public void delObserver(Observer o){
        obsList.Remove(o);
    }

    public void notife(){
        foreach(Observer o in obsList){
            o.notifed(clickedObject);
        }
    }
    void Start(){
        obj = this;
    }

    public void OnPointerClick(PointerEventData eventData){
        GameObject pressed = eventData.pointerPress;
        int n = MakeMoveScript.instance.field.size;
        bool stopFlag = false;
        for(int i = 0;i < n;i++){
            for(int j = 0;j < n;j++){
                if(MakeMoveScript.instance.field[i, j].getObject().name == pressed.name){
                    if(MakeMoveScript.instance.field[i, j].getState() == State.EMPTY){
                        MakeMoveScript.instance.field[i, j] = MakeMoveScript.instance.field[i, j].changeState(MakeMoveScript.instance.field[i, j], MakeMoveScript.instance.gameState);
                        clickedObject = MakeMoveScript.instance.field[i, j];
                        notife();
                    }
                    stopFlag = true;
                    break;
                }
                if(stopFlag) break;
            }
        }
    }
}
