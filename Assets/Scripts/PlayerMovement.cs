using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Variables for movement and jump
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jetpackForce = 8f;
    public float gravityScale = 2f;

    // Jetpack fuel system
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 20f;
    public float fuelRechargeRate = 10f;
    private float currentFuel;
    private bool isRecharging = false;

    // UI Fuel Bar
    [SerializeField] private TMP_Text coinText;
    public Image fuelBar;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float moveInput;

    private int coinCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentFuel = maxFuel;
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        // Jetpack lift if fuel is available
        if (Input.GetKey(KeyCode.Space) && currentFuel > 0)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, Mathf.Lerp(rb.linearVelocity.y, jetpackForce, Time.deltaTime * 10f));
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            isRecharging = false;
        }
        else
        {
            if (!isRecharging)
            {
                StartCoroutine(RechargeFuel());
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                // Regular jump when grounded
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else
            {
                // Simulate gravity pull when airborne
                rb.gravityScale = gravityScale;
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
        }

        // Clamp fuel within bounds
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);

        // Update UI Fuel Bar
        if (fuelBar != null)
        {
            fuelBar.fillAmount = currentFuel / maxFuel;
        }
    }

    private IEnumerator RechargeFuel()
    {
        isRecharging = true;
        yield return new WaitForSeconds(0.5f); // Small delay before recharging

        while (currentFuel < maxFuel && !Input.GetKey(KeyCode.Space))
        {
            currentFuel += fuelRechargeRate * Time.deltaTime;
            yield return null;
        }
        isRecharging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin")) // Fuel pickup
        {
            coinCount++;
            coinText.text = coinCount.ToString();
            currentFuel = Mathf.Min(currentFuel + 10f, maxFuel);
            Destroy(other.gameObject);
        }
    }
}



// Test