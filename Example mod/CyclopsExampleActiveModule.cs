using Nautilus.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nautilus.Examples;
internal class CyclopsExampleActiveModule : CyclopsModuleBase
{
    public override TechType TechType => TechType.None;

    public override void OnClick()
    {
        Subtitles.Add("Action !");
    }
}
