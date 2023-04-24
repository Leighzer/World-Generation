namespace World_Generation_CS
{
    public class Structure
    {
        public BuildingType type;
        public Plot p;

        public Structure(BuildingType type, Plot p)
        {
            this.type = type;
            this.p = p;
        }
    }
}