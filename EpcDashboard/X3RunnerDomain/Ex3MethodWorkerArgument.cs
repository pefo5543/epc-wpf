using System;
using System.Reflection;

namespace Epc.ExternalRunnerDomain
{
    [Serializable]
    public class Ex3MethodWorkerArgument
    {
        public MethodInfo Ex3Method { get; set; }
        public Object[] MethodArguments { get; set; }
    }
}
