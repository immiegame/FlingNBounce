using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * IMPORTANT NOTE
 * UNITY DOES STUFF IN RADIANS
*/

public class JumpMan : MonoBehaviour
{
    public float Health = 10;
    public int HeartNum;

    public Rigidbody2D rb;

    public Vector2 MouseStartPos;
    public Vector2 MouseEndPos;

    public bool isMousePressed;
    public bool isOnGround;

    public float PreviousYComp;

    public GameObject platformGen;
    public PlatformGenerator Generator;

    public float MaxHeight = 0;
    public float TimeBelowMaxHeight = 0;
    public int DisplayHeight;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Text HeightText;

    private LineRenderer lineRend;
    public SpriteRenderer spriteRend;

    public Sprite PlayerSprite;
    public Sprite PlayerFlySprite;

    public float TimeToSwapToOtherSpr = 0.5f;

    public GameObject Whoosh;
    public float RespawnTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.5f;
        rb.drag = 1F;
        rb.angularDrag = 1F;
        rb.mass = 100;
        rb.freezeRotation = true;

        isOnGround = true;
        isMousePressed = false;

        platformGen = GameObject.Find("PlatformSpawner");
        if(platformGen != null)
        {
            Generator = platformGen.GetComponent<PlatformGenerator>();
            Generator.player = this;
        }

        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.enabled = false;
        //lineRend.startColor = Color.white;
        //lineRend.endColor = Color.white;
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        lineRend.material = whiteDiffuseMat;
        
        spriteRend = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        if (Health > 0)
        {
            
            if (transform.position.y < MaxHeight)
            {
                TimeBelowMaxHeight += Time.deltaTime;
                if (TimeBelowMaxHeight > 15)
                {
                    Health -= Time.deltaTime * 0.2f;
                }
            }

            if (true)
            {
                /*
                 * Handle mouse
                 */

                if (isMousePressed)
                {
                    UpdateMouseCurrentPos(Input.mousePosition);
                    Vector3 jumpVec = VectorToJump();
                    jumpVec *= 0.1f;
                    jumpVec.x += transform.position.x;
                    jumpVec.y += transform.position.y;
                    Vector3[] pos = { transform.position, jumpVec };
                    lineRend.SetPositions(pos);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    OnInputPressed(Input.mousePosition);
                    lineRend.enabled = true;
                }

                else if (Input.GetMouseButtonUp(0))
                {
                    OnInputReleased(Input.mousePosition);
                    lineRend.enabled = false;
                }


                /*
                 * Handle touch
                 */

                if(Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        OnInputPressed(Input.mousePosition);
                        lineRend.enabled = true;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        OnInputReleased(Input.mousePosition);
                        lineRend.enabled = false;
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        UpdateMouseCurrentPos(touch.position);
                        Vector3 jumpVec = VectorToJump();
                        jumpVec *= 0.1f;
                        jumpVec.x += transform.position.x;
                        jumpVec.y += transform.position.y;
                        Vector3[] pos = { transform.position, jumpVec };
                        lineRend.SetPositions(pos);
                    }

                    
                }
            }

            if (transform.position.y > MaxHeight)
            {
                MaxHeight = transform.position.y;
                TimeBelowMaxHeight = 0;
            }

            if (TimeToSwapToOtherSpr < 0.3)
            {
                TimeToSwapToOtherSpr += Time.deltaTime;
                if (TimeToSwapToOtherSpr >= 0.3)
                {
                    spriteRend.sprite = PlayerSprite;
                }
                else
                {
                    spriteRend.sprite = PlayerFlySprite;
                }
            }

            DisplayHeight = (int)(MaxHeight * 10);
            HeightText.text = DisplayHeight.ToString();


        }
        else 
        {
            spriteRend.sprite = PlayerFlySprite;
            RespawnTimer -= Time.deltaTime;
            if(RespawnTimer <= 0)
            {
                PlayerLose();
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void OnInputPressed(Vector2 Position)
    {
        isMousePressed = true;
        Vector3 mousePos = Position;
        MouseStartPos.x = mousePos.x;
        MouseStartPos.y = mousePos.y;
        MouseEndPos = MouseStartPos;
    }

    void OnInputReleased(Vector2 Position)
    {
        isMousePressed = false;
        Vector3 mousePos = Position;
        MouseEndPos.x = mousePos.x;
        MouseEndPos.y = mousePos.y;

        Vector2 Jump = VectorToJump();
        if (Jump.y > 1)
        {
            rb.velocity = Jump;
            isOnGround = false;
            //rb.gravityScale = 2;
            SpawnJumpParticles();
            Generator.HandlePlayerJump();
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.11f);
            TimeToSwapToOtherSpr = 0;
        }
    }

    void UpdateMouseCurrentPos(Vector2 Position)
    {
        MouseEndPos.x = Position.x;
        MouseEndPos.y = Position.y;
    }

    Vector2 VectorToJump()
    {
        float XComponent = 0;
        float YComponent = 0;
        float Max = 17;
        float Divisor = 24;

        XComponent = (MouseStartPos.x - MouseEndPos.x) / Divisor < Max ? (MouseStartPos.x - MouseEndPos.x) / Divisor : Max;
        YComponent = (MouseStartPos.y - MouseEndPos.y) / Divisor < Max ? (MouseStartPos.y - MouseEndPos.y) / Divisor : Max;

        return new Vector2(XComponent, YComponent);
    }

    void SpawnJumpParticles()
    {
        GameObject spawnedObject = Instantiate(Whoosh) as GameObject;
        spawnedObject.transform.position = transform.position;
    }

    public void AddHealth(int AmountToAdd)
    {
        Health += AmountToAdd;
        if(Health > 10)
        {
            Health = 10;
        }
    }
    public void RemoveHealth(int AmountToRemove)
    {
        Health -= AmountToRemove;
    }

    public void PlayerLose()
    {
        SceneManager.LoadScene("Game");
    }

    public void UpdateHealth()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < Health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
