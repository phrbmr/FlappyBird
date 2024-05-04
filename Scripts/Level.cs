using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    private const float PIPE_DESTROY_X_POSITION = -100f;

    private static Level instance;

    private List<Pipe> pipeList;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;

    public static Level GetInstance() { 
        return instance; 
    }
    public enum Difficulty {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private void Awake() {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
    }

    private void Start() {
        CreateGapPipes(50f, 20f, 20f);
    }

    private void Update() {
        HandlePipeMovement();
        HandlePipeSpawning();
    }

    private void SetDifficulty(Difficulty difficulty) {
        switch (difficulty) {
            case Difficulty.Easy:
                gapSize = 50f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                break;
            case Difficulty.Hard:
                gapSize = 30f;
                break;
            case Difficulty.Impossible:
                gapSize = 20f;
                break;
        }
    }

    private Difficulty GetDifficulty() {
        if (pipesSpawned >= 50) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private class Pipe {

        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform) {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move() {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition() {
            return pipeHeadTransform.position.x;
        }

        public void DestroySelf() {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }

    private void CreatePipe(float height, float xPosition, bool createBottom) {
        // Pipe Head setup
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom) {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - (PIPE_HEAD_HEIGHT * 0.5f);
        } else {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + (PIPE_HEAD_HEIGHT * 0.5f);
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        // Pipe Body positioning
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom) {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        } else {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipeList.Add(pipe);
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition) {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void HandlePipeSpawning() {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0) { 
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heigtEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heigtEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2F;
            float maxHeight = totalHeight - gapSize * .5f - heigtEdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);

            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    private void HandlePipeMovement() {
        for (int i=0; i<pipeList.Count; i++) {
            Pipe pipe = pipeList[i];
            pipe.Move();
            if(pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) { 
                pipe.DestroySelf();
                pipeList.Remove(pipe);
            }
        }
    }

    public int GetPipesSpawned() {
        return pipesSpawned; 
    }
}