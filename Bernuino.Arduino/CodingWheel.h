#ifndef CODING_WHEEL
#define CODING_WHEEL

#include "Settings.h"

class CodingWheel {
   public:
      CodingWheel(Settings* settings, int pin1, int pin2, int pin3);
      void read();
      bool isNewLap() { return _last_c3 && !_c3; }
      bool canMoveX() { return _c1; }
      bool canMoveY() { return !_c2; }
   private:
      void newRotation();
      int _pin1;
      int _pin2;
      int _pin3;
      bool _c1;
      bool _c2;
      bool _c3;
      bool _last_c1;
      bool _last_c2;
      bool _last_c3;
      long _lastRotationTime;
      long _lastDuration;
      Settings* _settings;
};

#endif // !CODING_WHEEL
