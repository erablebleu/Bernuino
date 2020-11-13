#ifndef STEPPER_H
#define STEPPER_H

#include "Arduino.h"

#define HIGH_PULSE_WIDTH   50    // min 1 µs | min ok : 50
#define LOW_PULSE_WIDTH    50    // min 1 µs | min ok : 50
#define SETUP_TIME         25    // should be 200 
#define HOLD_TIME          10    // should be 200 ns

#define STEP_MODE_FULL     24    // Full step
#define STEP_MODE_1        24    // Full step
#define STEP_MODE_2        48    //  1/2 step
#define STEP_MODE_4        96    //  1/4 step
#define STEP_MODE_8        192   //  1/8 step
#define STEP_MODE_16       384   // 1/16 step

//#define DEBUG

class Stepper {
   public:
      Stepper(int limitPin, int dirPin, int stepPin, int stepCount, float lowerValue, float upperValue);
      int goTo(int val);
      int goTo(float val);
      void init();
   private:
      int _limitPin;
      int _dirPin;
      int _stepPin;
      int _stepCount;
      int _position;
      bool _isInit;
      float _lowerValue;
      float _upperValue;
      void step();
      int step(int val);
};

#endif // !STEPPER_H

