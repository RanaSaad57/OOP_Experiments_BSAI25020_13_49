using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Events;

public static class WeighingBalanceLayoutFixer
{
    [MenuItem("OOP Lab/06 Fix Weighing Balance Layout")]
    public static void FixLayout()
    {
        GameObject module = GameObject.Find("WeighingBalanceModule");
        if (module == null)
        {
            Debug.LogError("No WeighingBalanceModule found in the scene.");
            return;
        }

        Transform scaleModel = module.transform.Find("ScaleModel");
        Transform uiPanel = module.transform.Find("BalanceUIPanel");

        if (scaleModel == null)
        {
            Debug.LogError("No ScaleModel child found under WeighingBalanceModule.");
            return;
        }

        if (uiPanel == null)
        {
            Debug.LogError("No BalanceUIPanel child found under WeighingBalanceModule.");
            return;
        }

        EnsureCloseButton(uiPanel);
        RelinkController(module, uiPanel);

        module.transform.position = Vector3.zero;
        module.transform.rotation = Quaternion.identity;
        module.transform.localScale = Vector3.one * 5f;

        scaleModel.localRotation = Quaternion.identity;
        scaleModel.localScale = Vector3.one;

        Bounds bounds = CalculateBounds(scaleModel);
        Vector3 centerLocal = module.transform.InverseTransformPoint(bounds.center);
        Vector3 minLocal = module.transform.InverseTransformPoint(bounds.min);

        scaleModel.localPosition -= new Vector3(centerLocal.x, minLocal.y, centerLocal.z);

        bounds = CalculateBounds(scaleModel);
        Vector3 maxLocal = module.transform.InverseTransformPoint(bounds.max);
        Vector3 sizeLocal = bounds.size / module.transform.localScale.x;

        uiPanel.localPosition = new Vector3(0f, maxLocal.y + 0.28f, -0.15f);
        uiPanel.localRotation = Quaternion.Euler(32f, 0f, 0f);
        uiPanel.localScale = Vector3.one * 0.00042f;

        AddClickTarget(scaleModel.gameObject, module.GetComponent<OOPLab.Modules.WeighingBalance.WeighingBalanceController>(), scaleModel);

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0f, 1.75f, -3.8f);
            mainCamera.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
            mainCamera.fieldOfView = 43f;
        }

        Selection.activeGameObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log("Weighing Balance layout fixed. Save or replace the prefab if it looks good.");
    }

    private static void RelinkController(GameObject module, Transform uiPanel)
    {
        var controller = module.GetComponent<OOPLab.Modules.WeighingBalance.WeighingBalanceController>();
        if (controller == null)
        {
            controller = module.AddComponent<OOPLab.Modules.WeighingBalance.WeighingBalanceController>();
        }

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("balanceUIPanel").objectReferenceValue = uiPanel.gameObject;
        serializedController.FindProperty("sampleNameInput").objectReferenceValue = FindChildComponent<TMP_InputField>(uiPanel, "SampleNameInput");
        serializedController.FindProperty("massInput").objectReferenceValue = FindChildComponent<TMP_InputField>(uiPanel, "MassInput");
        serializedController.FindProperty("sampleText").objectReferenceValue = FindChildComponent<TMP_Text>(uiPanel, "SampleText");
        serializedController.FindProperty("gramsText").objectReferenceValue = FindChildComponent<TMP_Text>(uiPanel, "GramsText");
        serializedController.FindProperty("kilogramsText").objectReferenceValue = FindChildComponent<TMP_Text>(uiPanel, "KilogramsText");
        serializedController.FindProperty("statusText").objectReferenceValue = FindChildComponent<TMP_Text>(uiPanel, "StatusText");
        serializedController.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void EnsureCloseButton(Transform uiPanel)
    {
        Transform existing = uiPanel.Find("CloseButton");
        if (existing != null)
        {
            return;
        }

        var controller = uiPanel.GetComponentInParent<OOPLab.Modules.WeighingBalance.WeighingBalanceController>();
        GameObject buttonObject = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(uiPanel, false);

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(220, 150);
        rectTransform.sizeDelta = new Vector2(44, 34);

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.58f, 0.16f, 0.19f, 1f);

        Button button = buttonObject.GetComponent<Button>();

        if (controller != null)
        {
            UnityEventTools.AddPersistentListener(button.onClick, controller.HidePanel);
        }

        GameObject labelObject = new GameObject("Label", typeof(RectTransform));
        labelObject.transform.SetParent(buttonObject.transform, false);
        TMP_Text label = labelObject.AddComponent<TextMeshProUGUI>();
        label.text = "X";
        label.fontSize = 18;
        label.fontStyle = FontStyles.Bold;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;

        RectTransform labelRect = label.rectTransform;
        labelRect.anchorMin = new Vector2(0.5f, 0.5f);
        labelRect.anchorMax = new Vector2(0.5f, 0.5f);
        labelRect.pivot = new Vector2(0.5f, 0.5f);
        labelRect.anchoredPosition = Vector2.zero;
        labelRect.sizeDelta = new Vector2(36, 28);
    }

    private static void AddClickTarget(GameObject scaleModel, OOPLab.Modules.WeighingBalance.WeighingBalanceController controller, Transform scaleModelTransform)
    {
        BoxCollider collider = scaleModel.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = scaleModel.AddComponent<BoxCollider>();
        }

        Bounds bounds = CalculateBounds(scaleModelTransform);
        collider.center = scaleModelTransform.InverseTransformPoint(bounds.center);
        Vector3 localSize = scaleModelTransform.InverseTransformVector(bounds.size);
        collider.size = new Vector3(Mathf.Abs(localSize.x), Mathf.Abs(localSize.y), Mathf.Abs(localSize.z));

        var clickTarget = scaleModel.GetComponent<OOPLab.Modules.WeighingBalance.WeighingBalanceClickTarget>();
        if (clickTarget == null)
        {
            clickTarget = scaleModel.AddComponent<OOPLab.Modules.WeighingBalance.WeighingBalanceClickTarget>();
        }

        clickTarget.SetController(controller);
    }

    private static T FindChildComponent<T>(Transform root, string childName) where T : Component
    {
        Transform child = root.Find(childName);
        return child != null ? child.GetComponent<T>() : null;
    }

    private static Bounds CalculateBounds(Transform root)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return new Bounds(root.position, Vector3.one);
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }
}
