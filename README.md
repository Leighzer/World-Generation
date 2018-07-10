# World-Generation
This is a processing sketch that creates random grid worlds with relatively simple rules.

The goal of this program is to generate simple random worlds with a few varying biomes and structures.

How the world generation works:

We randomly place these objects called centers all around the grid space. These centers have an associated random biome with them. Any centers that qualify being in the north pole IE being too northern will either be tundra or ocean biomes. The same goes for centers placed too far south. This gives us ice cap looking poles. All other centers can have one of the following associated biomes:

Forest
Plains
Desert
Ocean

Now with our centers placed inside our grid space, it is time to start creating our plots or cells in the world. Every single plot checks against the list of centers and finds the closest one. Then based on the distance the plot will assume a color. Plots near a tundra center will be all white. Plots near a forest center will be a much darker green. Plots near a desert center will be a darker sand color etc. There is also a random chance that the plot will assume a structure from the centers biome, and draw that. For now there are only trees for the forest biome, and snow for the tundra.

After all the drawing, we have a randomized world with a smattering of different biomes. You can change the number of centers to have larger regions, and tweak the odds for some biomes to be more likely. In the end however simple looking compared to fractal world generation programs, I think this could be interesting to use in a top down game that used a grid world.
