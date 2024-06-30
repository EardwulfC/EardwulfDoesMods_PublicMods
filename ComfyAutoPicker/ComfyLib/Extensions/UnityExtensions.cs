namespace ComfyLib;

public static class ChatExtensions {
  public static void AddMessage(this Chat chat, object obj) {
    if (chat) {
      chat.AddString($"{obj}");
      chat.m_hideTimer = 0f;
    }
  }
}

public static class ComponentExtensions {
  public static T GetOrAddComponent<T>(this UnityEngine.Component component) where T : UnityEngine.Component {
    if (!component.TryGetComponent(out T otherComponent)) {
      otherComponent = component.gameObject.AddComponent<T>();
    }

    return otherComponent;
  }

  public static T GetOrAddComponent<T>(this UnityEngine.Component component, out T otherComponent)
      where T : UnityEngine.Component {
    if (!component.TryGetComponent(out otherComponent)) {
      otherComponent = component.gameObject.AddComponent<T>();
    }

    return otherComponent;
  }
}

public static class UnityExtensions {
  public static T Ref<T>(this T unityObject) where T : UnityEngine.Object {
    return unityObject ? unityObject : null;
  }

  public static T SetName<T>(this T component, string name) where T : UnityEngine.Component {
    component.name = name;
    return component;
  }
}
