using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(FRIDAYMod), "F.R.I.D.A.Y. Test System", "0.1.0", "Foxbin")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

public sealed class FRIDAYMod : MelonMod
{
    private bool hudVisible = true;
    private bool diagnosticsVisible;
    private string targetName = "No target";
    private float targetDistance = -1f;
    private string lastEvent = "Waiting for system startup...";
    private float nextScan;

    private readonly GUIStyle title = new();
    private readonly GUIStyle text = new();
    private readonly GUIStyle box = new();

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("======================================");
        MelonLogger.Msg("F.R.I.D.A.Y. Test System 0.1.0");
        MelonLogger.Msg("System initialization complete.");
        MelonLogger.Msg("F8 = HUD | F9 = Diagnostics | F10 = Target Scan");
        MelonLogger.Msg("======================================");

        title.fontSize = 22;
        title.fontStyle = FontStyle.Bold;
        title.normal.textColor = Color.white;

        text.fontSize = 15;
        text.normal.textColor = Color.white;

        box.normal.background = MakeTexture(new Color(0.03f, 0.04f, 0.06f, 0.88f));
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            hudVisible = !hudVisible;
            lastEvent = hudVisible ? "HUD enabled." : "HUD disabled.";
            MelonLogger.Msg($"F.R.I.D.A.Y.: {lastEvent}");
        }

        if (Input.GetKeyDown(KeyCode.F9))
            RunDiagnostics();

        if (Input.GetKeyDown(KeyCode.F10))
            ScanTarget();

        if (Time.unscaledTime >= nextScan)
        {
            ScanTarget();
            nextScan = Time.unscaledTime + 0.25f;
        }
    }

    private void RunDiagnostics()
    {
        diagnosticsVisible = true;
        lastEvent = "Diagnostics complete.";
        MelonLogger.Msg("F.R.I.D.A.Y. diagnostics started.");
        MelonLogger.Msg($"Unity: {Application.unityVersion}");
        MelonLogger.Msg($"Platform: {Application.platform}");
        MelonLogger.Msg($"Scene: {SceneManager.GetActiveScene().name}");
        MelonLogger.Msg($"Target: {targetName}");
        MelonLogger.Msg("Core status: ONLINE");
    }

    private void ScanTarget()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            targetName = "No main camera";
            targetDistance = -1f;
            return;
        }

        Ray ray = new(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetName = hit.collider != null ? hit.collider.name : "Unknown object";
            targetDistance = hit.distance;
        }
        else
        {
            targetName = "No target";
            targetDistance = -1f;
        }
    }

    public override void OnGUI()
    {
        if (!hudVisible)
            return;

        float width = Mathf.Min(420f, Screen.width - 30f);
        float x = 15f;
        float y = 15f;

        GUI.Box(new Rect(x, y, width, 190f), GUIContent.none, box);
        GUI.Label(new Rect(x + 15f, y + 10f, width - 30f, 30f), "F.R.I.D.A.Y.", title);
        GUI.Label(new Rect(x + 15f, y + 45f, width - 30f, 24f), "IRON MAN DEVELOPMENT SYSTEM", text);
        GUI.Label(new Rect(x + 15f, y + 72f, width - 30f, 24f), "CORE STATUS: ONLINE", text);
        GUI.Label(new Rect(x + 15f, y + 98f, width - 30f, 24f), $"TARGET: {targetName}", text);

        string distance = targetDistance >= 0f ? $"{targetDistance:0.0} m" : "--";
        GUI.Label(new Rect(x + 15f, y + 124f, width - 30f, 24f), $"DISTANCE: {distance}", text);
        GUI.Label(new Rect(x + 15f, y + 150f, width - 30f, 24f), $"STATUS: {lastEvent}", text);

        if (diagnosticsVisible)
        {
            float dy = y + 205f;
            GUI.Box(new Rect(x, dy, width, 125f), GUIContent.none, box);
            GUI.Label(new Rect(x + 15f, dy + 10f, width - 30f, 24f), "DIAGNOSTICS", title);
            GUI.Label(new Rect(x + 15f, dy + 43f, width - 30f, 22f), $"Unity: {Application.unityVersion}", text);
            GUI.Label(new Rect(x + 15f, dy + 67f, width - 30f, 22f), $"Scene: {SceneManager.GetActiveScene().name}", text);
            GUI.Label(new Rect(x + 15f, dy + 91f, width - 30f, 22f), "Core: ONLINE", text);
        }
    }

    private static Texture2D MakeTexture(Color color)
    {
        Texture2D texture = new(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}
