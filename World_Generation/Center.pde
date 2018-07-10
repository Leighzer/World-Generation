
class Center{
  
  public PVector pos;
  public Biome   biome;
  public boolean isPopulated;
  
  
  
  public Center(PVector pos, boolean isPopulated, Biome biome){
    this.pos = pos;
    this.isPopulated = isPopulated;
    this.biome = biome;
  }
 
  
  
}

public enum Biome{
  
  FOREST(0), PLAINS(1), TUNDRA(2), DESERT(3), OCEAN(4);
  private int value;
  private Biome(int value){
    this.value = value;
  }
  
}
  
  