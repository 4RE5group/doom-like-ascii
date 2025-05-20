#include <stdio.h>
#include <stdlib.h>
#include <math.h>


#define SCREEN_HEIGHT 20
#define SCREEN_WIDTH 53
#define MAP_SIZE 10
#define RENDER_DIST 10

#define rayStep 0.1f
#define fov 90.0f

#define PI 3.1415

float playerX = 5.0f;
float playerY = 5.0f;
float playerAngle = 0.0f;


void trace_line(int l, int h, char c, char*** screen) {
    if (*screen == NULL) return;
    int padding = (int)((SCREEN_HEIGHT - h) / 2);
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        if (y >= padding && y < h + padding) {
            (*screen)[y][l] = c;
        } else {
            (*screen)[y][l] = ' ';
        }
    }
}

int distance(float angle) {
    
}

int main(int argc, char* argv[]) {
    // Alloc mem for screen buffer
    char** screen = malloc(SCREEN_HEIGHT * sizeof(char*));
    if (screen == NULL) {
        fprintf(stderr, "Memory allocation failed\n");
        return 1;
    }
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        screen[y] = malloc(SCREEN_WIDTH * sizeof(char));
        if (screen[y] == NULL) {
            fprintf(stderr, "Memory allocation failed\n");
            // Free previously allocated memory
            for (int i = 0; i < y; i++) {
                free(screen[i]);
            }
            free(screen);
            return 1;
        }
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            screen[y][x] = ' ';
        }
    }

    // Clear screen
    printf("\e[1;1H\e[2J");
    float degPerColumn = (2*PI)/SCREEN_WIDTH;
    float angle=0;
    for(int c=0; c<SCREEN_WIDTH; c++) {
       angle += degPerColumn;
       // y=ax+b
       for(int x=0; x<RENDER_DIST; x++) {
           // round(x,y) -> player pos in matrix
           // 
       }
       printf("%fx\n", cos(angle));
       trace_line(c, 4, '#', &screen);
    }

    // Render
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            printf("%c", screen[y][x]);
        }
        printf("\n"); // Add newline for each row
    }

    // Free allocated memory
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        free(screen[y]);
    }
    free(screen);

    return 0;
}

