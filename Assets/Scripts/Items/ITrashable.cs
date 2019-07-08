using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrashable {
    bool IsTrash();
    void SetTrash(bool isTrash);
}
