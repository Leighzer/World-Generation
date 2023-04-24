namespace World_Generation_CS
{
    public class Structure
    {
        public BuildingType BuildingType;
        public Plot Plot;

        public Structure(BuildingType type, Plot p)
        {
            this.BuildingType = type;
            this.Plot = p;
        }
    }
}