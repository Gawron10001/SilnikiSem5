// Lesson 03 – Step 1: Cinemachine follow camera + level bounds.
// Run from the menu: Tools > Lesson 03 > Setup Cinemachine Camera.
// Safe to run again: it reuses the objects it created and recalculates the bounds.
#if CINEMACHINE_3
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CameraSetupTool
{
    private const string VcamName = "CM Follow Camera";
    private const string BoundsName = "CameraBounds";
    private const float OrthoSize = 6f;
    private const float BoundsPadding = 1f;

    [MenuItem("Tools/Lesson 03/Setup Cinemachine Camera")]
    public static void Setup()
    {
        // 1. Player
        GameObject player = FindPlayer();
        if (player == null)
        {
            EditorUtility.DisplayDialog("Camera setup",
                "No Player found. Name the player object \"Player\" or tag it \"Player\" and try again.", "OK");
            return;
        }
        if (!player.CompareTag("Player"))
        {
            Undo.RecordObject(player, "Tag Player");
            player.tag = "Player"; // Step 2 (hazards, coins) relies on this tag too.
        }

        // 2. Main Camera + Brain
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            EditorUtility.DisplayDialog("Camera setup", "No camera tagged MainCamera in the scene.", "OK");
            return;
        }
        Undo.RecordObject(mainCam, "Setup Main Camera");
        mainCam.orthographic = true;
        if (!mainCam.TryGetComponent(out CinemachineBrain _))
            Undo.AddComponent<CinemachineBrain>(mainCam.gameObject);

        // 3. Follow camera
        GameObject vcamGo = GameObject.Find(VcamName);
        if (vcamGo == null)
        {
            vcamGo = new GameObject(VcamName);
            Undo.RegisterCreatedObjectUndo(vcamGo, "Create Follow Camera");
            vcamGo.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        }
        CinemachineCamera vcam = GetOrAdd<CinemachineCamera>(vcamGo);
        Undo.RecordObject(vcam, "Setup Follow Camera");
        vcam.Target.TrackingTarget = player.transform;
        vcam.Lens.OrthographicSize = OrthoSize;

        CinemachinePositionComposer composer = GetOrAdd<CinemachinePositionComposer>(vcamGo);
        Undo.RecordObject(composer, "Setup Position Composer");
        composer.CameraDistance = 10f;
        composer.Damping = new Vector3(0.5f, 0.5f, 0f);
        composer.Lookahead.Enabled = true;
        composer.Lookahead.Time = 0.2f;
        composer.Lookahead.Smoothing = 5f;
        composer.Lookahead.IgnoreY = true;
        composer.Composition.DeadZone.Enabled = true;
        composer.Composition.DeadZone.Size = new Vector2(0.1f, 0.1f);

        // 4. Level bounds
        GameObject boundsGo = GameObject.Find(BoundsName);
        if (boundsGo == null)
        {
            boundsGo = new GameObject(BoundsName);
            Undo.RegisterCreatedObjectUndo(boundsGo, "Create Camera Bounds");
        }
        boundsGo.layer = LayerMask.NameToLayer("Ignore Raycast");
        PolygonCollider2D poly = GetOrAdd<PolygonCollider2D>(boundsGo);
        Undo.RecordObject(poly, "Setup Camera Bounds");
        Undo.RecordObject(boundsGo.transform, "Setup Camera Bounds");
        boundsGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        boundsGo.transform.localScale = Vector3.one;
        poly.isTrigger = true; // the player must not collide with the bounds

        Bounds level = CalculateLevelBounds(player, boundsGo);
        level.Expand(BoundsPadding * 2f);
        Vector2 min = level.min, max = level.max;
        poly.pathCount = 1;
        poly.SetPath(0, new[]
        {
            new Vector2(min.x, min.y), new Vector2(min.x, max.y),
            new Vector2(max.x, max.y), new Vector2(max.x, min.y)
        });

        // 5. Confiner
        CinemachineConfiner2D confiner = GetOrAdd<CinemachineConfiner2D>(vcamGo);
        Undo.RecordObject(confiner, "Setup Confiner");
        confiner.BoundingShape2D = poly;
        confiner.Damping = 0f;
        confiner.InvalidateBoundingShapeCache();

        EditorSceneManager.MarkSceneDirty(vcamGo.scene);
        Selection.activeGameObject = vcamGo;
        Debug.Log($"[Lesson 03] Camera set up. Following '{player.name}', bounds {min} → {max}. " +
                  "Save the scene (Ctrl+S). Edit CameraBounds by hand if the level shape needs it.");
    }

    private static GameObject FindPlayer()
    {
        GameObject tagged = null;
        try { tagged = GameObject.FindWithTag("Player"); } catch (UnityException) { }
        return tagged != null ? tagged : GameObject.Find("Player");
    }

    private static T GetOrAdd<T>(GameObject go) where T : Component
    {
        return go.TryGetComponent(out T existing) ? existing : Undo.AddComponent<T>(go);
    }

    // Union of every sprite / tilemap renderer in the scene, except the player and the camera objects.
    private static Bounds CalculateLevelBounds(GameObject player, GameObject boundsGo)
    {
        bool hasAny = false;
        Bounds result = new Bounds(player.transform.position, new Vector3(20f, 12f, 0f));
        foreach (Renderer r in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (r.transform.IsChildOf(player.transform) || r.transform.IsChildOf(boundsGo.transform)) continue;
            if (!(r is SpriteRenderer) && !(r is UnityEngine.Tilemaps.TilemapRenderer)) continue;
            if (r.bounds.size == Vector3.zero) continue;
            if (!hasAny) { result = r.bounds; hasAny = true; }
            else result.Encapsulate(r.bounds);
        }
        // Make sure the player's start position is inside.
        result.Encapsulate(player.transform.position);
        return result;
    }
}
#endif
