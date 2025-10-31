using UnityEngine;
public class FootstepsSFX : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] footstepSounds;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    public float volume = 1;
    public float stepInterval = 0.5f;
    private float stepTimer;
    private bool isJumping;
    private bool isGrounded;

    public LayerMask textureLayer;
    public float checkRadius = 0.1f;
    private Texture2D currentTexture;
    Texture2D oldTexture;
    public footstepTextures[] footstepTextures;

    public void Awake() => audioSource = GetComponent<AudioSource>();

    void Update()
    {
        isGrounded = GetComponent<RigidbodyFirstPersonController>().m_IsGrounded;

        bool isMoving = IsPlayerMoving();

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        stepTimer -= Time.deltaTime;

        if (isGrounded && isMoving)
        {
            float currentStepInterval = isSprinting ? stepInterval / 1.5f : stepInterval;

            if (stepTimer <= 0)
            {
                PlayRandomFootstep();
                stepTimer = currentStepInterval;
            }
        }

        if (!isGrounded && !isJumping)
        {
            isJumping = true;
            if (stepTimer <= 0)
            {
                PlayJumpSound();
                stepTimer = stepInterval;
            }
        }


        if (isJumping && isGrounded)
        {
            isJumping = false;
            if (stepTimer <= 0)
            {

                PlayLandingSound();

                stepTimer = stepInterval;
            }
        }

        if (oldTexture != currentTexture)
        {
            ChangeFootsteps();
        }

        oldTexture = currentTexture;
        CheckTexture();
    }
    bool IsPlayerMoving()
    {
        Vector3 playerVelocity = GetComponent<RigidbodyFirstPersonController>().Velocity;
        return playerVelocity.magnitude > 0.1f;
    }

    void PlayRandomFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(1, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index], volume);
            AudioClip clip = footstepSounds[index];
            footstepSounds[index] = footstepSounds[0];
            footstepSounds[0] = clip;
        }
    }
    bool firstJump;
    void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            if (firstJump)
                audioSource.PlayOneShot(jumpSound, volume);
            else firstJump = true;
        }
    }

    void PlayLandingSound()
    {
        if (landingSound != null)
        {
            audioSource.PlayOneShot(landingSound, volume);
        }
    }

    public void ChangeFootsteps()
    {

        for (int i = 0; i < footstepTextures.Length; i++)
        {
            for (int j = 0; j < footstepTextures[i].textures.Length; j++)
            {
                if (footstepTextures[i].textures[j] == currentTexture)
                {
                    footstepSounds = footstepTextures[i].clips;
                    jumpSound = footstepTextures[i].jump;
                    landingSound = footstepTextures[i].land;
                    volume = footstepTextures[i].volume;
                    stepInterval = footstepTextures[i].stepInterval;
                    break;
                }
            }
        }
    }

    void CheckTexture()
    {
        Vector3 playerPosition = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(playerPosition, -transform.up, out hit, checkRadius, textureLayer))
        {
            Terrain terrain = hit.collider.gameObject.GetComponent<Terrain>();
            if (terrain != null)
            {
                currentTexture = GetTerrainTextureAtPosition(terrain, hit.point);
            }
            else
            {
                if (hit.collider.gameObject.GetComponent<Renderer>() != null)
                    currentTexture = hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
            }
        }
        else
        {
            currentTexture = null;
        }
    }

    Texture2D GetTerrainTextureAtPosition(Terrain terrain, Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPosition = terrain.transform.position;

        int mapX = Mathf.RoundToInt((position.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((position.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int textureIndex = 0;
        float maxMix = 0;

        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            if (splatmapData[0, 0, i] > maxMix)
            {
                maxMix = splatmapData[0, 0, i];
                textureIndex = i;
            }
        }

        return terrainData.terrainLayers[textureIndex].diffuseTexture;
    }
}

[System.Serializable]
public class footstepTextures
{
    public string nameSFX;
    public float volume = 0.75f;
    public float stepInterval = 0.65f;
    public Texture2D[] textures;
    public AudioClip[] clips;
    public AudioClip jump;
    public AudioClip land;
}
