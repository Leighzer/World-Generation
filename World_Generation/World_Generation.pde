
import toxi.math.noise.SimplexNoise;

  int plotSize = 10;
  int numberOfRows;
  int numberOfColumns;
  Plot[][]  plots;
  Center[] centers;
  int numberOfCenters = 2500;
  
  double noise = SimplexNoise.noise(random(Float.MAX_VALUE), random(Float.MAX_VALUE));
  
void setup(){
  //size(1000,600);
  fullScreen();
  numberOfRows = floor(width / plotSize);
  numberOfColumns = floor(height / plotSize);
  plots = new Plot[numberOfRows][numberOfColumns];  
  centers = new Center[numberOfCenters];
  
  initPlots();
  initCenters();
  
  updatePlots();
  drawPlots();
   
}

void initPlots(){
  for(int i = 0; i < numberOfRows; i++){
    for(int j = 0; j < numberOfColumns; j++){
      plots[i][j] = new Plot( new PVector(plotSize*i, plotSize*j), plotSize, color(0,0,0));
    }
  }
}

void initCenters(){
  for(int i = 0; i < centers.length; i++){
     centers[i] = new Center(new PVector(random(width), random(height)), randomBool(), getBiasedWarmLand()); 
  }
  
  for(int i = 0; i < centers.length; i++){
    if(!(centers[i].biome == Biome.OCEAN)){
        centers[i].biome = Biome.OCEAN;
      if(centers[i].pos.y / height < (1/(float)6) || centers[i].pos.y / height > (5/(float)6)){
        centers[i].biome = Biome.TUNDRA;
     }
       else{
        centers[i].biome = getRandomNotTundraOcean();
     }
    }
  }
  
}

void updatePlot(Plot p){
  float dist, closestDist;
  
  Center centerToUse;
  centerToUse = centers[0];
  closestDist = dist(p.pos.x, p.pos.y, centers[0].pos.x, centers[0].pos.y);
  for(int i = 1; i < centers.length; i++){
     dist = dist(p.pos.x, p.pos.y, centers[i].pos.x, centers[i].pos.y);
     if(dist < closestDist){
       closestDist = dist;
       centerToUse = centers[i];
     }
  }
  
  p.updateColor(centerToUse, closestDist);
  
}

void updatePlots(){
  for(int i = 0; i < numberOfRows; i++){
    for(int j = 0; j < numberOfColumns; j++){
      updatePlot(plots[i][j]);
    }
  }
}

void smoothLand(int a, int b){
  if(green(color(plots[a][b].c)) == green((color(165,255,42)))){
    stroke(255,0,255);
    strokeWeight(50);
    point(plots[a][b].pos.x, plots[a][b].pos.y);
    float numberOfAdjacentLands = 0;
    
     if(green(plots[a+1][b].c) == green((color(165,255,42)))){
       numberOfAdjacentLands += 1;
     }
     if(green(color(plots[a-1][b].c)) == green(color(165,255,42))){
       numberOfAdjacentLands += 1;
     }
     if(green(color(plots[a][b+1].c)) == green((color(165,255,42)))){
       numberOfAdjacentLands += 1;
     }
     if(green(color(plots[a][b-1].c)) == green((color(165,255,42)))){
       numberOfAdjacentLands += 1;
     }
     
     if(numberOfAdjacentLands < 1){
       plots[a][b].c = color(132,112,255);
     }
  
  println("Color: " + color(plots[a][b].c) + " Number Of Adjacent Lands: " + numberOfAdjacentLands);
  }
  
  
}

void smoothLands(){
  
  for(int i = 1; i < numberOfRows - 1; i++){
    for(int j = 1; j < numberOfColumns - 1; j++){
      smoothLand(i,j);
    }
  }
  
  
}
  
  

void keyPressed(){
  if(key == ' '){
    exit();
  }
  else if(key == 'r'){
    saveFrame();
  }
  initCenters();
  updatePlots();
  drawPlots();
}

void drawPlots(){
  for(int i = 0; i < numberOfRows; i++){
      for(int j = 0; j < numberOfColumns; j++){
        plots[i][j].show();
      }
  }
}

boolean randomBool(){
  return random(1) > 0.5;
}

int randomSign(){
   if(random(1) > 0.5){
     return 1;
   }
   else{
     return -1;
   }
}

Biome getRandomBiome(){
   int randomVal = floor(random(4.9999));
     if(randomVal == 0){
       return Biome.FOREST;
     }
     else if(randomVal == 1){
       return Biome.PLAINS;
     }
     else if(randomVal ==2){
       return Biome.TUNDRA;
     }
     else if(randomVal ==3){
       return Biome.OCEAN;
     }
     else{//(randomVal == 4)
       return Biome.DESERT;
     }
}

Biome getRandomNotTundra(){
  int randomVal = floor(random(4.9999));
     if(randomVal == 0){
       return Biome.FOREST;
     }
     else if(randomVal == 1){
       return Biome.PLAINS;
     }
     else if(randomVal ==3){
       return Biome.OCEAN;
     }
     else{//(randomVal == 4)
       return Biome.DESERT;
     }
}

Biome getRandomNotTundraOcean(){
  int randomVal = floor(random(2.9999));
     if(randomVal == 0){
       return Biome.FOREST;
     }
     else if(randomVal == 1){
       return Biome.PLAINS;
     }
     else{//(randomVal == 2)
       return Biome.DESERT;
     }
}

Biome getBiasedWarmLand(){
  int randomVal = floor(random(3.4999));
     if(randomVal == 0){
       return Biome.FOREST;
     }
     else if(randomVal == 1){
       return Biome.PLAINS;
     }
     else if(randomVal ==2){
       return Biome.DESERT;
     }
     else{//(randomVal == 3)
       return Biome.OCEAN;
     }

}

void draw(){
  
}
  