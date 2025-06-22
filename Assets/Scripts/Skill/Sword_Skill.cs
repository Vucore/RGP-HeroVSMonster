using Unity.VisualScripting;
using UnityEngine;

public enum SwordType {
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    
    [SerializeField] private float freezeTimerDuration = 0.7f;
    [SerializeField] private float returnSwordSpeed = 12f;

    [Header("Sword")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 lanchForce; // hướng phóng
    [SerializeField] private float swordGravity; //trọng lực
    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefabs;
    [SerializeField] private Transform dotsParent;

    [Header("Bounce")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin")]
    [SerializeField] private float maxTravelDistance = 7f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 1f;
    [SerializeField] private float hitCooldown;


    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();

    }
    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }
    protected override void Update()
    {
        base.Update();
        //finalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetKey(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * lanchForce.x, AimDirection().normalized.y * lanchForce.y);


        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if(swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if(swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if(swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetUpSword(finalDir, swordGravity, player, freezeTimerDuration, returnSwordSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
        //newSword.GetComponent<Sword_Skill_Controller>().SetUpSword(finalDir, swordGravity);
    }
    #region Aim
    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefabs, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
                            AimDirection().normalized.x * lanchForce.x, 
                            AimDirection().normalized.y * lanchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t); //v + vt + at^2/2
        return position;
    }
    #endregion
}
