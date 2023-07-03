#if SUBNAUTICA
using Nautilus.Handlers;
using Nautilus.MonoBehaviours;
using Nautilus.Patchers;
using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nautilus.Assets.Gadgets;
public class CyclopsModuleGadget : Gadget
{
    /// <summary>
    /// Delegate that gets the crush depth.<br/>
    /// <c>-1f</c> is the default value.
    /// </summary>
    public Func<float> CrushDepth { get; private set; } = () => -1f;
   
    /// <summary>
    /// Delegate that gets whether the given depth is absolute or added as extra depth.
    /// </summary>
    public Func<bool> AbsoluteDepth { get; private set; } = () => false;

    /// <summary>
    /// Delegate that gets the energy cost of the upgrade.
    /// </summary>
    public Func<float> EnergyCost { get; private set; } = () => 0f;

    /// <summary>
    /// If the module is an active module, the ModuleClass MUST be given.<br/>
    /// It is used to render buttons on the HUD of the Cyclops.
    /// </summary>
    public CyclopsModuleBase ModuleClass { get; private set; }

    /// <summary>
    /// Delegate invoked when the module is added to the Cyclops.
    /// </summary>
    public Action<SubRoot, UpgradeConsole> delegateOnAdded;

    /// <summary>
    /// Delegate invoked when the module is removed from the Cyclops.
    /// </summary>
    public Action<SubRoot, UpgradeConsole> delegateOnRemoved;

    /// <summary>
    /// Constructs an equipment gadget.
    /// </summary>
    /// <param name="prefab"><inheritdoc cref="Gadget(ICustomPrefab)"/></param>
    public CyclopsModuleGadget(ICustomPrefab prefab) : base(prefab) { }

    /// <summary>
    /// Sets the new crush depth of the Cyclops.
    /// </summary>
    /// <param name="crushDepth">Depth in meters (positive).</param>
    /// <param name="absolute">Whether the depth is added to default depth or is the new depth.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithCrushDepth(float crushDepth, bool absolute = false)
    {
        CrushDepth = () => crushDepth;
        AbsoluteDepth = () => absolute;
        return this;
    }

    /// <summary>
    /// Sets the new crush depth of the Cyclops.
    /// </summary>
    /// <param name="crushDepth">A delegate that allows you to make the value configurable.</param>
    /// <param name="absolute">Wether the depth is added to default depth or is the new depth.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithCrushDepth(Func<float> crushDepth, bool absolute = false)
    {
        CrushDepth = crushDepth;
        AbsoluteDepth = () => absolute;
        return this;
    }

    /// <summary>
    /// Sets the new crush depth of the Cyclops.
    /// </summary>
    /// <param name="crushDepth">Depth in meters (positive).</param>
    /// <param name="absolute">A delegate that allows you to make the value configurable.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithCrushDepth(float crushDepth, Func<bool> absolute)
    {
        CrushDepth = () => crushDepth;
        AbsoluteDepth = absolute;
        return this;
    }

    /// <summary>
    /// Sets the new crush depth of the Cyclops.
    /// </summary>
    /// <param name="crushDepth">A delegate that allows you to make the value configurable.</param>
    /// <param name="absolute">A delegate that allows you to make the value configurable.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithCrushDepth(Func<float> crushDepth, Func<bool> absolute)
    {
        CrushDepth = crushDepth;
        AbsoluteDepth = absolute;
        return this;
    }

    /// <summary>
    /// Sets the energy cost of the module if it is an active module.
    /// <para>If modules are toggleable, the amount is consumed on a certain rate.</para>
    /// </summary>
    /// <param name="energyCost">Amount of energy consumed by the module.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithEnergyCost(float energyCost)
    {
        EnergyCost = () => energyCost;
        return this;
    }

    /// <summary>
    /// Sets the energy cost of the module if it is an active module.
    /// </summary>
    /// <param name="energyCost">A delegate that allows you to make the value configurable.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithEnergyCost(Func<float> energyCost)
    {
        EnergyCost = energyCost;
        return this;
    }

    /// <summary>
    /// Adds an action that is executed AFTER Nautilus' actions when the module is added to the Cyclops.
    /// </summary>
    /// <param name="onAdded">Action executed when the module is added.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithOnAdded(Action<SubRoot, UpgradeConsole> onAdded)
    {
        delegateOnAdded = onAdded;
        return this;
    }

    /// <summary>
    /// Adds an action that is executed AFTER Nautilus' actions when the module is removed to the Cyclops.
    /// </summary>
    /// <param name="onRemoved">Action executed when the module is removed.</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithOnRemoved(Action<SubRoot, UpgradeConsole> onRemoved)
    {
        delegateOnRemoved = onRemoved;
        return this;
    }

    /// <summary>
    /// For every module that has a button, this is a required field.
    /// <para>N.B. This will make a button based on many things associated in other places with this item.</para>
    /// </summary>
    /// <param name="moduleClass">A subclass of CyclopsModuleBase</param>
    /// <returns>A reference to this instance with changes applied.</returns>
    public CyclopsModuleGadget WithModuleClass(CyclopsModuleBase moduleClass)
    {
        ModuleClass = moduleClass;
        return this;
    }

    /// <inheritdoc/>
    protected internal override void Build()
    {
        if (prefab.Info.TechType is TechType.None)
        {
            InternalLogger.Error($"Prefab '{prefab.Info}' does not contain a TechType. Skipping {nameof(CyclopsModuleGadget)} build.");
            return;
        }

        CraftDataHandler.SetEnergyCost(prefab.Info.TechType, EnergyCost.Invoke());

        prefab.TryGetGadget(out EquipmentGadget equipmentGadget);
        if (equipmentGadget is null)
            return;

        if (equipmentGadget.EquipmentType != EquipmentType.CyclopsModule)
            throw new Exception($"The upgrade module type of item {prefab.Info.TechType} is not a EquipmentType.CyclopsModule. Please use an other gadget if your module is not a Cyclops module.");


        if (!AbsoluteDepth.Invoke() && CrushDepth.Invoke() > 0f)
        {
            SubRoot.hullReinforcement.Add(prefab.Info.TechType, CrushDepth.Invoke());
        }
        CyclopsModulesPatcher.CustomModules.Add(prefab.Info.TechType, prefab);
    }
}
#endif