#ifndef STITCH
#define STITCH

#define POINT_SIZE 128

#include "Arduino.h"

class Stitch {
   public:
      Stitch(int countX, int countY);
      float getX() { return _x[_stepX]; }
      float getY() { return _y[_stepY]; }
      void incStep(bool reverse);
   private:
      float _x[POINT_SIZE];
      float _y[POINT_SIZE];
      int _countX;
      int _countY;
      int _stepX;
      int _stepY;
};

#endif // !STITCH
