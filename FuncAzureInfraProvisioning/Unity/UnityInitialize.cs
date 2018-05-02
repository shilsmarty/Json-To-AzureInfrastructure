using Microsoft.Practices.Unity;

namespace FuncAzureInfraProvisioning.Unity
{
    /// <summary>
    /// Global unity container which can be leveraged any where
    /// </summary>
    public static class UnityInitialize
        {
            static UnityInitialize()
            {
                Container = new UnityContainer();
            }

            /// <summary>
            /// Gets or sets the unity container.
            /// </summary>
            public static IUnityContainer Container { get; set; }

        }
    }
