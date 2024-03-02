namespace YolkMe {
  public static class EggPickupController {
    public static bool IsEgg(ItemDrop itemDrop) {
      return itemDrop.m_itemData.m_dropPrefab.name == "ChickenEgg";
    }

    public static readonly float EggPickupDelay = 1.5f;
    public static float LastEggPickupTime = 0f;

    public static bool CanPickupEgg(float currentTime) {
      return (currentTime - LastEggPickupTime) > EggPickupDelay;
    }
  }
}
