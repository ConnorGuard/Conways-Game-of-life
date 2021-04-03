---
title: <project Conways Game of life >
author: <Connor> <Guard> - <n10690352>
date: 19/09/2020
---

## Build Instructions
1) locate life.dll file within CMD using cd C:\<path>
2)the file location within this zip is: C:<path>\CAB201_2020S2_ProjectPartB_n10690352\Life\Life\bin\Debug\netcoreapp3.1
3) enter "dotnet life.dll" to run program with default settings


## Usage 
This program runs a visual simulation of the Game of Life. Each cell is represnted by a white square. 
Each generation has a set of rules applied to the grid. These rules are dependent on the number of neighbours each cell has.

1) Leave 1 space between arguments.

3) There are 13 options that can be set through command line arguments.

4) These options are: [--dimensions], [--periodic], [--random], [--seed], [--generations], [--max-update], [--step], [--neighbour], [birth], [--survival], [--memory], [--output], [--ghost].

5) Some of these options must be entered with a value.

6) Enter arguments after "dotnet life.dll" for example "dotnet life.dll --random 0.5"

7) Use the spacebar to progress through the program.

[--dimensions]
Description: Sets the dimensions of the grid.
Requires two parameters <rows> <columns>, these must be positive integers between 4 and 48 inclusive.

Usage: --dimensions <rows> <columns>
Default: 16 16
Note: I have made the validation except 4, however the display library fails to handle a window size this small, and may crash.
    
[--periodic]
Description: Enables and Disables periodic behaviour.
Periodic behaviour uses a toroidal array, this means that there are no borders.
This takes no parameters.

Usage: --periodic
Default: OFF

[--random]
Description: Sets the random factor.
The random factor is used as the percentage of alive to dead cells,
used in the creation of the random seed.
This option takes one value <probability>, this must be a float between 1 and zero inclusive.

Usage: --random <probability>
Default: 0.5
    
[--seed]
Description: sets the seed file path.
This is used to set the initial location of alive cells in the begining of the simulation.
This must be a valid .seed file path to work

usage: --seed <filePath>
Default: N/A

[--generations]
Description: Defines the number of generations the Game Of Life will run.
This must be a non-zero, positive integer.

usage: --generations <number>
Default: 50

[--max-update]
Description: sets the number of generations iterated per second.
must be a float between 1 and 30 inclusive.

Usage: --max-update <float>
Default: 5

[--step]
Description: Enables and disables step mode.
Step mode waits until the space bar is pressed, before going to the next generation.
This takes no values.

Usage: --step
Default: OFF

[--neighbour]
Description: Sets 3 options: 
neighbourhood type: moore or vonNeumann, describes the neighbour bounds; 
order: an integer between 1 and 10 inclusive, defines the size or radius of the neighbourhood; 
centre cell: true or flase, decides if you count the centre cell when you add up the alive neighbours.

Usage: --neighbour <neighbourhood> <order> <centre> e.g. --neighbour vonNeumann 3 true
Default: --neighbour moore 1 false
    
[--birth]
Description: sets the number of neighbours that would allow for new cells to be born next generation.
Must be a positive integer less than the max number of neighbours. can take an arbitrary number of paramteres.
You can input a range of values using an elipses "..." e.g. "4...10" would input values 4,5,6,7,8,9,10.

Usage: --birth <integer> e.g. --birth 2 3 4...10
Default: --birth 3
    
[--survival]
Description: sets the number of neighbours that would allow for new cells to survive to next generation.
Must be a positive integer less than the max number of neighbours. can take an arbitrary number of paramteres.
You can input a range of values using an elipses "..." e.g. "4...10" would input values 4,5,6,7,8,9,10.

Usage: --survival <integer> e.g. --survival 2 3 4...10
Default: --survival 2 3

[--memory]
Description: defines the number of previous generations stored. We use previous generations to search for a steady state.
The larger the memory, the greater periodicity we can identify. Must be an integer between 4 and 512 inclusive.

Usage: --memory <integer>
Default --memory 16

[--output]
Description: sets the output file path, used to write the last generation of cells into a .seed file.
Writes it as the version 2 seed.

usage: --output <filepath>
DefaultL: N/A

[--ghost] 
Description: Enables ghost mode, which displays the last 3 generations of dead cells.

Usage: --ghost
Default: OFF
...

## Notes 

...