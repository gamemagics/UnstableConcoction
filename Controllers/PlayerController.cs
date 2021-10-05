using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerStatus {
    IDLE,
    MOVE,
    REACT
}

public class PlayerController : MonoBehaviour {
    public ElementType element;

    private Vector2 destination;

    [SerializeField] private PlayerStatus status = PlayerStatus.IDLE;

    private Stack<ICommand> commandStack = new Stack<ICommand>();

    private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        switch (status) {
            case PlayerStatus.IDLE:
                GetInput();
                break;
            case PlayerStatus.MOVE:
                Move();
                break;
            case PlayerStatus.REACT:
                React();
                break;
        }

        SetColorMask();
    }

    private void GetInput() {
        if (Input.GetMouseButtonUp(0)) {
            Vector3 pos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            Vector2 target = MapManager.instance.Move(worldPos, transform.position);

            if (target.x <= -1000.0f || !MapManager.instance.Destroy(worldPos, element)) {
                return;
            }

            var playerPos = transform.position;
            MoveCommand command = new MoveCommand(transform, playerPos, target);
            commandStack.Push(command);

            if (animator != null) {
                if (target.x < playerPos.x && target.y < playerPos.y) {
                    animator.SetInteger("Forward", 2);
                }
                else if (target.x > playerPos.x && target.y < playerPos.y) {
                    animator.SetInteger("Forward", 1);
                }
                else if (target.x < playerPos.x && target.y > playerPos.y) {
                    animator.SetInteger("Forward", 0);
                }
                else {
                    animator.SetInteger("Forward", 3);
                }

                animator.ResetTrigger("Stop");
                animator.SetTrigger("Move");
            }

            MusicEventManager.musicEvent.Invoke("move");

            destination = target;
            status = PlayerStatus.MOVE;
        }
        else if (Input.GetKeyUp(KeyCode.Z)) {
            if (commandStack.Count > 0) {
                ICommand cmd = commandStack.Peek();
                cmd.Redo();
                commandStack.Pop();

                MapManager.instance.Redo();
                animator.SetTrigger("Stop");
            }
        }
        else if (Input.GetKeyUp(KeyCode.R)) {
            Booter.instance.Change(1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)){
            Booter.instance.Change(0);
        }
        else {
            animator.SetTrigger("Stop");
        }
    }

    private void Move() {
        Vector2 pos = transform.position;
        ICommand cmd = commandStack.Peek();

        if (pos != destination) {
            cmd.Execute();
        }
        else {
            if (MapManager.instance.Finish(pos)) {
                GameBooter.instance.passed = true;
                MusicEventManager.musicEvent.Invoke("success");
                StartCoroutine(StairwayToHeaven());

                transform.position = new Vector3(999, 999, 999);
            }

            status = PlayerStatus.REACT;
            if (animator != null) {
                animator.ResetTrigger("Move");
                animator.SetTrigger("Stop");
            }
        }
    }

    private IEnumerator StairwayToHeaven() {
        yield return new WaitForSeconds(1.0f);
        Booter.levelIndex++;
        Booter.instance.Change(1);
        yield return null;
    }

    private void React() {
        MoveCommand cmd = commandStack.Peek() as MoveCommand;
        
        MapManager.instance.Spill(cmd.From, element);
        ElementType newType = MapManager.instance.Collect(cmd.To, element);
        ReactCommand newCmd = new ReactCommand(this, element, newType);
        EffectManager.instance.Spill(new Vector3(cmd.From.x, cmd.From.y - 0.16f, 3), element);
        newCmd.Execute();
        cmd.secondCommand = newCmd;

        status = PlayerStatus.IDLE;
    }

    private void SetColorMask() {
        if (spriteRenderer == null) {
            return;
        }

        switch (element) {
            case ElementType.Arcane:
                spriteRenderer.color = new Color(0.0f / 255.0f, 185.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
                break;
            case ElementType.Blight:
                spriteRenderer.color = new Color(168.0f / 255.0f, 58.0f / 255.0f, 58.0f / 255.0f, 255.0f / 255.0f);
                break;
            case ElementType.Crystal:
                spriteRenderer.color = new Color(255.0f / 255.0f, 155.0f / 255.0f, 190.0f / 255.0f, 255.0f / 255.0f);
                break;
            case ElementType.Diffusal:
                spriteRenderer.color = new Color(170.0f / 255.0f, 90.0f / 255.0f, 205.0f / 255.0f, 255.0f / 255.0f);
                break;
            case ElementType.Essence:
                spriteRenderer.color = new Color(255.0f / 255.0f, 195.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
                break;
            case ElementType.Force:
            spriteRenderer.color = new Color(108.0f / 255.0f, 205.0f / 255.0f, 126.0f / 255.0f, 255.0f / 255.0f);
                break;
        }
    }
}
