using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand {
    private Vector2 from, to;
    private Transform transform;

    private const float speed = 0.8f;

    public ICommand secondCommand;

    public MoveCommand(Transform trans, Vector2 f, Vector2 t) {
        transform = trans;
        from = f; to = t;
    }

    public void Execute() {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(to.x, to.y, transform.position.z), speed * Time.deltaTime);
    }

    public void Redo() {
        transform.position = new Vector3(from.x, from.y, transform.position.z);

        if (secondCommand != null) {
            secondCommand.Redo();
        }
    }

    public Vector2 From {
        get {
            return from;
        }
    }

    public Vector2 To {
        get {
            return to;
        }
    }
}
