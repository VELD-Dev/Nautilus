using Nautilus.Assets;
using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nautilus.Patchers;
internal class CyclopsModulesPatcher
{
    internal static IDictionary<TechType, ICustomPrefab> CustomModules = new SelfCheckingDictionary<TechType, ICustomPrefab>("CyclopsCustomModules");
}
