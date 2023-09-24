using System.Collections;
using System.Collections.Generic;

public interface Subject
{
    void regObserver(Observer o);
    void delObserver(Observer o);
    void notife();
}
