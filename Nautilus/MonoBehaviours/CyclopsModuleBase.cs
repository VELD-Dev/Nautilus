using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nautilus.MonoBehaviours;

/// <summary>
/// A class that is used as a base for Cyclops Modules with active interactions and with a button.
/// </summary>
public abstract class CyclopsModuleBase : MonoBehaviour
{
    public abstract TechType TechType { get; }

    /// <summary>
    /// If the module is a toggleable.
    /// </summary>
    public bool active;

    public bool mouseHover;

    /// <summary>
    /// Cyclops root class.
    /// </summary>
    public SubRoot subRoot;

    /// <summary>
    /// Text shown when the button of the module in the cockpit is hovered.
    /// </summary>
    public abstract string ButtonText { get; }

    /// <summary>
    /// Subtext shown when the button of the module in the cockpit is hovered.
    /// </summary>
    public virtual string ButtonSubtext { get; } = string.Empty;

    public virtual void Awake()
    {
        var upgradeConsoleIcon = gameObject.AddComponent<CyclopsUpgradeConsoleIcon>();
        upgradeConsoleIcon.upgradeType = 
    }

    public void Update()
    {
        if(mouseHover == true && !active)
        {
            HandReticle main = HandReticle.main;
            main.SetText(HandReticle.TextType.Hand, ButtonText, true, GameInput.Button.LeftHand);
            main.SetText(HandReticle.TextType.HandSubscript, ButtonSubtext, true, GameInput.Button.None);
        }
    }

    public virtual void OnMouseEnter()
    {
        if (Player.main.currentSub != subRoot)
            return;
        mouseHover = true;
    }

    public virtual void OnMouseExit()
    {
        if (Player.main.currentSub != subRoot)
            return;
        HandReticle.main.SetIcon(HandReticle.IconType.Default, 1f);
        this.mouseHover = false;
    }

    /// <summary>
    /// Event triggered when the button of the module is clicked in the Cyclops Driving HUD.
    /// </summary>
    public abstract void OnClick();
}
