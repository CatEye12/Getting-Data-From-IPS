namespace ComApi
{
    using Intermech.Interfaces.Plugins;
    using Intermech.Runtime.ComInterop.LocalServer;
    using System;
    using System.Runtime.InteropServices;

    internal sealed class Plugin : IPackage
    {
        public void Load(IServiceProvider serviceProvider)
        {
            if (ComHost.Configuration.ComSupportActive)
            {
                ComHost.ActivateClassFactory(typeof(TestObject));
            }
        }

        public void Unload()
        {
            if (ComHost.Configuration.ComSupportActive)
            {
                ComHost.DeactivateClassFactory(typeof(TestObject));
            }
        }

        public string Name
        {
            get { return "Пример доступа к IPS API через COM"; }
        }
    }
}