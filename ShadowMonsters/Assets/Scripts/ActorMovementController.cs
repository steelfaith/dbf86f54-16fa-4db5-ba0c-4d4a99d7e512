using UnityEngine;
using Assets.Scripts;
using Assets.ServerStubHome;
using Common.Enums;
using Common.Messages.Requests;

public class ActorMovementController : MonoBehaviour
{
    public float ScaleFactor = 0.25f;
    private Vector3 _up;
    private Vector3 _down;
    private Vector3 _left;
    private Vector3 _right;
    private Animator _animator;

    private GameObject _unityChan;
    private TextLogDisplayManager textLogDisplayManager;
    private ClientConnectionManager _clientConnectionManager;

    // Use this for initialization
    void Start ()
    {
        _clientConnectionManager = FindObjectOfType(typeof(ClientConnectionManager)) as ClientConnectionManager;

        textLogDisplayManager = TextLogDisplayManager.Instance();
        _up = new Vector3(0, 0, -1f*ScaleFactor);
        _down = new Vector3(0, 0, 1f*ScaleFactor);
        _left = new Vector3(1f * ScaleFactor, 0, 0);
        _right = new Vector3(-1f * ScaleFactor, 0, 0);

        _unityChan = transform.GetChild(0).gameObject;
        _animator = _unityChan.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (!_animator.isInitialized) return;
        _animator.SetFloat("Speed", 0.0f);

	    var position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	    Vector3 forward = new Vector3();

	    GenerateDirectionalVetors(ref position, ref forward);

        if (transform.position != position)
        {
            UpdateTransform(position, forward);

            var serverVector = new Common.Vector3
            {
                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z
            };

            //_clientConnectionManager.SendMessage(new PlayerMoveRequest(serverVector));
        }

	}

    private void GenerateDirectionalVetors(ref Vector3 position, ref Vector3 forward)
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            position += 0.5f * _up;
            position += 0.5f * _right;
            forward = new Vector3(-1f, 0, -1f);
            return;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            position += 0.5f * _up;
            position += 0.5f * _left;
            forward = new Vector3(1f, 0, -1f);
            return;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            position += 0.5f * _down;
            position += 0.5f * _right;
            forward = new Vector3(-1f, 0, 1f);
            return;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            position += 0.5f * _down;
            position += 0.5f * _left;
            forward = new Vector3(1f, 0, 1f);
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            position += _up;
            forward = new Vector3(0, 0, -1f);
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position += _left;
            forward = new Vector3(1f, 0, 0);
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            position += _down;
            forward = new Vector3(0, 0, 1f);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            position += _right;
            forward = new Vector3(-1f, 0, 0);
            return;
        }
    }

    private void UpdateTransform(Vector3 newPosition, Vector3 forward)
    {
        transform.position = newPosition;
        _unityChan.transform.forward = forward;
        _animator.SetFloat("Speed", 0.5f);
    }

    /// <summary>
    /// this is exclusively used by rigid bodies
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
    }

    /// <summary>
    /// This is a better solution for us because it allows us to handle movement without invoking the standard physics engine
    /// for it to work make sure the game object has a collider and the trigger property of our actor is set to isKinematic = true
    /// we wont actually care about mesh intersection with the shadows at this point, if we even keep that mechanic at all
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        var shadow = collider.gameObject.GetComponent<Shadow>();

        if (shadow == null)
            return;

        //shadow.enabled = false;
        //shadow.gameObject.SetActive(false);
        //collider.enabled = false;
        //collider.gameObject.SetActive(false);
        var player = gameObject.GetComponent<PlayerController>();
        if(player.CaughtBetweenPlanes)
        {
            textLogDisplayManager.AddText("You are caught between the planes and cannot fight.  You need to find a planeswaker to return you to your realm first.", AnnouncementType.System);
            return;
        }

        //we will eventually need to pass data through this more research on that later i suppose
        //this is also currently nasty it just flash switches the entire view lol 
        if (AnyManager._anyManager.LoadCombatScene())
            Destroy(shadow);
    }
}
