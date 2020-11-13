#include "Stitch.h"


#include "Arduino.h"

Stitch::Stitch(int countX, int countY) {
   _stepX = 0;
   _stepY = 0;
   /*
   _countX = countX;
   _countY = countY;

   _x = new float(_countX);
   _y = new float(_countY);
   
   for (int i = 0; i < _countY; i++) {
      _y[i] = 0.8 * sin(TWO_PI *i / _countY);
   }

   _x[0] = 0.5; // zig zag
   //_y[0] = 0.5;
   //_y[1] = -0.5;

   */

   _countX = 5;
   _countY = 5;
   _x[0] = 0.010839325071358902;
   _y[0] = -0.8379493803519168;
   _x[1] = 0.7298478881381651;
   _y[1] = 0.18094717635581903;
   _x[2] = 0.7804314051378402;
   _y[2] = -0.4296681359973987;
   _x[3] = 0;
   _y[3] = 0.1845602847129384;
   _x[4] = 0.6503595042815336;
   _y[4] = -0.8307231636376776;

   _stepX = _countX - 1;
}

void Stitch::incStep(bool reverse) {
   if (reverse) {
      _stepX--;
      _stepY--;
      if (_stepX < 0) _stepX = _countX - 1;
      if (_stepY < 0) _stepY = _countY - 1;
   }
   else {
      _stepX++;
      _stepY++;
      if (_stepX >= _countX) _stepX = 0;
      if (_stepY >= _countY) _stepY = 0;
   }
}