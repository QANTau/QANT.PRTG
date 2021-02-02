using SnmpSharpNet;

namespace QANT.PRTG
{
    public class SnmpAsnItem
    {
        public string Description { get; set; }
        public Vb Vb { get; set; }

        public SnmpAsnItem(string description)
        {
            Description = description;
        }
    }
}
