using UnityEngine;
using UnityEngine.EventSystems;

public class ClickReciver : MonoBehaviour, IPointerClickHandler
{
    public Cell clickedObject = null;
    public static ClickReciver obj = null;

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
                        obj = this;
                    }
                    stopFlag = true;
                    break;
                }
                if(stopFlag) break;
            }
        }
    }
}
