namespace ComfyLib;

using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public static class UIResources {
  public static readonly Dictionary<string, Sprite> SpriteCache = new();

  public static Sprite GetSprite(string spriteName) {
    if (!SpriteCache.TryGetValue(spriteName, out Sprite cachedSprite)) {
      cachedSprite = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(s => s.name == spriteName);
      SpriteCache[spriteName] = cachedSprite;
    }

    return cachedSprite;
  }

  public static readonly Dictionary<string, Material> MaterialCache = new();

  public static Material GetMaterial(string materialName) {
    if (!MaterialCache.TryGetValue(materialName, out Material cachedMaterial)) {
      cachedMaterial = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(m => m.name == materialName);
      MaterialCache[materialName] = cachedMaterial;
    }

    return cachedMaterial;
  }

  public static readonly DefaultControls.Resources DefaultControlResources = new () {
    standard = GetSprite("UISprite"),
    background = GetSprite("Background"),
    inputField = GetSprite("InputFieldBackground"),
    knob = GetSprite("Knob"),
    checkmark = GetSprite("Checkmark"),
    dropdown = GetSprite("DropdownArrow"),
    mask = GetSprite("UIMask")
  };

  public static readonly TMP_DefaultControls.Resources TMPDefaultControlResources = new() {
    standard = GetSprite("UISprite"),
    background = GetSprite("Background"),
    inputField = GetSprite("InputFieldBackground"),
    knob = GetSprite("Knob"),
    checkmark = GetSprite("Checkmark"),
    dropdown = GetSprite("DropdownArrow"),
    mask = GetSprite("UIMask")
  };
}
