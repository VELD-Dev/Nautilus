# SN1 Sprite Manager Groups

List of default Sprite Manager Groups for use in `SpriteManager.Get(SpriteManager.Group group, string name)`
method (overload of `SpriteManager.Get(string atlasName, string name)`).  
  
You must know that the SpriteManager is initialized very lately in the game launch. You may require an `IEnumerator` coroutine
to get it properly for your custom items, custom tabs, custome logs, etc...  
  
Example of use:  

```csharp
// Making a PDA Log Entry with a custom sprite instead of the default one.
PDAHandler.AddLogEntry(
    key: "MyLogKey"
    languageKey: "Subtitles_MyLogKey",
    sound: aRandomFMODAsset,
    icon: SpriteManager.Get(SpriteManager.Group.Log, "Pda")  // Although you may require an IEnumerator here,
                                                             // for example when the game finishes to load the main menu.
);

// Maybe you want to grab the portraits of Fred, the guy of the trailer?
var fredFace = SpriteManager.Get(SpriteManager.Group.Portraits, "Fred");

// Or you're looking for a tab icon, maybe?
var encyTabIcon = SpriteManager.Get(SpriteManager.Group.Tab, "TabEncyclopedia");
```

## Sprite Manager groups

- Item
  - Instead of using the Item group, just do `SpriteManager.Get(TechType)`.
- Background
  - There is really way too much backgrounds to quote them here, but basically that's all the HUD backgrounds, you better search in an asset bundle explorer.
- Category
  - categoryHovered
  - categoryNormal
- Log
  - Sunbeam
  - Aurora
  - **[BZ]** Human
  - Pda
  - Radio
  - Default
  - Unknown
- Tab
  - GroupExteriorModules
  - GroupBasePieces
  - GroupMiscellaneous
  - GroupInteriorModules
  - GroupInteriorPieces
  - TabInventory
  - TabJournal
  - TabEncyclopedia
  - TabTimeCapsule
  - TabLog
  - TabPing
  - TabGallery
- Pings
  - Still very unclear.
- ItemActions
  - Swap
  - Drop
- Portraits
  - Robin
  - Vinh
  - Alexis
  - Zeta
  - Danielle
  - Parvan
  - Sam
  - Marguerit
  - Fred
  - Emmanuel
  - Lilian
  - Jeremiah

That's basically all.
