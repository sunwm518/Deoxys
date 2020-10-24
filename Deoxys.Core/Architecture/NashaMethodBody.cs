using System.Collections.Generic;

namespace Deoxys.Core
{
    public class NashaMethodBody
    {
        public NashaMethod NashaMethod { get; }
        public List<NashaInstruction> Instructions { get; set; }

        public NashaMethodBody(NashaMethod nashaMethod)
        {
            NashaMethod = nashaMethod;
        }
    }
}