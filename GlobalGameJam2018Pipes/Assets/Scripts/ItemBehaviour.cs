using System.Collections;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public float floatSpeed;
    [SerializeField] public int Row;
    [SerializeField] public int Column;
    private LastStep lastStep;
    private PlayBoard playBoard;
    private bool isMoving;
    private float originalPitch;

    private AudioSource audioSource;

    public ColoredMaterial material;
    public AudioClip trashClip;
    public AudioClip dropClip;
    public float pitchRange;


    // Use this for initialization
    void Start()
    {
        SetMoving(false);

        GameObject playBoardObject = GameObject.Find("PlayBoard(Clone)");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        lastStep = LastStep.Right;

        var particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startColor = MixerScript.ConvertMaterialColor(material.Color);
        }
        else
        {
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = MixerScript.ConvertMaterialColor(material.Color);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        originalPitch = audioSource.pitch;
        audioSource.pitch = UnityEngine.Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
    }

    private IEnumerator MoveItem(FlowDirection direction)
    {
        SetMoving(true);

        while (CanContinueMoving(direction))
        {
            UpdateCurrentTile(direction);

            yield return MoveToCurrentTile();

            direction = DetermineNextFlowDirection();
        }

        SetMoving(false);

        yield return ApplyFinalAction(direction);
    }

    private void SetMoving(bool isMoving)
    {
        if (this.isMoving)
        {
            if (!isMoving)
            {
                // we stop moving - stop playing sound
                audioSource.Pause();
            }
        }
        else
        {
            if (isMoving)
            {
                // we start moving - start to play sound
                audioSource.Play();
            }
        }
        this.isMoving = isMoving;
    }

    private void UpdateCurrentTile(FlowDirection nextDirection)
    {
        switch (nextDirection)
        {
            case FlowDirection.ToLeft:
                --Column;
                lastStep = LastStep.Left;
                break;
            case FlowDirection.ToTop:
                ++Row;
                lastStep = LastStep.Up;
                break;
            case FlowDirection.ToRight:
                ++Column;
                lastStep = LastStep.Right;
                break;
            case FlowDirection.ToDown:
                --Row;
                lastStep = LastStep.Down;
                break;
        }
    }

    private IEnumerator MoveToCurrentTile()
    {
        var originPosition = this.transform.position;
        var targetPosition = new Vector3(
            playBoard.GetXPosition(Column),
            originPosition.y,
            playBoard.GetZPosition(Row));

        var timePassed = 0.0f;
        var requiredTime = 1 / floatSpeed;

        do
        {
            timePassed += Time.deltaTime;
            this.transform.position = Vector3.Lerp(originPosition, targetPosition, timePassed / requiredTime);
            yield return null;
        }
        while (timePassed < requiredTime);
    }

    private FlowDirection DetermineNextFlowDirection()
    {
        var currentTile = playBoard.GetTileForPosition(Column, Row);

        if (currentTile == null)
        {
            if (!FindSink())
            {
                return FlowDirection.Drop;
            }
            else
            {
                return FlowDirection.Stop;
            }

        }

        if (currentTile.pipe == null)
        {
            return FlowDirection.Drop;
        }

        switch (lastStep)
        {
            case LastStep.Down:
                return currentTile.pipe.FromTop;
            case LastStep.Left:
                return currentTile.pipe.FromRight;
            case LastStep.Right:
                return currentTile.pipe.FromLeft;
            case LastStep.Up:
                return currentTile.pipe.FromBottom;
            default:
                return FlowDirection.Drop;
        }
    }

    private bool CanContinueMoving(FlowDirection direction)
    {
        if (direction == FlowDirection.Stop || direction == FlowDirection.Drop || direction == FlowDirection.Trash)
        {
            return false;
        }

        return !FindSink();
    }

    private IEnumerator ApplyFinalAction(FlowDirection nextDirection)
    {
        var currentTile = playBoard.GetTileForPosition(Column, Row);

        switch (nextDirection)
        {
            case FlowDirection.Drop:
                SwitchSound(dropClip);
                if (currentTile != null)
                {
                    currentTile.Block(this);
                }
                else
                {
                    yield return DelayedDestroy(dropClip.length);
                }
                break;
            case FlowDirection.Stop:
                var mixerPipe = currentTile?.GetComponentInChildren<MixerPipe>();
                if (mixerPipe != null)
                {
                    mixerPipe.ProcessItem(this);
                }
                break;
            case FlowDirection.Trash:
                SwitchSound(trashClip);
                yield return DelayedDestroy(trashClip.length);
                break;
            default:
                Debug.Log("Unknown PipeType");
                break;
        }
    }

    private void SwitchSound(AudioClip clip)
    {
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }

        var particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        yield return new WaitForSeconds(delay);

        Destroy(this.gameObject);
    }

    public void StartMoving(FlowDirection direction)
    {
        StartCoroutine(MoveItem(direction));
    }

    private bool FindSink()
    {
        foreach (var itemSink in playBoard.itemSinks)
        {
            if (itemSink.row == Row && itemSink.column == Column)
            {
                itemSink.ProcessSinkItem(this);
                return true;
            }
        }

        return false;
    }

    private enum LastStep { Left, Right, Up, Down }
}
