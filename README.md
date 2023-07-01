# Evilang

Evilang is a toy scripting inspired by brainf**ck and Chicken. Similar to brainf**ck, Evilang also operates on an array of memory cells and has a pointer that point into first-cell memory. the cell size is range from 0 - 255.

The commands are: 

| No | Command | Description |
| --- | --- | --- |
| 1 | evil | Set the array size by 2 x total evil |
| 2 | e | Shift pointer to the left |
| 3 | v | Shift pointer to the right |
| 4 | i | Increment current memory cell. |
| 5 | l | Decrement current memory cell. |
| 6 | vi | Input a byte and store it in the current cell |
| 7 | ev | Output the character at the current cell |
| 8 | evi | Output the byte value at current cell |
| 9 | il | Start loop. |
| 10 | el | Stop loop if the cell at the pointer is zero. |

## Example
```
evil evil evil i evi e ii evi e iii evi e iiii evi e iiiii evi e iiiiii evi e iiiiiii evi

#Ouput: 1234567
```
