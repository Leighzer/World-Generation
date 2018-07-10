
class Plot{
  PVector pos;
  int size;
  color c;
  Structure s;
  
  
  public Plot(PVector pos, int size, color c){
    this.pos = pos;
    this.size = size;
    this.c = c;
    
  }
  
  
  public void show(){
    stroke(0);
    fill(c);
    strokeWeight(0);
    rect(pos.x, pos.y, pos.x + size, pos.y + size); 
    
    if(s != null){
      s.show();
    }
    
  }
  
  public void updateColor(Center centerToUse, float d){
    //d = 1/d; 
    this.s = null;
    
    
    if(centerToUse.biome == Biome.PLAINS){
      if(d < 32){
        c = color(153,255,51);
      }
      else{
        c = color(0,255,0);
      }
    }
    
    if(centerToUse.biome == Biome.TUNDRA){
      
      if(d < 16){
        c = color(255,255,255);
      }
      else if(d < 32){
        c = color(102,178,255);
        this.s = new Structure(BuildingType.SNOW, this);
      }
      else{
        c = color(102,178,255);
      }
      
    }
    
    if(centerToUse.biome == Biome.FOREST){
      if(d< 32){
        c = color(0,133,0);
      }
      else{
        c = color(0,155,0);
      }
      if(random(1)>0.5){
        this.s = new Structure(BuildingType.TREE,this);
      }
    }
    
    if(centerToUse.biome == Biome.OCEAN){
      c = color(0,0,255);
    }
    
    if(centerToUse.biome == Biome.DESERT){
      if(d < 32){
        c = color(200, 180, 71);
      }
      else{
        c = color(229, 211, 101);
      }      
    }
    
    
  }
  
  
 
  
  
  
  
}