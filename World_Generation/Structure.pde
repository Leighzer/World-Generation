class Structure{
  
  public BuildingType type;
  public Plot p;
  
  public Structure(BuildingType type, Plot p){
    this.type = type; 
    this.p = p;
    
  }
  
  public void show(){
    
    PVector middle = new PVector(p.pos.x + p.size/(float)2, p.pos.y + p.size/(float)2);
    
   if(type == BuildingType.TREE){
     //Trunk
     stroke(0);
     fill(color(139,69,19));
     strokeWeight(0);
     rect(p.pos.x+p.size*.16, p.pos.y+p.size*.16, p.size*.68 , p.size*.68) ;
     
     //Leaves
     stroke(color(77,255,58));
     strokeWeight(p.size*0.33);
     point(middle.x - p.size*.33, middle.y);
     point(middle.x + p.size*.33 , middle.y);
     point(middle.x , middle.y+ p.size*.33);
     point(middle.x, middle.y- p.size*.33);
     point(middle.x, middle.y);
     
     //Detail Leaves
     strokeWeight(p.size*.16);
     point(middle.x + p.size*.16, middle.y + p.size*.16);
     point(middle.x - p.size*.16, middle.y + p.size*.16);
     point(middle.x + p.size*.16, middle.y - p.size*.16);
     point(middle.x - p.size*.16, middle.y - p.size*.16);
   }
   if(type == BuildingType.SNOW){
     stroke(255);
     strokeWeight(p.size*0.1);
     
     for(int i = 0; i < 50; i++){
        point( middle.x + randomSign()*random(p.size/2), middle.y + randomSign()*random(p.size/2));
     }
     
   }
    
  }
  
  
  
  
  
  
}

enum BuildingType{
  TREE(0), ROCK(1), SNOW(2), GRASS(3);
  private int value;
  private BuildingType(int value){
    this.value = value;
  }
  
  
}