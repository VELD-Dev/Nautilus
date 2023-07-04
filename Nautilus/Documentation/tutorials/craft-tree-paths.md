# Crafting Tree Paths

List of default crafting tree paths for use in the `CraftingGadget.WithStepsToFabricatorTab(...)` method, alongside any of the methods in the [CraftTreeHandler](xref:Nautilus.Handlers.CraftTreeHandler) class.

Remember that paths divided by slashes must always be separate strings when used in Nautilus functions. For example:

```csharp
// EX1: Add the prefab to the Advanced Materials tab.
craftingGadget.WithStepsToFabricatorTab("Resources", "AdvancedMaterials");

// EX2: Add the prefab to the root of the fabricator.
craftingGadget.WithStepsToFabricatorTab();

// EX3: Add the prefab to the Vehicles tab of the mobile vehicle bay.
craftingGadget.WithFabricatorType(CraftTree.Type.Constructor)
    .WithStepsToFabricatorTab("Vehicles");
```

## Fabricator (Fabricator):
Resources
  - BasicMaterials (Basic Materials)
    - [Craft nodes]
  - AdvancedMaterials (Advanced Materials)
    - [Craft nodes]
  - Electronics
    - [Craft nodes]

Survival
  - Water
    - [Craft nodes]
  - CookedFood (Cooked food)
    - [Craft nodes]
  - CuredFood (Cured food)
    - [Craft nodes]

Personal
  - Equipment
    - [Craft nodes]
  - Tools
    - [Craft nodes]

Machines
  - [Craft nodes]

Upgrades (**BZ only**)
  - ExosuitUpgrades (Exosuit upgrades)
    - [Craft nodes]
  - SeatruckUpgrades (Seatruck upgrades)
    - [Craft nodes]

#### Simplified list of paths
* `Resources/BasicMaterials`
* `Resources/AdvancedMaterials`
* `Resources/Electronics`
* `Survival/Water`
* `Survival/CookedFood`
* `Survival/CuredFood`
* `Personal/Equipment`
* `Personal/Tools`
* `Machines`
* `Upgrades/ExosuitUpgrades` (**BZ only**)
* `Upgrades/SeatruckUpgrades` (**BZ only**)

---

## Workbench (Modification station):
Not applicable; all crafting nodes are located in the root.

---

## Constructor (Mobile vehicle bay):
  - Vehicles
    - [Craft nodes]
  - Rocket (**SN1 only**)
    - [Craft nodes]
  - Modules (**BZ only**)
    - [Craft nodes]

#### Simplified list of paths
* `Vehicles`
* `Rocket` (**SN1 only**)
* `Modules` (**BZ only**)

---

## CyclopsFabricator:
Not applicable; all crafting nodes are located in the root.

---

## Centrifuge:
Not applicable; all crafting nodes are located in the root.

---

## MapRoom (Scanner Room Fabricator):
Not applicable; all crafting nodes are located in the root.

---

## SeamothUpgrades (Vehicle Upgrade Console):
CommonModules (Common Modules) (**SN1 only**)
  - [Craft nodes]

SeamothModules (Seamoth Modules) (**SN1 only**)
  - [Craft nodes]

ExosuitModules (Prawn Suit Modules)
  - [Craft nodes]

Torpedoes (Torpedoes) (**SN1 only**)
  - [Craft nodes]

SeaTruckUpgrades (Seatruck upgrades) (**BZ only**)
  - [Craft nodes]

#### Simplified list of paths
* `CommonModules` (**SN1 only**)
* `SeamothModules` (**SN1 only**)
* `ExosuitModules`
* `Torpedoes` (**SN1 only**)
* `SeaTruckUpgrades` (**BZ only**)

---

## PrecursorPartOrgans (Precursor crafting bench where you craft organs) (BZ only)
Not applicable; all crafting nodes are located in the root.

---

## PrecursorPartTissue (Precursor crafting bench where you craft tissue) (BZ only)
Not applicable; all crafting nodes are located in the root.

---

## PrecursorPartSkeleton (Precursor crafting bench whre you craft the skeleton) (BZ only)
Not applicable; all crafting nodes are located in the root.
