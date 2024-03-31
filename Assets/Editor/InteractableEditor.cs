using UnityEditor;
[CustomEditor(typeof(Interacable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interacable interacable = (Interacable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interacable.promptMessage = EditorGUILayout.TextField("Prompt Message", interacable.promptMessage);
            EditorGUILayout.HelpBox("EvenOnlyInteractable can ONLY use UnityEvents.", MessageType.Info);
            if (interacable.GetComponent<InteractionEvent>() == null)
            {
                interacable.useEvents = true;
                interacable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interacable.useEvents)
            {
                // we are using events, add the component
                if (interacable.GetComponent<InteractionEvent>() == null)
                {
                    interacable.gameObject.AddComponent<InteractionEvent>();
                }

            }
            else
            {
                //we are not using events, remove the component
                if (interacable.GetComponent<InteractionEvent>() != null)
                {
                    DestroyImmediate(interacable.GetComponent<InteractionEvent>());
                }
            }
        }
    }
}
