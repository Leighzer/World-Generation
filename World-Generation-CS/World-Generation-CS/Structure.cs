namespace World_Generation_CS
{
    public class Structure
    {
        public BuildingType BuildingType { get; set; }
        public Plot Plot { get; set; }

        public Structure(BuildingType buildingType, Plot plot)
        {
            this.BuildingType = buildingType;
            this.Plot = plot;
        }
    }
}