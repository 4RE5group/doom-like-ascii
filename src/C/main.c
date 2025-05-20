#include <stdio.h>
#include <stdlib.h>
#include <math.h>


#define gotoxy(x,y) printf("\033[%d;%dH", (y), (x))

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


// buffers 
char** screen;
char** map;

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
    float rayDirX = (float)cos(angle);
    float rayDirY = (float)sin(angle);

    float rayPosX = playerX;
    float rayPosY = playerY;

    float distance = 0.0f;

    while(1) {
        if(rayPosX < 0 || rayPosY < 0 || rayPosX > sizeof(map) || rayPosY > sizeof(map))
            return -1;

        if(distance > RENDER_DIST)
            return -1;

        distance+=rayStep;
        rayPosX+=rayDirX*rayStep;
        rayPosY+=rayDirY*rayStep;

        if(map[(int)rayPosY][(int)rayPosX] == '#')
            return distance;
    }
}

int main(int argc, char* argv[]) {
    // Alloc mem for screen buffer
    screen = malloc(SCREEN_HEIGHT * sizeof(char*));
    map = malloc(MAP_SIZE * sizeof(char*));


    if (screen == NULL || map == NULL) {
        fprintf(stderr, "Memory allocation failed\n");
        return 1;
    }
    // fill up the screen buffer
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        screen[y] = malloc(SCREEN_WIDTH * sizeof(char));
        if (screen[y] == NULL) {
            fprintf(stderr, "Memory allocation failed\n");
            // free previously allocated memory
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
    // now time for the map initialisation
    for(int y=0; y<MAP_SIZE; y++) {
        map[y] = malloc(MAP_SIZE * sizeof(char*));
        if(map[y] == NULL) {
            sprintf(stderr, "Memory allocation failed\n");
            for (int y = 0; y < MAP_SIZE; y++) {
                free(map[y]);
            }
            free(map);
            return 1;
        }
        for (int x = 0; x < MAP_SIZE; x++) {
            map[y][x] = ' ';
        }
    }

    // Clear screen
    printf("\e[1;1H\e[2J");
    
    int key;
    while(1) {
        key = getchar();
        printf("%d\n", key);
    }


    // Render
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            printf("%c", screen[y][x]);
        }
        printf("\n"); // Add newline for each row
    }





    // free allocated memory
    for (int y = 0; y < SCREEN_HEIGHT; y++) {
        free(screen[y]);
    }
    free(screen);
    // now map
    for (int y = 0; y < MAP_SIZE; y++) {
        free(map[y]);
    }
    free(map);

    return 0;
}
