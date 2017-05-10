using UnityEngine;
using Assets.Scripts;
using Common.Enums;

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

    // Use this for initialization
    void Start ()
    {
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

	    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
	    {
            transform.position += 0.5f * _up;
	        transform.position += 0.5f * _right;
            _unityChan.transform.forward = new Vector3(-1f, 0, -1f);
            _animator.SetFloat("Speed", 0.5f);
	        return;
	    }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.position += 0.5f * _up;
            transform.position += 0.5f * _left;
            _unityChan.transform.forward = new Vector3(1f, 0, -1f);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            transform.position += 0.5f * _down;
            transform.position += 0.5f * _right;
            _unityChan.transform.forward = new Vector3(-1f, 0, 1f);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            transform.position += 0.5f * _down;
            transform.position += 0.5f * _left;
            _unityChan.transform.forward = new Vector3(1f, 0, 1f);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

        if (Input.GetKey(KeyCode.W))
	    {
            transform.position += _up;
            //transform.Rotate(0.0f, 180.0f, 0.0f);
            _unityChan.transform.forward = new Vector3(0,0,-1f);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

	    if (Input.GetKey(KeyCode.A))
	    {
            transform.position += _left;
            //transform.Rotate(0.0f, 90.0f, 0.0f);
            _unityChan.transform.forward = new Vector3(1f, 0, 0);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

	    if (Input.GetKey(KeyCode.S))
	    {
            transform.position += _down;
            //transform.Rotate(0.0f, 0.0f, 0.0f);
            _unityChan.transform.forward = new Vector3(0, 0, 1f);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }

	    if (Input.GetKey(KeyCode.D))
	    {
            transform.position += _right;
            //transform.Rotate(0.0f, 270.0f, 0.0f);
            _unityChan.transform.forward = new Vector3(-1f, 0, 0);
            _animator.SetFloat("Speed", 0.5f);
            return;
        }
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
