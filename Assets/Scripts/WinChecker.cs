using System;

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
