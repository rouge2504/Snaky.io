using System.Collections;
using CnControls;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager;

public class ArrowController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------
  
  [SerializeField] private Image _arrowImage = null;
  //[SerializeField] private SpriteRenderer _arrowSprite;

    [Header("Values")]
    [SerializeField] private Vector3 defaultArrowPosition;
  [SerializeField] private float _minDistance = 6f; 
  [SerializeField] private float _maxDistance = 20f; 
  [SerializeField] private float _smoothDistance = 3f;
  [SerializeField] private float _startPositionDistance = 40f;

  [SerializeField] private float _fadeInSpeed = 4f;
  [SerializeField] private float _fadeOutSpeed = 1f;
  [SerializeField] private float _moveForwardSpeed = 1f;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    public string HorizontalAxisName = "Horizontal";
  public string VerticalAxisName = "Vertical";
  
  protected VirtualAxis HorizintalAxis;
  protected VirtualAxis VerticalAxis;
  
  private Vector2 _startPoisition = Vector2.zero;
  private float _distance;
  private Vector2 _normalizedDifference;

  private Vector2 _screenCenter;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------
    
  private void OnEnable()
  {
        _screenCenter = Camera.main.WorldToScreenPoint(SnakeSpawner.Instance.playerTracker.transform.position);// _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
       //Camera.main.WorldToScreenPoint(GameManager.instance.playerSnake.GetComponentInChildren<SnakeHead>().transform.localPosition);

        _arrowImage.gameObject.SetActive(true);
        _arrowImage.color = Color.white; ;// LooknFeelDisplay._instance.selectedColorTemplate.colors[0];
      //  _arrowSprite.gameObject.SetActive(true);
      //  _arrowSprite.color = Color.white;// LooknFeelDisplay._instance.selectedColorTemplate.colors[0];

        HorizintalAxis = HorizintalAxis ?? new CnControls.VirtualAxis(HorizontalAxisName);
    VerticalAxis = VerticalAxis ?? new VirtualAxis(VerticalAxisName);

    CnInputManager.RegisterVirtualAxis(HorizintalAxis);
    CnInputManager.RegisterVirtualAxis(VerticalAxis);
    
    SetArrowAlpha(0f);
  }
    
    private void OnDisable()
    {
        StopAllCoroutines();
        _arrowImage.gameObject.SetActive(false);
        CnInputManager.UnregisterVirtualAxis(HorizintalAxis);
        CnInputManager.UnregisterVirtualAxis(VerticalAxis);
     //   StartCoroutine(FadeOutCoroutine());
    }

  public void OnDrag(PointerEventData eventData)
  {
       // _screenCenter = Camera.main.WorldToScreenPoint(SnakeSpawner.Instance.playerTracker.transform.position);//new Vector2(SnakeSpawner.Instance.playerTracker.transform.position.x, SnakeSpawner.Instance.playerTracker.transform.position.z);
        var difference = eventData.position - _startPoisition;
    
    _normalizedDifference = difference.normalized;
    _distance = Mathf.Clamp(difference.magnitude / _smoothDistance + _minDistance, _minDistance, _maxDistance);
    
   // UpdateArrowTransform();

    HorizintalAxis.Value = _normalizedDifference.x;
    VerticalAxis.Value = _normalizedDifference.y;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
        isArrowMoving = false;
       // _screenCenter = Camera.main.WorldToScreenPoint(SnakeSpawner.Instance.playerTracker.transform.position); //_screenCenter = new Vector2(SnakeSpawner.Instance.playerTracker.transform.position.x, SnakeSpawner.Instance.playerTracker.transform.position.z);
        StopAllCoroutines();
        _arrowImage.gameObject.SetActive(false);
        //StartCoroutine(FadeOutCoroutine());
    }

  public void OnPointerDown(PointerEventData eventData)
  {
        isArrowMoving = true;
        //_screenCenter = new Vector2(SnakeSpawner.Instance.playerTracker.transform.position.x, SnakeSpawner.Instance.playerTracker.transform.position.z);
        _arrowImage.gameObject.SetActive(true);
        _arrowImage.transform.localPosition = defaultArrowPosition;
        var playerHeadForward = SnakeSpawner.Instance.playerTracker.transform.forward;
    _distance = _startPositionDistance;
    _normalizedDifference = new Vector2(playerHeadForward.x, playerHeadForward.z);
  //  UpdateArrowTransform();
    
    _startPoisition = eventData.position - _normalizedDifference * _startPositionDistance;

        StopAllCoroutines();
     //   _arrowImage.gameObject.SetActive(false);
    StartCoroutine(FadeInCoroutine());
  }
  
  private void UpdateArrowTransform()
  {
    //Debug.LogFormat("Screen center:- {0} + Normalized diff:- {1} + Distance:- {2}", _screenCenter, _normalizedDifference, _distance);
    _arrowImage.rectTransform.position = _screenCenter + _normalizedDifference * _distance;
    var rotationZ = Mathf.Atan2(_normalizedDifference.y, _normalizedDifference.x) * Mathf.Rad2Deg;
    _arrowImage.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        
  }

  public IEnumerator FadeInCoroutine()
  {
          for (var i = _arrowImage.color.a; i <= 1f; i += Time.deltaTime * _fadeInSpeed)
          {
            SetArrowAlpha(i);
            yield return null;
          }
        yield return null;
        SetArrowAlpha(1f);
  }

  public IEnumerator FadeOutCoroutine()
  {
         for (var i = _arrowImage.color.a; i >= 0f; i -= Time.deltaTime * _fadeOutSpeed)
         {
           SetArrowAlpha(i);
           _distance += Time.deltaTime * _moveForwardSpeed;
            //_arrowImage.rectTransform.position = _screenCenter + _normalizedDifference * _distance;
            _arrowImage.transform.localPosition = new Vector3(0, 0, _arrowImage.transform.localPosition.z + 1);
                 yield return null;
         }

         SetArrowAlpha(0f);
        _arrowImage.transform.localPosition = defaultArrowPosition;
        yield return null;
    }

  private void SetArrowAlpha(float i)
  {
        var arrowImageColor = _arrowImage.color;//_arrowImage.color;
    arrowImageColor.a = i;
        _arrowImage.color = arrowImageColor;
  }
    bool isArrowMoving = false;
    void Update()
    {
        if (isArrowMoving)
        {
            _screenCenter = Camera.main.WorldToScreenPoint(SnakeSpawner.Instance.playerTracker.transform.position);
            UpdateArrowTransform();
        }
    }
    
}