using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    List<FieldObject> fieldObjects = new List<FieldObject>();

    bool AreFieldObjectsColliding(FieldObject object1, FieldObject object2) =>
        Vector3.Distance(object1.Position, object2.Position) <= (object1.Radius + object2.Radius);

    T GetCollisions<T>(FieldObject fieldObject, Func<IEnumerable<FieldObject>, T> collisionProcessor)
    {
        var collidingObjects = fieldObjects
            .Where(obj => obj != fieldObject && AreFieldObjectsColliding(fieldObject, obj)); // 자신을 제외하고 충돌하는 객체 선택

        return collisionProcessor(collidingObjects); // 처리 함수에 collidingObjects 전달
    }
    public List<FieldObject> GetCollidingFieldObjects(FieldObject fieldObject)
    {
        return GetCollisions(fieldObject, collidingObjects => collidingObjects.ToList()); // 리스트 반환
    }
    public FieldObject GetFirstCollidingFieldObject(FieldObject fieldObject)
    {
        return GetCollisions(fieldObject, collidingObjects => collidingObjects.FirstOrDefault()); // 첫 번째 충돌 객체 반환
    }
}
