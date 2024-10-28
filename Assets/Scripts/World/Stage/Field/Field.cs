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
            .Where(obj => obj != fieldObject && AreFieldObjectsColliding(fieldObject, obj)); // �ڽ��� �����ϰ� �浹�ϴ� ��ü ����

        return collisionProcessor(collidingObjects); // ó�� �Լ��� collidingObjects ����
    }
    public List<FieldObject> GetCollidingFieldObjects(FieldObject fieldObject)
    {
        return GetCollisions(fieldObject, collidingObjects => collidingObjects.ToList()); // ����Ʈ ��ȯ
    }
    public FieldObject GetFirstCollidingFieldObject(FieldObject fieldObject)
    {
        return GetCollisions(fieldObject, collidingObjects => collidingObjects.FirstOrDefault()); // ù ��° �浹 ��ü ��ȯ
    }
}
