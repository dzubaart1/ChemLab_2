namespace BioEngineerLab.Substances
{
    public class Substance
    {
        public float Weight
        {
            get
            {
                return _weight;
            }
        }
        public SubstanceProperty SubstanceProperty { get; private set; }

        private float _weight;
        
        public Substance(SubstanceProperty substanceProperty, float weight)
        {
            _weight = weight;
            SubstanceProperty = substanceProperty;
        }

        public Substance(Substance substance)
        {
            _weight = substance._weight;
            SubstanceProperty = substance.SubstanceProperty;
        }

        public void RemoveWeight(float weight)
        {
            _weight -= weight;
        }

        public void AddWeight(float weight)
        {
            _weight += weight;
        }
    }
}