using Configurations;

namespace Core
{
    public class EngineConfiguration : Configuration
    {
        public static readonly string[] DefaultTypeAssemblies = {"Assembly-CSharp"};

        public string[] TypeAssemblies = DefaultTypeAssemblies;
    }
}
