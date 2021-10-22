using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    private Text _velocityText;
    private Text _walkingAngleText;
    private Text _isGroundedText;
    private Text _directionVectorText;
    private Text _localPlayerVectorText;
    private Player2D _player2D;

    // Start is called before the first frame update
    void Start()
    {
        _player2D = GameObject.Find("player").GetComponent<Player2D>();
        if (_player2D == null)
        {
            Debug.LogError("player2D is null");
        }

        _velocityText = gameObject.transform.Find("Velocity").gameObject.GetComponent<Text>();
        _walkingAngleText = gameObject.transform.Find("WalkingAngle").gameObject.GetComponent<Text>();
        _isGroundedText = gameObject.transform.Find("IsGrounded").gameObject.GetComponent<Text>();
        _directionVectorText = gameObject.transform.Find("DirectionVector").gameObject.GetComponent<Text>();
        _localPlayerVectorText = gameObject.transform.Find("LocalPlayerVector").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        SetVelocityText();
        SetWalkingAngle();
        SetIsGroundedText();
        SetDirectionVector();
        SetLocalPlayerVector();
    }

    private void SetWalkingAngle()
    {
        _walkingAngleText.text = "Walking angle: " + _player2D.GetWalkingAngle();
    }

    private void SetVelocityText()
    {
        Vector2 velocity = _player2D.GetCurrentVelocity();
        _velocityText.text = "Velocity: " + velocity;
    }

    private void SetIsGroundedText()
    {
        _isGroundedText.text = "Is grounded: " + _player2D.IsGrounded();
    }

    private void SetDirectionVector()
    {
        _directionVectorText.text = "Direction vector: " + _player2D.GetDirectionVector();
    }

    private void SetLocalPlayerVector()
    {
        _localPlayerVectorText.text = "Local player vector: " + _player2D.GetLocalVector();
    }
}